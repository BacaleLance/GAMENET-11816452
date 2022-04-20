using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Shooting : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public GameObject bullet;
    public GameObject muzzle;
  
    void Start()
    {
        
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
        GameObject b = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * 500);
    }
}
