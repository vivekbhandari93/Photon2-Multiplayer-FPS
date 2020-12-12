using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

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

    [Header("Inside Room Panel")]
    [SerializeField] GameObject insideRoomPanel;
    [SerializeField] Text roomInfoText;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] Transform contentInInsideRoomPanel;
    [SerializeField] GameObject startGameButton;

    [Header("Join Random Room Panel")]
    [SerializeField] GameObject joinRandomRoomPanel;

    [Header("Room List Panel")]
    [SerializeField] GameObject roomListPanel;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform contentInRoomListPanel;


    Dictionary<string, RoomInfo> cachedRoomList;
    Dictionary<string, GameObject> roomInstancesList;

    Dictionary<int, GameObject> playerListGameObjects;


    #region Unity Methods

    void Start()
    {
        ActivatePanel(loginPanel.name);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomInstancesList = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;
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


    public void OnExitButtonClicked()
    {
        Application.Quit();
    }


    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInput.text.Trim();
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1, 1000);
        }

        RoomOptions roomOptions = new RoomOptions();

        if (!maxPlayersInput.text.All(char.IsDigit) || string.IsNullOrEmpty(maxPlayersInput.text.Trim())) { maxPlayersInput.text = "20"; }
        else if (double.Parse(maxPlayersInput.text) > 20 || double.Parse(maxPlayersInput.text) < 1) { maxPlayersInput.text = "20"; }
        roomOptions.MaxPlayers = byte.Parse(maxPlayersInput.text);

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(gameOptionsPanel.name);
    }


    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(roomListPanel.name);
    }


    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(gameOptionsPanel.name);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }


    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
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

        if(playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }

        ActivatePanel(insideRoomPanel.name);

        DisplayPlayerList();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        ClearRoomListView();

        foreach (RoomInfo room in roomList)
        {
            AddValidRoomToCachedRoomList(room);
        }

        AddingCacheRoomListToRoomListPrefab();
    }


    public override void OnLeftLobby()
    {
        base.OnLeftLobby();

        ClearRoomListView();
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " Players/Max Players: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerInstance = Instantiate(playerListPrefab);
        playerInstance.transform.SetParent(contentInInsideRoomPanel.transform);
        playerInstance.transform.localScale = Vector3.one;

        playerInstance.transform.Find("PlayerNameText").GetComponent<Text>().text = newPlayer.NickName;

        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerInstance.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            playerInstance.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }

        playerListGameObjects.Add(newPlayer.ActorNumber, playerInstance);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " + "Players/Max Players: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber]);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        foreach (GameObject playerGameObject in playerListGameObjects.Values)
        {
            Destroy(playerGameObject);
        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;

        ActivatePanel(gameOptionsPanel.name);
    }

    #endregion

    #region Public Methods

    public void ActivatePanel(string panelToActivate)
    {
        loginPanel.SetActive(panelToActivate.Equals(loginPanel.name));
        gameOptionsPanel.SetActive(panelToActivate.Equals(gameOptionsPanel.name));
        createRoomPanel.SetActive(panelToActivate.Equals(createRoomPanel.name));
        insideRoomPanel.SetActive(panelToActivate.Equals(insideRoomPanel.name));
        joinRandomRoomPanel.SetActive(panelToActivate.Equals(joinRandomRoomPanel.name));
        roomListPanel.SetActive(panelToActivate.Equals(roomListPanel.name));
    }

    #endregion

    #region Private Methods

    private void ClearRoomListView()
    {
        foreach(GameObject roomInstance in roomInstancesList.Values)
        {
            Destroy(roomInstance);
        }
        roomInstancesList.Clear();
    }

    private void AddValidRoomToCachedRoomList(RoomInfo roomToCheck)
    {
        if(!roomToCheck.IsOpen || !roomToCheck.IsVisible || roomToCheck.RemovedFromList)
        {
            if (cachedRoomList.ContainsKey(roomToCheck.Name))
            {
                cachedRoomList.Remove(roomToCheck.Name);
            }
        }
        else
        {
            if (cachedRoomList.ContainsKey(roomToCheck.Name))
            {
                cachedRoomList[roomToCheck.Name] = roomToCheck;
            }
            else
            {
                cachedRoomList.Add(roomToCheck.Name, roomToCheck);
            }
        }
    }


    private void AddingCacheRoomListToRoomListPrefab()
    {
        foreach(RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomInstance = Instantiate(roomListPrefab);
            roomInstance.transform.SetParent(contentInRoomListPanel.transform);
            roomInstance.transform.localScale = Vector3.one;

            roomInstance.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
            roomInstance.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers;
            roomInstance.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(()=> OnJoinRoomButtonClicked(room.Name));


            roomInstancesList.Add(roomInstance.name, roomInstance);
        }
    }

    private void OnJoinRoomButtonClicked(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    private void DisplayPlayerList()
    {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " + "Players/Max Players: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerInstance = Instantiate(playerListPrefab);
            playerInstance.transform.SetParent(contentInInsideRoomPanel.transform);
            playerInstance.transform.localScale = Vector3.one;

            playerInstance.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;

            if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerInstance.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                playerInstance.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }

            playerListGameObjects.Add(player.ActorNumber, playerInstance);
        }
    }

    #endregion

}
