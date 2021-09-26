using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Demo1 : MonoBehaviour {

	public ColorPicker cPicker;
	
	// Update is called once per frame
	void Update () {
		GetComponent<Image> ().color = cPicker.pickedColor;
		GetComponentInChildren<Text> ().text = "R " + cPicker.pickedColor.r * 255 + "\nG " + cPicker.pickedColor.g * 255 + "\nB " + cPicker.pickedColor.b * 255;
	}
}
