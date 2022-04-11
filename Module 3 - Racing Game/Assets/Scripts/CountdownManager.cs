using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    public Text timerText;
    public Text timerTextDR;
    public float timeToStartRace = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        {
            timerText = RacingGameManager.instance.timeText;
        }
       else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        {
            timerTextDR = DeathRaceManager.instances.timeTextDR;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeToStartRace > 0)
            {
                timeToStartRace -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timeToStartRace);
            }
            else if (timeToStartRace < 0)
            {
                photonView.RPC("StartRace", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void SetTime(float time)
    {
        if (time > 0)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
            {
                timerText.text = time.ToString("F1");
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
            {
                timerTextDR.text = time.ToString("F1");
            }

            
        }
        else 
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
            {
                timerText.text = "";
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
            {
                timerTextDR.text = "";
            }
           
        }
    }

    [PunRPC]
    public void StartRace()
    {
        GetComponent<VehicleMovement>().isControlEnabled = true;
        this.enabled = false;
    }
}
