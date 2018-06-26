using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
	public GameObject bulletPrefab;
	float speed = 0.1f;
	//float x;
	float y;
	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	[Command]
	void CmdFire()
	{
		// This [Command] code is run on the server!

		// create the bullet object locally
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			transform.position + transform.forward,
			Quaternion.identity);

		bullet.GetComponent<Rigidbody>().velocity = transform.forward*4;

		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// when the bullet is destroyed on the server it will automaticaly be destroyed on clients
		Destroy(bullet, 2.0f);
	}

	void Update()
	{
		if (!isLocalPlayer)
			return;
		var x = Input.GetAxis("Horizontal2")*0.03f;
		var z = Input.GetAxis("Vertical2")*0.03f;
		if(Input.GetMouseButton(0)){
			y = Input.GetAxis("Mouse X") * Time.deltaTime * speed*3 + Input.GetAxis("Mouse Y") * Time.deltaTime * speed*3;               
			　　　//　x = Input.GetAxis("Mouse Y") * Time.deltaTime * speed; 
		}else{
			　　　　 y = 0 ;
		}
		this.transform.Rotate(new Vector3(0,-y,0));

		transform.Translate(x, 0, z);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Command function is called from the client, but invoked on the server
			CmdFire();
		}
	}
}