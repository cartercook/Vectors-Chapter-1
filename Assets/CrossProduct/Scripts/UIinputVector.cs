using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIinputVector : UIvector {
	private Master master;
	private InputField x;
	private InputField y;
	private InputField z;

	// Use this for initialization
	void Start () {
		master = Master.instance;
			//nameInputField.onSubmit.AddListener((value) => SubmitName(value));
		InputField[] components = GetComponentsInChildren<InputField>();
		x = components[0];
		y = components[1];
		z = components[2];
		x.onValueChange.AddListener((input) => setX(input));
		y.onValueChange.AddListener((input) => setY(input));
		z.onValueChange.AddListener((input) => setZ(input));
		/*
		Text[] components = new Text[3];
		for (int i=0; i < transform.childCount; i++) {
			components[i] = transform.GetChild(i).FindChild("Text").GetComponent<Text>();
		}
		x = components[0];
		y = components[1];
		z = components[2];
		Debug.Log (x.name + ", " + y + ", " + z);
		*/
	}

	private void setX(string input) {
		if (!(input.Equals("") || input.Equals("-"))) {
			master.updateMoveableX(float.Parse(input));
		}
	}
	private void setY(string input) {
		if (!(input.Equals("") || input.Equals("-"))) {
			master.updateMoveableY(float.Parse(input));
		}
	}
	private void setZ(string input) {
		if (!(input.Equals("") || input.Equals("-"))) {
			master.updateMoveableZ(float.Parse(input));
		}
	}

	public void addListeners() {
		x.onValueChange.AddListener((input) => removeListeners());
		y.onValueChange.AddListener((input) => removeListeners());
		z.onValueChange.AddListener((input) => removeListeners());
	}

	private void removeListeners() {
		/*
		x.onValueChange.RemoveListener(removeListeners());
		y.onValueChange.RemoveListener(removeListeners());
		z.onValueChange.RemoveListener(removeListeners());
		*/
		master.hideHalo();
	}

	public override void set(Vector3 vector) {
		x.text = vector.x.ToString("0"); //0 means format with no decimals
		y.text = vector.y.ToString("0");
		z.text = vector.z.ToString("0");
	}

}
