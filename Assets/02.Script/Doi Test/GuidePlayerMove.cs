using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuidePlayerMove : MonoBehaviour
{

    public float speed = 3f;
    public Animator camera_rig_ani;

    bool done = false;

    // Update is called once per frame
    void Update()
    {

        if (!done)
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
       

        if(transform.position.z > 62f)
        {
            if (!done)
            {
                MoveAniStart();
                done = true;
                return;

            }
        }
    }

    void MoveAniStart()
    {
        camera_rig_ani.SetTrigger("SceneEnd");
        StartCoroutine(LoadSelectScene());
    }

    IEnumerator LoadSelectScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("03_Play_Select");
    }
}
