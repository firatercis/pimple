using UnityEngine;
using UnityEditor;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace sr
{
  /**
   * Various utilities provides to simplify scene-related actions. Used to be
   * a wrapper for functionality before the 'new' Scene Management API.
   */ 
  public class SceneUtil
  {

    public static Scene? LoadScene(string assetPath, OpenSceneMode mode)
    {
      // Debug.Log("[SceneSubJob] assetPath:"+assetPath);
      UnityEngine.Object sceneObj = AssetDatabase.LoadMainAssetAtPath(assetPath);
      if(sceneObj == null)
      {
        // this probably means we are in a new scene.
        // Debug.Log("[SceneUtil] path is missing for " + assetPath);
        return EditorSceneManager.GetActiveScene();
        // return null;
      }
      // Debug.Log("[SceneUtil] " + assetPath + " sceneObj:"+sceneObj, sceneObj);
      Scene s = EditorSceneManager.GetSceneByPath(assetPath);
      if(!s.IsValid())
      {
        // Debug.Log("[SceneUtil] attempting to open scene");
        s = EditorSceneManager.OpenScene(assetPath, mode);
      }
      EditorSceneManager.SetActiveScene(s); // PathInfo/ObjectID uses GetActiveScene(). Importanté!
      return s;
    }


    public static string GuidPathForActiveScene()
    {
      return EditorSceneManager.GetActiveScene().path;
    }

    public static bool SaveDirtyScenes()
    {
      return EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }


    public static IEnumerable<GameObject> SceneRoots()
     {
         var prop = new HierarchyProperty(HierarchyType.GameObjects);
         var expanded = new int[0];
         while (prop.Next(expanded)) {
             yield return prop.pptrValue as GameObject;
         }
     }

     public static void OpenObjectInScene(PathInfo pathInfo)
     {
       // Debug.Log("[SceneUtil] Opening object:" + pathInfo.FullPath());
       Scene? scene = SceneUtil.LoadScene(pathInfo.assetPath, OpenSceneMode.Single);
       if(scene.HasValue)
       {
         UnityEngine.Object obj = pathInfo.objID.searchForObjectInScene(scene.Value);
         Selection.activeObject = obj;
       }
     }

  }
}