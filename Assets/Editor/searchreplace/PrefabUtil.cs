using UnityEngine;
using UnityEditor;

namespace sr
{
  /**
   * Utility methods to find out useful bits about prefabs.
   */
  public class PrefabUtil
  {
    /**
     * Provides a method to determine if the given property on the given prefab instance is a modification to the existing prefab locally (in a scene).
     */
    public static bool isInstanceModification(SerializedProperty prop)
    {
      Object obj = prop.serializedObject.targetObject;
      Object prefab = PrefabUtility.GetPrefabObject(obj);
      if(prefab != null)
      {
        PropertyModification[] pms = PrefabUtility.GetPropertyModifications(obj);
#if UNITY_2018_2 || UNITY_2018_3 || UNITY_2018_4
        UnityEngine.Object parent = PrefabUtility.GetCorrespondingObjectFromSource(obj);
#else
        UnityEngine.Object parent = PrefabUtility.GetPrefabParent(obj);
#endif

        if(pms.Length > 0)
        {
          foreach(PropertyModification pm in pms)
          {
            // Debug.Log("[PrefabUtil] pm:"+pm.propertyPath);
            if(pm.target == parent && pm.propertyPath.StartsWith(prop.propertyPath) )
            {
              return true;
            }
          }
        }
      }
      return false;
    }

    public static bool isInPrefabInstance(UnityEngine.Object obj)
    {
      // Debug.Log("[PrefabUtil] prefab type:"+PrefabUtility.GetPrefabType(obj)+ " : "+ obj.name + " parent:"+PrefabUtility.GetPrefabObject(obj));
      return PrefabUtility.GetPrefabType(obj) == PrefabType.PrefabInstance;
    }


    public static bool isPrefab(UnityEngine.Object obj)
    {
      // Debug.Log("[PrefabUtil] prefab type:"+PrefabUtility.GetPrefabType(obj)+ " : "+ obj.name + " parent:"+PrefabUtility.GetPrefabObject(obj));
      return PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab;
    }

    public static bool isPrefabRoot(UnityEngine.Object obj)
    {
      if(obj == null)
      {
        return false;
      }
      if(obj is GameObject)
      {
        GameObject go = (GameObject) obj;
        if(isPrefab(go))
        {
          return PrefabUtility.FindPrefabRoot(go) == obj;
        }
      }
      return false;

    }

    public static GameObject getPrefabRoot(UnityEngine.Object obj)
    {
      if(obj == null)
      {
        return null;
      }
      if(obj is GameObject)
      {
        return getPrefabRoot((GameObject)obj);
      }
      if(obj is Component)
      {
        Component c = (Component)obj;
        return getPrefabRoot(c.gameObject);
      }
      return null;
    }

    public static GameObject getPrefabRoot(GameObject go)
    {
      PrefabType type = PrefabUtility.GetPrefabType(go);
      if(type == PrefabType.PrefabInstance)
      {
        go = PrefabUtility.FindPrefabRoot(go);
#if UNITY_2018_2 || UNITY_2018_3 || UNITY_2018_4
        go = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(go);
#else
        go = (GameObject)PrefabUtility.GetPrefabObject(go);
#endif
        return go;
      }
      if(type == PrefabType.Prefab)
      {
        return PrefabUtility.FindPrefabRoot(go);
      }
      return null;
    }

    public static void SwapPrefab(SearchJob job, SearchResult result, GameObject gameObjToSwap, GameObject prefab, bool updateTransform, bool rename)
    {
      Transform swapParent = gameObjToSwap.transform.parent;
      int index = gameObjToSwap.transform.GetSiblingIndex();

      result.replaceStrRep = prefab.name;
      result.strRep = gameObjToSwap.name;
      // Debug.Log("[ReplaceItemSwapObject] Instantiating:"  +prefab, prefab);
      GameObject newObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
      if(newObj != null)
      {
        newObj.transform.parent = swapParent;
        newObj.transform.SetSiblingIndex(index);
        Transform oldT = gameObjToSwap.transform;
        if(updateTransform)
        {
          newObj.transform.rotation = oldT.rotation;
          newObj.transform.localPosition = oldT.localPosition;
          newObj.transform.localScale = oldT.localScale;
        }
        if(rename)
        {
          newObj.name = gameObjToSwap.name;
        }
        result.pathInfo = PathInfo.GetPathInfo(newObj, job);

        UnityEngine.Object.DestroyImmediate(gameObjToSwap);
      }else{
        Debug.Log("[Search & Replace] No object instantiated...hrm!");
      }
    }
  }
}
