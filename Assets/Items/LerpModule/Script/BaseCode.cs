using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BaseCode : MonoBehaviour
{
    public int interval;
    public Transform obj1;
    Stopwatch sw;

    // Start is called before the first frame update
    void Start()
    {
        sw = new Stopwatch();
        sw.Start();
    }

    // Update is called once per frame
    void Update()
    {
        float value = UpdateTimeToValue(interval);

        UpdateLine(obj1, value);
        
    }

    private float UpdateTimeToValue(int interval)
	{
        if (sw.ElapsedMilliseconds >= interval) sw.Restart();

        float _value = (int)sw.ElapsedMilliseconds / (float)interval;
        //Debug.Log(_value);

        return _value;
	}

    private void UpdateLine(Transform obj, float value)
	{
        //Debug.Log("Hello");
        //Debug.Log(value);
        obj1.position = new Vector3(
            obj1.position.x,
            value,
            obj1.position.z
            );
	}
}
