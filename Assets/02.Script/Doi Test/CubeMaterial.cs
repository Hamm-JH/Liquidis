using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMaterial : MonoBehaviour
{

    public Button left_rotate_button;
    public Button right_rotate_button;

    public GameObject cubeObj;

    bool rotateLeft = false;
    bool rotateRight = false;
    float rotateTime = 1f;
    float curTime = 0f;

    float targetRotation = 0f;
    float curRotation = 0f;

    public int currentEmotion = 1;
    // 1부터 시작, 1,2,3,4

    public MappingParameter mappingParameter;

    // Start is called before the first frame update
    void Start()
    {
        left_rotate_button.onClick.AddListener(RotateCubeLeft);
        right_rotate_button.onClick.AddListener(RotateCubeRight);
    }

    void RotateCubeLeft()
    {
        
       
        if(currentEmotion < 4)
        {
            currentEmotion += 1;
            mappingParameter.SetCurrentEmotion(currentEmotion);

            targetRotation = cubeObj.transform.eulerAngles.y + 90f;
            rotateLeft = true;

        }

    }

    void RotateCubeRight()
    {
       
       
        
        if (currentEmotion > 1)
        {
            currentEmotion -= 1;
            mappingParameter.SetCurrentEmotion(currentEmotion);

            if (targetRotation <= 0f)
            {
                targetRotation += 360f;
                //360 - 현재값.
            }

            targetRotation = cubeObj.transform.eulerAngles.y - 90f;
            Debug.Log(targetRotation);
            rotateRight = true;

        }

    }



    private void Update()
    {
        if (rotateLeft)
        {
            
            if (cubeObj.transform.rotation.eulerAngles.y < targetRotation)
            {
                cubeObj.transform.Rotate(new Vector3(0f, 1f, 0f));
                
            }
            else
            {
               
                rotateLeft = false;
            }
           

        }
        Debug.Log("y is "+transform.rotation.eulerAngles.y);
        if (rotateRight)
        {
            cubeObj.transform.Rotate(new Vector3(0f, -1f, 0f));

            if (cubeObj.transform.rotation.eulerAngles.y < targetRotation)
            {
                rotateRight = false;
               
                

            }
            else
            {

                
            }


        }
    }
}
