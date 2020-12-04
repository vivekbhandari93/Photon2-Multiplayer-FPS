using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    [SerializeField] Text connectionStatusText;

    [Header("Login UI Panel")]
    [SerializeField] InputField playerNameInput;
    [SerializeField] GameObject loginPanel;

    [Header("Game Options UI Panel")]
    [SerializeField] GameObject gameOptionsPanel;

    [Header("Create Room Panel")]
    [SerializeField] GameObject createRoomPanel;

    [Header("Join Random Room Panel")]
    [SerializeField] GameObject joinRandomRoomPanel;

    [Header("Room List Panel")]
    [SerializeField] GameObject roomListPanel;


    #region Unity Methods

    void Start()
    {
        ActivatePanel(loginPanel.name);
    }

    
    void Update()
    {
        connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState.ToString();
    }

    #endregion

    #region UI Callbacks

    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName)) 
        {
            Debug.Log("Invalid name!");
            return; 
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ActivatePanel(gameOptionsPanel.name);
    }

    #endregion

    #region Public Methods

    public void ActivatePanel(string panelToActivate)
    {
        loginPanel.SetActive(panelToActivate.Equals(loginPanel.name));
        gameOptionsPanel.SetActive(panelToActivate.Equals(gameOptionsPanel.name));
        createRoomPanel.SetActive(panelToActivate.Equals(createRoomPanel.name));
        joinRandomRoomPanel.SetActive(panelToActivate.Equals(joinRandomRoomPanel.name));
        roomListPanel.SetActive(panelToActivate.Equals(roomListPanel.name));
    }

    #endregion
}
