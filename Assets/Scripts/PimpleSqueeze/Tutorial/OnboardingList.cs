using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingList : MonoBehaviour
{
    public List<TutorialManager> tutorials;


    TutorialManager onGoingTutorial = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTutorialActive(int tutorialIndex, Transform highlightedObject = null)
    {
        tutorials[tutorialIndex].OnStart(highlightedObject);
        onGoingTutorial = tutorials[tutorialIndex];

    }

    public void HideTutorial(float delay = 0)
    {
        if(onGoingTutorial != null)
        {
            onGoingTutorial.OnEnd();
            onGoingTutorial = null;
        }
    }
}
