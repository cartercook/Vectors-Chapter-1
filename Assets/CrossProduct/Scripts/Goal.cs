using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	private CrossMaster master;
	private uint level;
	private CapsuleCollider crossProduct;

	void Start() {
		master = CrossMaster.instance as CrossMaster;
		level = 1;
		crossProduct = GameObject.Find("Cluster/CrossProduct").GetComponent<CapsuleCollider>();
	}

	void OnTriggerEnter(Collider other) {
		if (other != crossProduct) {
			return;
		}

		level++;
		switch (level) {
		//BASIC WARM UP
		case 2:
			transform.position = Vector3.down*2;
			break;
		case 3:
			master.setStaticArrow(Vector3.forward*2);
			transform.position = Vector3.up*4;
			break;
		case 4:
			transform.position = Vector3.down*4;
			break;
		case 5:
			master.setStaticArrow(Vector3.left*3);
			transform.position = Vector3.up*6;
			break;
		case 6:
			transform.position = Vector3.down*6;
			break;
		//USE ONLY INPUT-FILEDS AND REPEAT EARLIER STEPS
		case 7:
			transform.position = Vector3.up*2;
			master.setStaticArrow(Vector3.right);
			master.setMoveArrow(Vector3.forward);;
			master.showHalo();
			master.moveable = false;
			break;
		case 8:
			transform.position = Vector3.down*2;
			break;
		case 9:
			master.setStaticArrow(Vector3.forward*2);
			transform.position = Vector3.up*4;
			break;
		case 10:
			transform.position = Vector3.down*4;
			break;
		case 11:
			master.setStaticArrow(Vector3.left*3);
			transform.position = Vector3.up*6;
			break;
		case 12:
			transform.position = Vector3.down*6;
			break;
		case 13:
			transform.position = Vector3.up*2;
			master.setStaticArrow(Vector3.forward);
			master.showHalo();
			master.moveable = false;
			break;
		}
	}
}
