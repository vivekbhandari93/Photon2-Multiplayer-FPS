using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerControlsUI;
    [SerializeField] Camera fpsCamera;

    PlayerMovementController playerMovementController;


    private void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        if (photonView.IsMine)
        {   
            GameObject playerControlsUIInstance = Instantiate(playerControlsUI);

            playerMovementController.joystick = playerControlsUIInstance.transform.Find("Fixed Joystick").GetComponent<Joystick>();
            playerMovementController.fixedTouchField = playerControlsUIInstance.transform.Find("Rotation Touch Field").GetComponent<FixedTouchField>();

            fpsCamera.enabled = true;
        }
        else
        {
            fpsCamera.enabled = false;
            playerMovementController.enabled = false;
            GetComponent<RigidbodyFirstPersonController>().enabled = false;
        }
    }

}
