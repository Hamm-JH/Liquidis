using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private Transform target;

    public bool isRotateNow;
    public float movedVector;
    public int indexLeftOrRight;

    // Start is called before the first frame update
    void Start()
    {
        isRotateNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(target.rotation.eulerAngles.y);
        // 회전중일때
        if(isRotateNow)
		{
            movedVector += 90 * Time.deltaTime * indexLeftOrRight;
            target.Rotate(new Vector3(0, 90 * Time.deltaTime * indexLeftOrRight, 0));

            Vector3 angle = target.rotation.eulerAngles;

            if(indexLeftOrRight > 0)
			{
                if(movedVector > 90 * indexLeftOrRight)
			    {
                    isRotateNow = false;

                    target.eulerAngles = new Vector3(
                        angle.x,
                        (int)angle.y,
                        angle.z
                        );
			    }
			}
            else if(indexLeftOrRight < 0)
			{
                if (movedVector < 90 * indexLeftOrRight)
                {
                    isRotateNow = false;

                    target.eulerAngles = new Vector3(
                        angle.x,
                        (int)angle.y,
                        angle.z
                        );
                }
            }
		}
        else
		{
            movedVector = 0;
		}
    }

    public void OnRotate(int index)
	{
        if(!isRotateNow)
		{
            // On Left
            if(index == 0)
		    {
                isRotateNow = true;
                indexLeftOrRight = 1;
		    }
            // On Right
            else if(index == 1)
		    {
                isRotateNow = true;
                indexLeftOrRight = -1;
            }
		}
	}
}
