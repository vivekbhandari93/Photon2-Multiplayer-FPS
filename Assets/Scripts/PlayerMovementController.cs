using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] FixedTouchField fixedTouchField;

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
