using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            int spawnPoint = Random.Range(1, 3);
            int pointX = 0;
            int pointZ = 0;

            if (spawnPoint == 1)
            {
                pointX = -16;
                pointZ = -19;
            }
            else if (spawnPoint == 2)
            {
                pointX = 5;
                pointZ = -21;
            }
            else if (spawnPoint == 3)
            {
                pointX = 11;
                pointZ = 1;
            }
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(pointX, 0, pointZ), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
