using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public virtual void stretchTo(Vector3 vector) {
		transform.rotation = Quaternion.FromToRotation(Vector3.up, vector); //rotate the cylinder
		//vector /= 2; //do this because transform in halfway up the cylinder
		transform.localScale = new Vector3(transform.localScale.x, vector.magnitude, transform.localScale.z); //stretch the cylinder
		//transform.position = vector; //reposition, because tranform is in center of cylinder
	}

	public Vector3 toVector() {
		return transform.up*transform.localScale.y;
		//return transform.position*2;
	}
}
