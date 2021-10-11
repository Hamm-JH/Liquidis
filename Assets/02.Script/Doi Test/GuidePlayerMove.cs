using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePlayerMove : MonoBehaviour
{

    public float speed = 3f;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0f, 0f, 1f) * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0f, 0f, -1f) * Time.deltaTime * speed);
        }
    }
}
