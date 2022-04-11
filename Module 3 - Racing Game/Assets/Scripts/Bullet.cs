using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviour {

	
	void OnCollisionEnter(Collision col)
    {
    	Debug.Log(col.collider.gameObject.name);
		Destroy(this.gameObject);

		if (col.gameObject.CompareTag("Player") && !col.gameObject.GetComponent<PhotonView>().IsMine)
		{
			
			col.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
		}
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
