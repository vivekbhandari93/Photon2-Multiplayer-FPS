using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerControlsUI;
    [SerializeField] Camera fpsCamera;

    PlayerMovementController playerMovementController;
    Shooting shooting;


    private void Awake()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        shooting = GetComponent<Shooting>();

        if (photonView.IsMine)
        {   
            GameObject playerControlsUIInstance = Instantiate(playerControlsUI);

            playerControlsUIInstance.transform.Find("Fire Button").GetComponent<Button>().onClick.AddListener(() => shooting.Fire());

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


    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = photonView.Owner.NickName;
    }
}
