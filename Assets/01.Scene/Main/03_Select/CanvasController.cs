using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetCamera.transform);
    }
}
