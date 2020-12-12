using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        if (!playerControlsUIInstance) { return; }

        playerControlsUIInstance.transform.Find("Info").GetComponent<TextMeshProUGUI>().text = "Player Alive: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        playerControlsUIInstance.transform.Find("Quit Button").GetComponent<Button>().onClick.AddListener(()=>OnQuitButtonClicked());
    }


    private void Update()
    {
        if (!playerControlsUIInstance) { return; }

        playerControlsUIInstance.transform.Find("Info").GetComponent<TextMeshProUGUI>().text = "Player Alive: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }


    private void OnQuitButtonClicked()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        SceneManager.LoadScene(0);
    }
}
