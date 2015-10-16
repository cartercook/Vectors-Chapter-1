using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * pretty crappy as is, just allows me to reference other objects programmatically
 * when they need to interact.
 */
public class DotMaster : Master {

	private Arrow staticArrow;
	private Transform numLine;
	private UIvector vector1; //mapped to staticArrow

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		base.initialise(); //instantiates moveArrow, vector2, and halo
		staticArrow = GameObject.Find("Cluster/StaticArrow").GetComponent<Arrow>();
		numLine = GameObject.Find("Cluster/StaticArrow/NumLine").transform;
		vector1 = GameObject.Find("Canvas/Header/Vector1").GetComponent<UIvector>(); //mapped to staticArrow
		vector1.set(staticArrow.toVector()); //staticArrow does not move
	}

	//MoveableArrow calls this in its onDrag function
	public override void updateCrossProduct() {
		Vector3 moveVec = moveArrow.toVector();
		Vector3 statVec = staticArrow.toVector();
		Debug.Log(moveVec+", "+", "+statVec+", "+Vector3.Dot(moveVec, statVec));
		numLine.localScale = new Vector3(1, Vector3.Dot(moveVec, staticArrow.toVector()), 1);
		vector2.set(moveVec); //update moveArrow UI
	}

	public void setStaticArrow (Vector3 vector) {
		staticArrow.stretchTo(vector);
	}

	//getters and setters
	public bool moveable {
		set {moveArrow.controlable = value;}
	}

}