using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRaycastController : MonoBehaviour
{
    [SerializeField]
    private BactaController touchedController = null;

    [SerializeField]
    private LayerMask raycastingMask;

    private void Update()
    {
        var isButtonPressed = false;
        var screenPosition = Vector3.zero;

#if UNITY_EDITOR
        isButtonPressed = Input.GetMouseButton(0);
        if (isButtonPressed)
        {
            screenPosition = Input.mousePosition;
        }
#elif UNITY_IOS || UNITY_ANDROID
        isButtonPressed = Input.touchCount > 0;
        if (isButtonPressed)
        {
            screenPosition = Input.GetTouch(0).position;
        }
#endif

        if (isButtonPressed)
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
            var currentlyTouchedCollider = Physics2D.OverlapPoint(worldPoint, raycastingMask);

            if (currentlyTouchedCollider == null)
            {
                //no hit
                if (touchedController != null)
                {
                    //reset controller
                    touchedController.StopTouchingPhase();
                    touchedController = null;
                }
            }
            else
            {
                //some collider hit
                var currentlyTouchedController = currentlyTouchedCollider.transform.parent.GetComponent<BactaController>();
                if (touchedController == null)
                {
                    touchedController = currentlyTouchedController;
                    touchedController.StartTouchingPhase();
                }
                else if (touchedController != currentlyTouchedController)
                {
                    //change focus
                    touchedController.StopTouchingPhase();
                    touchedController = currentlyTouchedController;
                    touchedController.StartTouchingPhase();
                }
            }
        }
        else if (touchedController != null)
        {
            //reset controller
            touchedController.StopTouchingPhase();
            touchedController = null;
        }
    }
}
