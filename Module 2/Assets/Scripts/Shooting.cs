using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    public Text playerNameText;
    public Text playerWinner;

    public int playerScore = 0;
    //public bool isDead;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        animator = this.GetComponent<Animator>();

        playerNameText.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject killText = GameObject.Find("Kill Text");
       killText.GetComponent<Text>().text = "Kills: " + playerScore.ToString();
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
       
        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
            }
        }
    }
    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;
        GameObject killLog = GameObject.Find("Kill Log");

        if (health <= 0)
        {
            Die();
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);

            killLog.GetComponent<Text>().text = info.Sender.NickName + " killed " + info.photonView.Owner.NickName;

            if (!gameObject.GetComponent<PhotonView>().IsMine)
            {
                gameObject.GetComponent<PhotonView>().RPC("KillScore", RpcTarget.AllBuffered);
            }
            Debug.Log(info.Sender.NickName + ": " + playerScore);
        }
    }
   
    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead" , true);
          
            StartCoroutine(RespawnCountdown());
        }
    }

     IEnumerator RespawnCountdown()
    {
        GameObject respawnText = GameObject.Find("Respawn Text");
        float respawnTime = 5.0f;

        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
           
            respawnText.GetComponent<Text>().text = "You are dead. Respawning in " + respawnTime.ToString(".00");
        }

        animator.SetBool("isDead", false);
      
        respawnText.GetComponent<Text>().text = "";

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
   
        this.transform.position = new Vector3(pointX, 0, pointZ);
        transform.GetComponent<PlayerMovementController>().enabled = true;
       
        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }

    [PunRPC]
    public void KillScore(PhotonMessageInfo info)
    {
        this.playerScore++;

        if (this.playerScore >= 10)
        {
            GameObject playerWinner = GameObject.Find("Winner Display");
            playerWinner.GetComponent<Text>().text = info.Sender.NickName + " wins!";
            StartCoroutine(ReturnLobbyCountdown());
            Debug.Log(info.Sender.NickName + "winner!");
        }
    }
    IEnumerator ReturnLobbyCountdown()
    {
        GameObject respawnText = GameObject.Find("Respawn Text");
        float respawnTime = 5.0f;

        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            
            respawnText.GetComponent<Text>().text = "Returning to Lobby in  " + respawnTime.ToString(".00");
            photonView.RPC("PlayerFreeze", RpcTarget.AllBuffered);
        }
        PhotonNetwork.LoadLevel("LobbyScene");

    }
    public void PlayerFreeze()
    {
        transform.GetComponent<PlayerMovementController>().enabled = false;
    }
}
