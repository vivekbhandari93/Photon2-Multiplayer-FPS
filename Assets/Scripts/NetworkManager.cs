using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    [SerializeField] Text connectionStatusText;

    [Header("Login Panel")]
    [SerializeField] InputField playerNameInput;
    [SerializeField] GameObject loginPanel;

    [Header("Game Options Panel")]
    [SerializeField] GameObject gameOptionsPanel;

    [Header("Create Room Panel")]
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] InputField roomNameInput;
    [SerializeField] InputField maxPlayersInput;


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

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInput.text;
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1, 1000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(maxPlayersInput.text);

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(gameOptionsPanel.name);
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ActivatePanel(gameOptionsPanel.name);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to room, " + PhotonNetwork.CurrentRoom.Name + ".");

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
