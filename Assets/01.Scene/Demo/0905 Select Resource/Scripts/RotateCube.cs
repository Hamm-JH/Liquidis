using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private Transform target;

    public bool isRotateNow;
    public float movedVector;
    public int indexLeftOrRight;

    public Button left_button;
    public Button right_button;

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
            if(left_button != null && right_button != null)
            {
                left_button.interactable = false;
                right_button.interactable = false;
            }
           


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
            if (left_button != null && right_button != null)
            {
                left_button.interactable = true;
                right_button.interactable = true;
            }
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

    //public void LeftButton()
    //{
    //    if(MappingParameter.instance.currentMatchEmotion < 2)
    //    {
    //        OnRotate(0);
    //    }
    //}

    //public void RightButton()
    //{
    //    if (MappingParameter.instance.currentMatchEmotion > 1)
    //    {
    //        OnRotate(1);

    //    }
    //}
}
