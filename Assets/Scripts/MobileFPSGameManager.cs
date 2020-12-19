using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileFPSGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<Transform> spawnPoints;


    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab)
            {
                int spawnPointIndex = Random.Range(0, spawnPoints.Count - 1);

                PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
                FindObjectOfType<Canvas>().transform.Find("Quit Button").GetComponent<Button>().onClick.AddListener(() => OnQuitButtonClicked());
            }
            else
            {
                Debug.Log("Player prefab not assigned!");
            }
        }
    }


    private void OnQuitButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
