using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _firstTouchPos;
    private Vector2 _lastTouchPos;
    public double touchOffDragDistance;

    public Directions HandleInput()
    {
        Directions direction;

        direction = HandleKeyboardInput();
        if (direction == Directions.None)
        {
            direction = HandleSwipeInputs();
        }

        return direction;
    }
    
    
    private Directions HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Directions.Forward;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Directions.Back;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
             return Directions.Right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Directions.Left;
        }

        return Directions.None;
    }
    
            
    private Directions HandleSwipeInputs()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            var touch = Input.GetTouch(0); // get the touch
            
            //checking in which part of the screen the touch lies
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("touch input began");
                _firstTouchPos = touch.position;
                _lastTouchPos = touch.position;
            }
            
            switch (touch.phase)
            {
                /*
                 *the _lastTuchPos is updated
                 *it also handles the onScreenTouch drag input
                 */
                case TouchPhase.Moved:
                    _lastTouchPos = touch.position;
                    break;
                //check if the finger is removed from the screen
                case TouchPhase.Ended:
                    _lastTouchPos = touch.position;

                    var xDis = Mathf.Abs(_lastTouchPos.x - _firstTouchPos.x);
                    var yDis = Mathf.Abs(_lastTouchPos.y - _firstTouchPos.y);
                    
                    var xDir = xDis > touchOffDragDistance && xDis > yDis;
                    var yDir = yDis > touchOffDragDistance && xDis < yDis;

                    if (_lastTouchPos.x - _firstTouchPos.x > 0 && xDir)
                    {
                        return Directions.Right;
                    }

                    if (_lastTouchPos.x - _firstTouchPos.x < 0 && xDir)
                    {
                        return Directions.Left;
                    }

                    if (_lastTouchPos.y - _firstTouchPos.y < 0 && yDir)
                    {
                        return Directions.Back;
                    }

                    if (_lastTouchPos.y - _firstTouchPos.y > 0 && yDir)
                    {
                        return Directions.Forward;
                    }

                    break;
            }
        }

        return Directions.None;
    }
}
