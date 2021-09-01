﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AkilliMum.SRP.LCRS
{
    public class SimpleCameraMove : MonoBehaviour
    {

        public float lookSpeed = 15.0f;
        public float moveSpeed = 15.0f;

        float rotationX = 0.0f;
        float rotationY = 0.0f;

        void Update()
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.position += transform.forward * moveSpeed * Input.GetAxis("Vertical");
            transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal");
        }
    }
}