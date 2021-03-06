﻿using System.Collections;
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

    GameObject playerControlsUIInstance;

    private void Awake()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        shooting = GetComponent<Shooting>();

        if (photonView.IsMine)
        {   
            playerControlsUIInstance = Instantiate(playerControlsUI);

            playerControlsUIInstance.transform.Find("Fire Button Right").GetComponent<Button>().onClick.AddListener(() => shooting.Fire());
            playerControlsUIInstance.transform.Find("Fire Button Left").GetComponent<Button>().onClick.AddListener(() => shooting.Fire());
            playerControlsUIInstance.transform.Find("Info").GetComponent<TextMeshProUGUI>().text = "Player Alive: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();

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


    private void Update()
    {
        if (!playerControlsUIInstance) { return; }

        playerControlsUIInstance.transform.Find("Info").GetComponent<TextMeshProUGUI>().text = "Player Alive: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }


    
}
