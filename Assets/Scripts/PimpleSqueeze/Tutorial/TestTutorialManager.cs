using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTutorialManager : MonoBehaviour
{
    public TutorialManager target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            target.OnStart();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            target.OnEnd();
        }

    }
}
