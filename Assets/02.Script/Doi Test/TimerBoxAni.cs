using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBoxAni : MonoBehaviour
{
    //. select box ani ����

    //public int currentOpen = 0;
    //bool[] aniOpen;
    //bool isAllCheck = false;

    //bool isCanvasOn = false;

    //public GameObject left_button;
    //public GameObject right_button;
    public Animator timerCanvas_ani;

    bool isFirstClicked = false;
    // �� ������ ���� TimerRotateLoop(Set trigger: TimerLoop) : ��ȭ���� �������.
    private void Start()
    {
        
    }

    public void RightButtonClicked()
    {
        if (!isFirstClicked)
        {
            timerCanvas_ani.SetTrigger("2FadeIn");
            isFirstClicked = true;
        }
        else
        {
            timerCanvas_ani.SetTrigger("ButtonOff");

        }
    }
    #region rotate cube ani
    //private void Start()
    //{
    //    aniOpen = new bool[3];
    //    HelpUIStart();
    //}
    //public void HelpUIStart()
    //{
    //    aniOpen[0] = true;
    //    currentOpen = 0;

    //    //GetComponent<Animator>().SetTrigger("GuideStart");
    //    GetComponent<Animator>().SetTrigger("1FadeIn");
    //    Debug.Log("fadeStart?");

    //}

    //public void LeftButton()
    //{
    //    if (!isAllCheck)
    //    {
    //        if (CheckAllTrue())
    //        {
    //            AllFadeOut();
    //            isAllCheck = true;
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }

    //    if (currentOpen < 2)//���� â��ġ�� 2���� ������
    //    {

    //        currentOpen += 1;//�¹�ư ���� ������ ���ڰ� �þ��.
    //        Debug.Log("currentOpen:" + currentOpen);
    //        if (!aniOpen[currentOpen]) //���� ����â�� ���� �ƴϸ�
    //            aniOpen[currentOpen] = true; //���� ����â�� ������ �Ѵ�.

    //        switch (currentOpen)
    //        {
    //            case 1:
    //                Debug.Log("ani trigger : " + currentOpen);
    //                GetComponent<Animator>().SetTrigger("1FadeOut");
    //                GetComponent<Animator>().SetTrigger("2FadeIn");





    //                break;
    //            case 2:
    //                Debug.Log("ani trigger : " + currentOpen);

    //                GetComponent<Animator>().SetTrigger("2FadeOut");
    //                GetComponent<Animator>().SetTrigger("3FadeIn");
    //                break;
    //        }

    //    }



    //}
    //public void RightButton()
    //{

    //    if (!isAllCheck)
    //    {
    //        if (CheckAllTrue())
    //        {
    //            AllFadeOut();
    //            isAllCheck = true;
    //        }
    //    }
    //    else
    //        return;

    //    if (currentOpen > 0)
    //    {
    //        currentOpen -= 1;
    //        Debug.Log("currentOpen:" + currentOpen);
    //        if (!aniOpen[currentOpen])
    //            aniOpen[currentOpen] = true;

    //        switch (currentOpen)
    //        {
    //            case 0:
    //                Debug.Log("Right ani trigger : " + currentOpen);
    //                GetComponent<Animator>().SetTrigger("2FadeOut");
    //                GetComponent<Animator>().SetTrigger("1FadeIn");

    //                break;

    //            case 1:
    //                Debug.Log("Rignt ani trigger : " + currentOpen);


    //                GetComponent<Animator>().SetTrigger("2FadeIn");
    //                GetComponent<Animator>().SetTrigger("3FadeOut");
    //                break;
    //        }

    //    }


    //}
    //bool CheckAllTrue()
    //{
    //    bool check = false;
    //    int count = 0;

    //    for (int i = 0; i < aniOpen.Length; i++)
    //    {
    //        if (aniOpen[i])
    //        {
    //            count++;
    //        }
    //    }

    //    if (count == 3)
    //    {
    //        check = true;
    //    }
    //    else
    //    {
    //        check = false;

    //    }

    //    return check;
    //}

    //void AllFadeOut()
    //{
    //    Debug.Log("all read");
    //    GetComponent<Animator>().SetTrigger("1FadeOut");
    //    GetComponent<Animator>().SetTrigger("2FadeOut");
    //    GetComponent<Animator>().SetTrigger("3FadeOut");

    //    left_button.SetActive(false);
    //    right_button.SetActive(false);

    //}
    #endregion
}
