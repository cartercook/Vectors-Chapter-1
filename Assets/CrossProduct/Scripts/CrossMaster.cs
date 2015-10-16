using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * pretty crappy as is, just allows me to reference other objects programmatically
 * when they need to interact.
 */
public class CrossMaster : Master {

	private Arrow staticArrow;
	private Arrow crossProduct;
	private GridOverlay gridOverlay;
	private Toggle gridToggle;
	private UIvector vector1; //mapped to staticArrow
	private UIvector vector3; //mapped to crossProduct

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		base.initialise(); //instantiates moveArrow, vector2, and halo
		staticArrow = GameObject.Find("Cluster/StaticArrow").GetComponent<Arrow>();
		crossProduct = GameObject.Find("Cluster/CrossProduct").GetComponent<Arrow>();
		gridToggle = GameObject.Find("Canvas/GridToggle").GetComponent<Toggle>();
		vector1 = GameObject.Find("Canvas/Header/Vector1").GetComponent<UIvector>(); //mapped to staticArrow
		vector1.set(staticArrow.toVector()); //staticArrow does not move
		vector3 = GameObject.Find("Canvas/Header/Vector3").GetComponent<UIvector>(); //mapped to crossProduct
	}

	//MoveableArrow calls this in its onDrag function
	public override void updateCrossProduct() {
		Vector3 moveVec = moveArrow.toVector();
		Vector3 cross = Vector3.Cross(moveVec, staticArrow.toVector());
		crossProduct.stretchTo(cross); //update crossProduct
		vector2.set(moveVec); //update moveArrow UI
		vector3.set(cross); //update crossProduct UI
	}
	
	public void setStaticArrow (Vector3 vector) {
		staticArrow.stretchTo(vector);
		updateCrossProduct();
	}

	//getters and setters
	public bool moveable {
		set {moveArrow.controlable = value;}
	}
	//MoveableArrow uses this to determine whether or not to snap to grid
	public bool snapToGrid {
		get {return gridToggle.isOn;}
	}

}
