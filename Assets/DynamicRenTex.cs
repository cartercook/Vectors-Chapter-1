using UnityEngine;
using System.Collections;

public class DynamicRenTex : MonoBehaviour {
	RenderTexture renderTexture;

	void Awake() {

	}

	// Use this for initialization
	void Start() {
		Camera camera = GetComponent<Camera>();
		renderTexture = new RenderTexture(Screen.width/2, Screen.height/2, 0);
		renderTexture.Create();
		renderTexture.isPowerOfTwo = false;
		camera.targetTexture = renderTexture;
		camera.depth = -10; // force draw earlier than main camera
		GameObject staticArrow = GameObject.Find("Cluster/StaticArrow");
		staticArrow.GetComponent<Renderer>().material.SetTexture("_Detail", renderTexture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
