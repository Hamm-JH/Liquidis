using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBoxAni : MonoBehaviour
{
    public Animator lightGroup_ani;
    public int currentOpen = 0;
    bool[] aniOpen;
    bool isAllCheck = false;

    private void Start()
    {
        

        if(MappingParameter.instance.currentScene == MappingParameter.scene.SELECT)
        {
            aniOpen = new bool[3];
            HelpUIStart();
        }
    }
    public void HelpUIStart()
    {
        aniOpen[0] = true;
        currentOpen = 0;

        //GetComponent<Animator>().SetTrigger("GuideStart");
        GetComponent<Animator>().SetTrigger("1FadeIn");

    }

    public void LeftButton()
    {
        if (!isAllCheck)
        {
            if (CheckAllTrue())
            {
                AllFadeOut();
                isAllCheck = true;
            }
        }
        else
        {
            return;
        }
        
            if (currentOpen < 2)
            {

                currentOpen += 1;
                if (!aniOpen[currentOpen])
                    aniOpen[currentOpen] = true;

                switch (currentOpen)
                {
                    case 1:
                        Debug.Log("ani trigger : " + currentOpen);
                        GetComponent<Animator>().SetTrigger("2FadeOut");
                        GetComponent<Animator>().SetTrigger("1FadeIn");
                        break;
                    case 2:
                        Debug.Log("ani trigger : " + currentOpen);

                        GetComponent<Animator>().SetTrigger("3FadeOut");
                        GetComponent<Animator>().SetTrigger("2FadeIn");
                        break;
                }

            }
        
        
        
    }

    public void RightButton()
    {

        if (!isAllCheck)
        {
            if (CheckAllTrue())
            {
                AllFadeOut();
                isAllCheck = true;
            }
        }
        else
            return;
        
            if (currentOpen > 0)
            {
                currentOpen -= 1;
                if (!aniOpen[currentOpen])
                    aniOpen[currentOpen] = true;

                switch (currentOpen)
                {
                    case 0:
                        Debug.Log("ani trigger : " + currentOpen);

                        GetComponent<Animator>().SetTrigger("1FadeOut");
                        GetComponent<Animator>().SetTrigger("2FadeIn");
                        break;
                    case 1:
                        Debug.Log("ani trigger : " + currentOpen);

                        GetComponent<Animator>().SetTrigger("2FadeOut");
                        GetComponent<Animator>().SetTrigger("3FadeIn");
                        break;
                }

            }
        
        
    }


    public void TriggerLightGroup()
    {
        lightGroup_ani.SetTrigger("Start");
    }

    public void SelectAniEnd()
    {
        WaitingRoom.instance.DisabledSelectBox();
    }

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

        if (count == 3)
        {
            check = true;
        }
        else
        {
            check = false;

        }

        return check;
    }

    void AllFadeOut()
    {
        Debug.Log("all read");
        GetComponent<Animator>().SetTrigger("1FadeOut");
        GetComponent<Animator>().SetTrigger("2FadeOut");
        GetComponent<Animator>().SetTrigger("3FadeOut");

    }
}
