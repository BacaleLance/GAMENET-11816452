using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviourPunCallbacks {

	
	void OnCollisionEnter(Collision col)
    {
    	
		Destroy(this.gameObject);

		if (col.gameObject.CompareTag("Player") && !col.gameObject.GetComponent<PhotonView>().IsMine)
		{
			Debug.Log(col.collider.gameObject.name);
			col.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
			//col.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
		}
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
