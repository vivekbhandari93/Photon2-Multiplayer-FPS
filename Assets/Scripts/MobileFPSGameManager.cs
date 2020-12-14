using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class MobileFPSGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    Shooting shooting;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab)
            {
                float randomPointPosX = Random.Range(-40f, 40f);
                float randomPointPosZ = Random.Range(-40f, 40f);

                GameObject playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPointPosX, 80f, randomPointPosZ), Quaternion.identity);
                shooting = playerInstance.GetComponent<Shooting>();
            }
            else
            {
                Debug.Log("Player prefab not assigned!");
            }
        }
    }


    void Update()
    {
        if (shooting && shooting.isDead)
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
