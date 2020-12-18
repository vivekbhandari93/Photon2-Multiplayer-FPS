using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class PlayerMovementController : MonoBehaviourPunCallbacks
{
    public Joystick joystick;
    public FixedTouchField fixedTouchField;

    RigidbodyFirstPersonController rigidbodyFirstPersonController;
    Animator animator;
    Camera fpsCamera;

    Canvas playerControlsUI;
    bool isSprintButtonClicked = false;

    private void Start()
    {
        rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
        animator = GetComponent<Animator>();
        fpsCamera = GetComponentInChildren<Camera>();

        if (photonView.IsMine)
        {
            playerControlsUI = FindObjectOfType<Canvas>();
        }
    }

    private void FixedUpdate()
    {

        playerControlsUI.transform.Find("Sprint Button").GetComponent<Button>().onClick.AddListener(() => OnSprintButtonClicked());

        Move();
        UpdateFpsCameraPosition();

        if(Mathf.Abs(joystick.Horizontal) >= 0.1f)
        {
            isSprintButtonClicked = false;
        }
    }

    private void Move()
    {
        rigidbodyFirstPersonController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;

        if (isSprintButtonClicked)
        {
            rigidbodyFirstPersonController.joystickInputAxis.y = 1f;
            animator.SetFloat("vertical", 1f);
        }
        else
        {
            rigidbodyFirstPersonController.joystickInputAxis.x = joystick.Horizontal;
            rigidbodyFirstPersonController.joystickInputAxis.y = joystick.Vertical;


            animator.SetFloat("horizontal", joystick.Horizontal);
            animator.SetFloat("vertical", joystick.Vertical);
        }
    }


    private void UpdateFpsCameraPosition()
    {
        fpsCamera.transform.localPosition = new Vector3(0f, joystick.Vertical * -0.15f, 0f );
    }

    private void OnSprintButtonClicked()
    {
        isSprintButtonClicked = true;
    }
}
