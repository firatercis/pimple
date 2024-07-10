using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ObjectGroup
{
    public List<GameObject> elements;

    public ObjectGroup()
    {
        elements = new List<GameObject>();
    }

    public void Add(GameObject go)
    {
        elements.Add(go);
    }

}


[ExecuteAlways]
public class ObjectPermutator : MonoBehaviour
{
    //Settings

    // Connections
    public List<Transform> groupParents;
    // State Variables
    public List<ObjectGroup> groups;
    public List<GameObject> allObjects;
    Dictionary<Transform, int> groupsHash;
    // Start is called before the first frame update

    private void Awake()
    {
        InitConnections();
    }

    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        

    }

#if UNITY_EDITOR
    [ContextMenu("Load Pimples")]
    public void LoadPimples()
    {
        LoadObjects<AcneManagerRev>(this.transform, (x) => !IsBonusPimple(x));
    }

    private bool IsBonusPimple(GameObject pimpleGO) // TODO: Baska yere tasinabilir., bu class'in pimple'a bagimli olmamasi en iyisi.
    {
        AcneManagerRev pimple = pimpleGO.GetComponent<AcneManagerRev>();
        return pimple.pimpleType == AcneManagerRev.PimpleType.BonusPimple;
    }

    public void LoadObjects<T>(Transform root, Func<GameObject,bool> filterFunction = null) where T : Component // Should be called only in editor
    {
        groupsHash = new Dictionary<Transform, int>();
        groups = new List<ObjectGroup>();
        T[] componentsInChildren = root.GetComponentsInChildren<T>(includeInactive: true);
        allObjects = new List<GameObject>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            GameObject currentGO = componentsInChildren[i].gameObject;
            bool objectWillBeAdded = filterFunction != null ? filterFunction(currentGO) : true;
            if (!objectWillBeAdded) continue;

            allObjects.Add(currentGO);

            Transform currentParent = componentsInChildren[i].transform.parent;
            int currentParentIndex = -1;
            bool parentFound = groupsHash.TryGetValue(currentParent, out currentParentIndex);

            if (parentFound)
            {

                groups[currentParentIndex].Add(currentGO);
            }
            else
            {
                groupParents.Add(currentParent);
                ObjectGroup newGroup = new ObjectGroup();
                newGroup.Add(currentGO);
                groupsHash.Add(currentParent, groups.Count);
                groups.Add(newGroup);
            }
        }
        EditorUtility.SetDirty(gameObject);
    }

    [ContextMenu("Clear Objects")]
    public void ClearObjects()
    {
        allObjects.Clear();
        groups.Clear();
        groupParents.Clear();
        groupsHash.Clear();
        EditorUtility.SetDirty(gameObject);
    }
#endif
    public List<GameObject> SelectRandomNDifferentGroups(int n)
    {
        List<GameObject> selected = new List<GameObject>();

        List<ObjectGroup> groupsShuffled = groups.OrderBy((x) => Random.value).ToList();

        for(int i=0;i<n; i++)
        {
            int randomIndex = Random.Range(0, groupsShuffled[i].elements.Count);
            selected.Add(groupsShuffled[i].elements[randomIndex]);
        }

        return selected;

    }

    public List<GameObject> SelectAll()
    {
        List<GameObject> selected = new List<GameObject>();

        for (int i = 0; i < groups.Count; i++)
        {
            selected.AddRange(groups[i].elements);
        }

        return selected;

    }


    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {

    }

   

}

