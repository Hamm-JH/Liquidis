using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggingHead : MonoBehaviour
{
    public Transform Head;

    public Transform mainCam;

    // Update is called once per frame
    void Update()
    {
        Head.rotation = mainCam.rotation;
    }
}
