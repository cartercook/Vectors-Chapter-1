using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/**
 * attach this to NumberLine/Canvas/Holder
 */
public class NumArray : MonoBehaviour {
	private float canvasWidth;
	private RawImage line;
	private RectTransform numbers;
	private Object prefab;
	
	void Start () {
		canvasWidth = transform.parent.GetComponent<RectTransform>().rect.width;
		line = transform.FindChild("Line").GetComponent<RawImage>();
		numbers = transform.FindChild("Numbers").transform as RectTransform;
		prefab = Resources.Load("Number");
		addNumber();
	}

	public void stretchTo(float length) { //called by NumLineArrow so it can stretch to fit properly
		line.uvRect = new Rect(0, 0, length, 1); //graphic is set to auto-tile in the inspector
		//stretch numbers and add if necessary
		numbers.localScale = new Vector2((canvasWidth*numbers.childCount)/(numbers.rect.width*length), transform.localScale.y);
	}

	public void setLength(float length) {

	}

	public void addNumber() {
		GameObject newText = Instantiate(prefab) as GameObject; //create a new text object
		newText.transform.SetParent(numbers); //set its parent to numbers
		//numbers.sizeDelta += new Vector2((newText.transform as RectTransform).rect.width, 0);
		newText.transform.SetAsLastSibling();
		newText.GetComponent<Text>().text = newText.name = (newText.transform.GetSiblingIndex() + 1).ToString();
		newText.transform.localScale = newText.transform.lossyScale; //fix the fucky parent scale
	}

}