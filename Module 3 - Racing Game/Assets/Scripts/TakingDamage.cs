using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;
public class TakingDamage : MonoBehaviourPunCallbacks
{
    public List<GameObject> deathTriggers = new List<GameObject>();

    [Header("Hp Related")]
    private float startHealth = 100;
    public float health;
    public Image healthBar;

    public Text playerNameText;
    public enum RaiseEventsCode
    {
        WhoDiedEventCode = 0
    }
    private int eliminationOrder = 0;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoDiedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfFinishedPlayer = (string)data[0];
            eliminationOrder = (int)data[1];
            int viewID = (int)data[2];

            Debug.Log(nickNameOfFinishedPlayer + " " + eliminationOrder);

            GameObject orderUiText = DeathRaceManager.instances.eliminationTextUI[eliminationOrder - 1];
            orderUiText.SetActive(true);

            if (viewID == photonView.ViewID) // this is you
            {
                orderUiText.GetComponent<Text>().text = eliminationOrder + " " + nickNameOfFinishedPlayer + " got eliminated! ";
                orderUiText.GetComponent<Text>().color = Color.red;
            }
            else
            {
                orderUiText.GetComponent<Text>().text = eliminationOrder + " " + nickNameOfFinishedPlayer + " got eliminated! " + "(YOU)";
            }
        }

    }
   
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;

        playerNameText.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;
        GameObject killLog = GameObject.Find("KillLog");

        if (health <= 0)
        {
            Die();
            killLog.GetComponent<Text>().text = info.Sender.NickName + " killed " + info.photonView.Owner.NickName;
            Debug.Log(info.Sender.NickName + "killed " + info.photonView.Owner.NickName);
        }
    }

    public void Die() 
    {
        GetComponent<PlayerSetup>().camera.transform.parent = null;
        GetComponent<VehicleMovement>().enabled = false;

        eliminationOrder++;

        string nickName = photonView.Owner.NickName;
        int viewID = photonView.ViewID;

        //event data
        object[] data = new object[] { nickName, eliminationOrder, viewID };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoDiedEventCode, data, raiseEventOptions, sendOption);

       
    }

   void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Bullet") && !col.gameObject.GetComponent<PhotonView>().IsMine)
        {
            col.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
        }
    }
}
