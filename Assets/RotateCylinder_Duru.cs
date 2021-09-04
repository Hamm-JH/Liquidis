using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCylinder_Duru : MonoBehaviour
{
    public GameObject cy1;
    public GameObject cy2;
    public GameObject cy3;
    public float speed;

    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        Rotate();   
    }

    void Rotate()
    {


        cy1.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        cy2.transform.Rotate(Vector3.forward * Time.deltaTime * -speed);
        cy3.transform.Rotate(Vector3.forward * Time.deltaTime * speed);


    }
}
