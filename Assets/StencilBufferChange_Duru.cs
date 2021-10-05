using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilBufferChange_Duru : MonoBehaviour
{
    public GameObject window;
    public int refnumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBuffer();
    }

    void ChangeBuffer()
    {
        Debug.Log("refnum " + refnumber);
        window.GetComponent<Renderer>().material.SetInt("_StencilRef", refnumber);
        Debug.Log("now the buffer is" + refnumber);
    }
}
