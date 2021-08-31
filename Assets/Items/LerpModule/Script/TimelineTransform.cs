using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TimelineTransform : MonoBehaviour
{
    public Timer timer;

    public int index;
    public float interval;

    //Stopwatch sw;

    public Transform sineObj;

    public int tInterval;

    // Start is called before the first frame update
    void Start()
    {
        //sw = new Stopwatch();
        //sw.Start();
        //Mathf.Sin(0 ~ 360)
        if(index == 3)
		{
            
		}
    }

    // Update is called once per frame
    void Update()
    {
        float _value = 0;
        // sine
        if(index == 0)
		{
            _value = SineValue(GetValue(interval));
		}
        // cos
        else if(index == 1)
		{
            //Debug.Log(GetValue(interval));
            _value = CosValue(GetValue(interval));
			//Debug.Log(_value);
		}
        else if(index == 2)
		{
            _value = TanValue(GetValue(interval));
		}
        else if(index == 3)
		{
            _value = GetValue(interval);
            _value = _value % (interval * 10);
			//Debug.Log(_value);
			//Debug.Log(interval);
			sineObj.localPosition = Vector3.Lerp(
                sineObj.localPosition,
                new Vector3(0, _value * 10, 0),
                Time.deltaTime);
        }
        else if(index == 4)
		{
            int currV = (int)timer.sw.ElapsedMilliseconds / 1000;
            if(currV != tInterval)
			{
                //Debug.Log(tInterval);   
                // 타이머 다름
                sineObj.localPosition = Vector3.Slerp(
                    sineObj.localPosition,
                    new Vector3(0, (float)currV * 1000, 0),
                    Time.deltaTime
                    );
			}
            tInterval = currV;

		}


        //Debug.Log(_value);

        if(index >= 0 && index < 3)
		{
            Vector3 pos = sineObj.position;
            sineObj.position = new Vector3(
                pos.x,
                _value,
                pos.z
                );
		}
    }

    private float SineValue(float _value)
	{
        float value = Mathf.Sin(_value * 360);

        return value;
	}

    private float CosValue(float _value)
	{
        float value = Mathf.Cos(_value * 360);

        return value;
	}

    private float TanValue(float _value)
	{
        float value = Mathf.Tan(_value * 180);

        return value;
	}



    private float GetValue(float interval)
	{
        float _value = (float)timer.sw.ElapsedMilliseconds / (1000 * interval);

        return _value;
	}
}
