using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSphere : MonoBehaviour
{
    float speed = 1f;
    public BallAniTest_Duru ball_ani;

    public void SetSphereSpeed(float _speed)
    {
        speed = _speed;

    }

    private void OnMouseDown()
    {
        ball_ani.TitleAni();
    }
    private void OnMouseDrag()
    {
        MoveSphere();
    }

    public void MoveSphere()
    {
        float xPos = Input.mousePosition.x / Screen.width;

        //Debug.Log(xPos);
        xPos = xPos * 17f;
        xPos -= 8.5f;

        xPos = Mathf.Clamp(xPos, -8.5f, 8.5f);

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}
