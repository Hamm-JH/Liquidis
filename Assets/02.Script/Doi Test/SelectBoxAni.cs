using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBoxAni : MonoBehaviour
{
    //public Animator lightGroup_ani;
    //public Animator guide_ani;
    public int currentOpen = 0;
    bool[] aniOpen;
    //bool isAllCheck = false;

    //public GameObject mainCanvas;
    //bool isCanvasOn = false;

    public GameObject camera;

    public Vector3 targetFrontCameraPos;

    private void Start()
    {
        
        if(MappingParameter.instance.currentScene == MappingParameter.scene.SELECT)
        {
            aniOpen = new bool[6];
            HelpUIStart();
        }
    }
    public void HelpUIStart()
    {
        aniOpen[0] = true;
        currentOpen = 0;

        GetComponent<Animator>().SetTrigger("1FadeIn");

    }

    public void LeftButton()
    {
        //if (!isAllCheck)
        //{
        //    if (CheckAllTrue())
        //    {
        //        AllFadeOut();
        //        isAllCheck = true;
        //    }
        //}
        //else
        //{
        //    return;
        //}
            Debug.Log("button left:");



        if (currentOpen < 5)//���� â��ġ�� 2���� ������
            {

                currentOpen += 1;//�¹�ư ���� ������ ���ڰ� �þ��.
            Debug.Log("currentOpen:" + currentOpen);
            if (!aniOpen[currentOpen]) //���� ����â�� ���� �ƴϸ�
                aniOpen[currentOpen] = true; //���� ����â�� ������ �Ѵ�.

            switch (currentOpen)
                {
                    case 1:
                    Debug.Log("ani trigger : " + currentOpen); GetComponent<Animator>();
                    GetComponent<Animator>().SetTrigger("1FadeOut");
                    GetComponent<Animator>().SetTrigger("2FadeIn");

                    //if (!isCanvasOn)
                    //{
                    //    mainCanvas.SetActive(true);
                    //    isCanvasOn = true;
                    //}

                        break;
                    case 2:
                    Debug.Log("ani trigger : " + currentOpen);

                    GetComponent<Animator>().SetTrigger("2FadeOut");
                        GetComponent<Animator>().SetTrigger("3FadeIn");
                        break;
                case 3:
                    Debug.Log("ani trigger : " + currentOpen);

                    GetComponent<Animator>().SetTrigger("3FadeOut");
                    GetComponent<Animator>().SetTrigger("4FadeIn");
                    break;
                case 4:
                    Debug.Log("ani trigger : " + currentOpen);

                    GetComponent<Animator>().SetTrigger("4FadeOut");
                    GetComponent<Animator>().SetTrigger("5FadeIn");
                    break;
                case 5:
                    Debug.Log("ani trigger : " + currentOpen);

                    GetComponent<Animator>().SetTrigger("ButtonOff");
                    //MoveCameraFront();
                    break;
            }

            }
        
        
        
    }

    public void RightButton()
    {

        //if (!isAllCheck)
        //{
        //    if (CheckAllTrue())
        //    {
        //        AllFadeOut();
        //        isAllCheck = true;
        //    }
        //}
        //else
        //    return;
            Debug.Log("button right:");

        if (currentOpen > 0)
            {
                currentOpen -= 1;


            if (!aniOpen[currentOpen])
                aniOpen[currentOpen] = true;

            switch (currentOpen)
                {
                    case 0:
                    GetComponent<Animator>().SetTrigger("2FadeOut");
                    GetComponent<Animator>().SetTrigger("1FadeIn");
                    break;

                    case 1:
                    GetComponent<Animator>().SetTrigger("2FadeIn");
                    GetComponent<Animator>().SetTrigger("3FadeOut");
                    break;

                    case 2:
                    GetComponent<Animator>().SetTrigger("3FadeIn");
                    GetComponent<Animator>().SetTrigger("4FadeOut");
                    break;

                    case 3:
                    GetComponent<Animator>().SetTrigger("4FadeIn");
                    GetComponent<Animator>().SetTrigger("5FadeOut");
                    break;
            }

            }
        
        
    }


    //public void TriggerLightGroup()
    //{
    //    lightGroup_ani.SetTrigger("Start");
    //}

    //public void SelectAniEnd()
    //{
    //    WaitingRoom.instance.DisabledSelectBox();
    //}

    bool CheckAllTrue()
    {
        bool check = false;
        int count = 0;

        for(int i=0; i<aniOpen.Length; i++)
        {
            if (aniOpen[i])
            {
                count++;
            }
        }

        if (count == 5)
        {
            check = true;
        }
        else
        {
            check = false;

        }

        return check;
    }

    //void AllFadeOut()
    //{
    //    GetComponent<Animator>().SetTrigger("1FadeOut");
    //    GetComponent<Animator>().SetTrigger("2FadeOut");
    //    GetComponent<Animator>().SetTrigger("3FadeOut");

    //}

    public void MoveCameraFront()
    {
        camera.transform.position = targetFrontCameraPos;
    }


}
