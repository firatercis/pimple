using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, SimpleUserTapInputListener
{
    public float speed = 10;
    public float roadBorder = 2;
    public bool canMove = true;
    public bool canSwerve = true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SimpleUserTapInput>().SetListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void OnDirectiveTap(Vector2 direction)
    {
        Debug.Log("On Directive Tap " + direction);
        if (canSwerve)
        {
            direction.y = 0;
            if (Mathf.Abs(transform.position.x + direction.x) < roadBorder)
                transform.Translate(direction);
        }
    }

    public void Disable()
    {
        canMove = false;
        canSwerve = false;
    }

    public void Enable()
    {
        canMove = true;
        canSwerve = true;
    }
}