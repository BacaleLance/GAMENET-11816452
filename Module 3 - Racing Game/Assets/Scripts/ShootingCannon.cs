using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShootingCannon : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Camera fpsCamerac;

    [SerializeField]
    public GameObject bullet;
    public GameObject turret;

    public Text playerNameTextc;

    // Start is called before the first frame update
    void Start()
    {
        playerNameTextc.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();

        }
     }

  void Fire()
    {
        GameObject b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
    }
}
