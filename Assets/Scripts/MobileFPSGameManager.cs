using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileFPSGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab)
            {
                float randomPointPosX = Random.Range(-40f, 40f);
                float randomPointPosZ = Random.Range(-40f, 40f);

                GameObject playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPointPosX, 80f, randomPointPosZ), Quaternion.identity);

                FindObjectOfType<Canvas>().transform.Find("Quit Button").GetComponent<Button>().onClick.AddListener(()=> OnQuitButtonClicked());
            }
            else
            {
                Debug.Log("Player prefab not assigned!");
            }
        }
    }

    private void OnQuitButtonClicked()
    {
        Invoke("LeaveRoom", 1f);
    }


    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
