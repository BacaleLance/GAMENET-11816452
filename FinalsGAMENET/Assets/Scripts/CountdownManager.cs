using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CountdownManager : MonoBehaviourPunCallbacks
{
    public Text timerText;

    public float timeToStartMatch = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        timerText = SparGameManager.instance.timeText;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeToStartMatch > 0)
            {
                timeToStartMatch -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timeToStartMatch);
            }
            else if (timeToStartMatch < 0)
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
            timerText.text = time.ToString("F1");
        }
        else
        {
            timerText.text = "";
        }
    }

    [PunRPC]
    public void StartRace()
    {
        GetComponent<MovementController>().isControlEnabled = true;
        this.enabled = false;
    }
}
