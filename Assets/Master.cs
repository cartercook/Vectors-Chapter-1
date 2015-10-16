using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Master : MonoBehaviour {

	public static Master instance = null;
	public int radius;
	protected MoveableArrow moveArrow;
	protected UIinputVector vector2; //mapped to moveArrow
	protected GameObject halo;

	protected void initialise() {
		moveArrow = GameObject.Find("Cluster/MoveArrow").GetComponent<MoveableArrow>();
		vector2 = GameObject.Find("Canvas/Header/Vector2").GetComponent<UIinputVector>(); //mapped to moveArrow
		halo = GameObject.Find("Canvas/Halo");
	}

	public abstract void updateCrossProduct(); //this is just here so it can be referenced by moveArrow

	//A trio of funcs called by UIinputVector to manipulate moveableArrow
	public void updateMoveableX(float newX) {
		Vector3 newVector = moveArrow.toVector();
		newVector.x = newX;
		setMoveArrow(newVector); //also updates cross product
	}
	public void updateMoveableY(float newY) {
		Vector3 newVector = moveArrow.toVector();
		newVector.y = newY;
		setMoveArrow(newVector); //also updates cross product
	}
	public void updateMoveableZ(float newZ) {
		Vector3 newVector = moveArrow.toVector();
		newVector.z = newZ;
		setMoveArrow(newVector); //also updates cross product
	}

	public void setMoveArrow (Vector3 vector) {
		moveArrow.stretchTo(vector);
		updateCrossProduct();
	}

	//public methods for controlling the UI halo
	public void showHalo() { //center halo behind vector2 make it visible
		RectTransform haloTrans = (RectTransform)halo.transform;
		RectTransform vectorTrans = (RectTransform)vector2.transform;
		haloTrans.position = vectorTrans.position;
		halo.GetComponent<RawImage>().enabled = true;
		vector2.addListeners(); //to disable the halo once vector2 is changed
	}
	public void hideHalo() {
		halo.GetComponent<RawImage>().enabled = false;
	}

}