using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public interface UserInputButtonListener
{
    void OnButtonHeld();
    void OnButtonPressed();
    void OnButtonReleased();

}

public class UserInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Settings

    // Connections
    public UserInputButtonListener listener;
    // State Variables dafasdf
    bool isPressed;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            listener.OnButtonHeld();
        }   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        listener.OnButtonPressed();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        listener.OnButtonReleased();
    }
}

