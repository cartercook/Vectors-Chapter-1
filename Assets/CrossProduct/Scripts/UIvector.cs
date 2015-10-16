using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIvector : MonoBehaviour {

	private Text x;
	private Text y;
	private Text z;

	// Use this for initialization
	void Start () {
		Text[] components = GetComponentsInChildren<Text>();
		x = components[0];
		y = components[1];
		z = components[2];
	}

	public virtual void set(Vector3 vector) {
		x.text = vector.x.ToString("0"); //0 means format with no decimals
		y.text = vector.y.ToString("0");
		z.text = vector.z.ToString("0");
	}
}
