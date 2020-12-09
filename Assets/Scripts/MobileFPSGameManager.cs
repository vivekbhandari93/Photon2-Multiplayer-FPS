using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MobileFPSGameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab)
            {
                float randomPoint = Random.Range(-10f, 10f);

                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
            }
            else
            {
                Debug.Log("Player prefab not assigned!");
            }
        }
    }


    void Update()
    {
        
    }
}
