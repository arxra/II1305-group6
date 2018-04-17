using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPlot {
    public enum SwipeDirection {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    private Vector2 accumulatedSwipeVelocity;
    private bool dead = false;

    public SwipeDirection GetSwipeDirection(float gestureLengthThreshold) {
        if (Input.touchCount > 0) {
            // Primary touch point - first finger found
            Touch primary = Input.GetTouch(0);

            switch (primary.phase) {
                case TouchPhase.Began:
                    accumulatedSwipeVelocity = Vector2.zero;
                    dead = false;
                    break;
                case TouchPhase.Moved:
                    accumulatedSwipeVelocity += Input.GetTouch(0).deltaPosition;
                    break;
            }         
        }

        SwipeDirection dir = SwipeDirection.NONE;
        if (!dead) {
            float max;
            
            switch (Mathf.Abs(accumulatedSwipeVelocity.x) < Mathf.Abs(accumulatedSwipeVelocity.y)) {
                case false: 
                    max = accumulatedSwipeVelocity.x;

                    if (max < -gestureLengthThreshold)
                        dir = SwipeDirection.LEFT;
                    else if (max > gestureLengthThreshold)
                        dir = SwipeDirection.RIGHT;
                    break;

                case true: 
                    max = accumulatedSwipeVelocity.y;

                    if (max < -gestureLengthThreshold)
                        dir = SwipeDirection.DOWN;
                    else if (max > gestureLengthThreshold)
                        dir = SwipeDirection.UP;
                    break;
            }
            
            if (dir != SwipeDirection.NONE)
                dead = true;
        }


        return dir;
    }

}