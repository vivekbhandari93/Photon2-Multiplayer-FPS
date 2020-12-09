using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerMovementController : MonoBehaviour
{
    public Joystick joystick;
    public FixedTouchField fixedTouchField;

    private RigidbodyFirstPersonController rigidbodyFirstPersonController;


    private void Start()
    {
        rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
    }

    private void FixedUpdate()
    {
        rigidbodyFirstPersonController.joystickInputAxis.x = joystick.Horizontal;
        rigidbodyFirstPersonController.joystickInputAxis.y = joystick.Vertical;

        rigidbodyFirstPersonController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;
    }
}
