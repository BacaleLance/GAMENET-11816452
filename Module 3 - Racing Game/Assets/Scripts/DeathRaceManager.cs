using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DeathRaceManager : MonoBehaviour
{
   

    public GameObject[] vehiclePrefabs;
    public Transform[] startingPositions;
    public GameObject[] eliminationTextUI;

    public static DeathRaceManager instances = null;
    public Text timeTextDR;
    void Awake()
    {
        if (instances == null)
        {
            instances = this;
        }
        else if (instances != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
   
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log((int)playerSelectionNumber);

                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 instantiatePosition = startingPositions[actorNumber - 1].position;
                PhotonNetwork.Instantiate(vehiclePrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
            }
        }

        foreach (GameObject go in eliminationTextUI)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitButton()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
