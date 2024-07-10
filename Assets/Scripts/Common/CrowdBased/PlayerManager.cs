using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    //Settings
    int additionValue;
    int multiplyValue;
    // Connections
    public GameObject cloneGO;
    public List<GameObject> clones;
    public List<Vector3> offsets;
    public float cloneConvergenceSpeed;
    // State Variables
    bool isMovingToOrigin;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){

    }
    void InitState(){

        foreach(GameObject clone in clones)
        {
            CloneCollision cloneCol = clone.GetComponent<CloneCollision>();

            cloneCol.GatePassed += GatePassed;
            cloneCol.DestroyClone += DestroyClone;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddOneClone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddMultipleClones();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MultiplyClones();
        }

        if (isMovingToOrigin)
        {
            foreach(GameObject clone in clones)
            {
                Vector3 offset = transform.position - clone.transform.position;
                clone.transform.Translate(offset * Time.deltaTime * cloneConvergenceSpeed);
            }
        }

        PlayerMovement();

    }

    void AddOneClone()
    {
        PushOutExistingClones();
        GameObject newClone = Instantiate(cloneGO, 
            new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y, transform.position.z + Random.Range(-0.5f, 0.5f)), 
            Quaternion.identity);
        newClone.transform.parent = transform;
        CloneCollision colCheck = newClone.GetComponent<CloneCollision>();
        colCheck.GatePassed += GatePassed;
        colCheck.DestroyClone += DestroyClone;
        clones.Add(newClone);

        
    }

    void AddMultipleClones()
    {
        for(int i = 0; i < additionValue; i++)
        {
            AddOneClone();
            
        }

        isMovingToOrigin = true;
        Invoke(nameof(CancelMovingToOrigin), 0.2f);
    }

    void MultiplyClones()
    {
        int startingCloneAmount = clones.Count;
        for(int i = 0; i < multiplyValue; i++)
        {
            for(int j = 0; j < startingCloneAmount; j++)
            {
                AddOneClone();
            }
        }

        isMovingToOrigin = true;
        Invoke(nameof(CancelMovingToOrigin), 0.4f);
    }

    void PushOutExistingClones()
    {
        for(int i = 0; i < clones.Count; i++)
        {
            Vector3 offset = transform.position - clones[i].transform.position;
            offset.y = 0;

            offset = offset / offset.magnitude;
            offset /= 2;

            clones[i].transform.position -= offset;
        }
    }

    void CancelMovingToOrigin()
    {
        isMovingToOrigin = false;
    }

    void GatePassed(bool isMultiplier, int value)
    {
        if (isMultiplier)
        {
            multiplyValue = value;
            MultiplyClones();
        }
        else
        {
            additionValue = value;
            AddMultipleClones();
        }
    }

    void DestroyClone(GameObject clone)
    {
        clones.Remove(clone);
        Destroy(clone);
    }

    void PlayerMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 6);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * 4);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 4);

        }

    }

}
