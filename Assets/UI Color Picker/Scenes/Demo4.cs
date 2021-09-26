using UnityEngine;
using System.Collections;

public class Demo4 : MonoBehaviour {

	public ColorPicker cPicker;
	
	// Update is called once per frame
	void Update () {
		GetComponent<Renderer>().material.color = cPicker.pickedColor;
	}
}
