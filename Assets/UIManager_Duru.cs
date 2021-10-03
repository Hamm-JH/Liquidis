using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Duru : MonoBehaviour
{
   // public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider col)
    {
       // Debug.Log("colname is " + col);
        if (col.tag == "Guide")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
        if (col.tag == "Guide2")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
        if (col.tag == "Guide3")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
        if (col.tag == "Guide4")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
        if (col.tag == "Guide5")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
        if (col.tag == "Guide6")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
        if (col.tag == "Guide7")
        {
            //Debug.Log("Enter~");
            col.gameObject.GetComponent<Animator>().SetTrigger("Start");
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Guide")
        {
            if(col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }
            
        }
        if (col.tag == "Guide2")
        {
            if (col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }

        }
        if (col.tag == "GuidE3")
        {
            if (col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }

        }
        if (col.tag == "Guide4")
        {
            if (col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }

        }
        if (col.tag == "Guide5")
        {
            if (col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }

        }
        if (col.tag == "Guide6")
        {
            if (col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }

        }
        if (col.tag == "Guide7")
        {
            if (col.gameObject.GetComponent<Animator>().isActiveAndEnabled)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("End");
            }

        }
    }
   
    
}
