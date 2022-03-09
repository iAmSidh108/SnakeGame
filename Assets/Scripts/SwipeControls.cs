using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    public enum swipeDirections
    {
        Up,
        Down,
        Left,
        Right
    }

    Vector2 swipeStart;
    Vector2 swipeEnd;
    float minDistance=10f;

    public static event System.Action<swipeDirections> OnSwipe = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began) //for swipe start
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)  //for swipe end
            {
                swipeEnd = touch.position;
                StartSwipeProcess();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
             swipeStart= Input.mousePosition;  //for  using mouse as checking
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            StartSwipeProcess();
        }
    }
    void StartSwipeProcess()
    {
        float distance =Mathf.Abs(Vector2.Distance(swipeEnd, swipeStart));
        if (distance > minDistance)         // then it is a swipe
        {
            if (IsVerticalSwipe())     //checks if it is up/down swipe
            {
                if (swipeEnd.y > swipeStart.y)
                {
                    OnSwipe(swipeDirections.Up);
                    //up
                }
                else
                {
                    OnSwipe(swipeDirections.Down);
                    //down
                }
            }
            
            else                //works if swipe is not up/down i.e. it is left or right
            {
                if (swipeEnd.x > swipeStart.x)
                {
                    OnSwipe(swipeDirections.Right);
                    //right
                }
                else
                {
                    OnSwipe(swipeDirections.Left);
                    //left
                }
            }

        }
    }
    bool IsVerticalSwipe()
    {
        float vertical = Mathf.Abs(swipeEnd.y - swipeStart.y);
        float horizontal = Mathf.Abs(swipeEnd.x - swipeStart.x);
        if (vertical > horizontal)
            return true;
        else
            return false;
    }
}
