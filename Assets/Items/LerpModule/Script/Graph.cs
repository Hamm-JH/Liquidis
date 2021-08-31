using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Log(Slider slider)
	{
        float value = Mathf.Log((slider.value*31 + 1), 2) / 5;

        obj.transform.localPosition = new Vector3(
            obj.transform.localPosition.x,
            value,
            obj.transform.localPosition.z
            );

        //Debug.Log(slider.value);
        Debug.Log(Mathf.Log((slider.value*31 + 1), 2) / 5);
        //Debug.Log(Mathf.Log10(slider.value));
        //Debug.Log(Mathf.Log(slider.value + 1));
	}

    public void Exponential(Slider slider)
	{
		//Debug.Log(slider.value);

		float value = Mathf.Exp(slider.value * 2);

        obj.transform.localPosition = new Vector3(
            obj.transform.localPosition.x,
            value,
            obj.transform.localPosition.z
            );

        Debug.Log(value);
	}

    public void Power(Slider slider)
	{
        float value = Mathf.Pow(slider.value * 16, 2) / Mathf.Pow(16, 2);

        obj.transform.localPosition = new Vector3(
            obj.transform.localPosition.x,
            value,
            obj.transform.localPosition.z
            );

        Debug.Log(value);
	}
}
