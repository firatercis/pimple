using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TutorialManager : MonoBehaviour
{
    public float endTutorialTime;
    public RectTransform highlightMask;
    public GameObject[] endAnimationGOs;
    public string tutorialPrefsKey;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart(Transform highlightedPoint = null)
    {
        gameObject.SetActive(true);
        if(highlightedPoint != null)
        {
            Vector3 highlightPosition =  Camera.main.WorldToScreenPoint(highlightedPoint.position); // TODO: Yanlis ama calisiyor
            highlightMask.position = highlightPosition;
        }
    }
    
    public void OnEnd()
    {
        for(int i=0; i < endAnimationGOs.Length; i++)
        {
            DOTweenAnimation endAnimation = GetLastAnimationComponent(endAnimationGOs[i]);
            endAnimation.DOPlay();
        }
        Invoke(nameof(HideTutorial), endTutorialTime);
    }

    void HideTutorial()
    {
        gameObject.SetActive(false);
    }

    DOTweenAnimation GetLastAnimationComponent(GameObject target)
    {
        DOTweenAnimation[] animations =  target.GetComponents<DOTweenAnimation>();

        DOTweenAnimation result = animations.Length > 0 ?  animations[animations.Length - 1] : null;
        return result;

    }
}
