using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallAniTest_Duru : MonoBehaviour
{

    float x;
    public GameObject play;
    public GameObject movie;
    public GameObject liquidis;

    public bool isTriggered = false;
 
    // Update is called once per frame
    void Update()
    {
        x = gameObject.transform.position.x;
        XAxisMove();
      
        if(isTriggered)
        {
            TitleAni();
            isTriggered = false;
        }
    }
  
    // Ʈ���� Ŭ�� ��(������ Ȯ�� ��!)_
    public void TitleAni()
    {

        liquidis.GetComponent<Animator>().SetTrigger("Liquidis");
        
    }


    // x������ �����̴� ���� ���� �÷��̿� ���� �̹��� ���İ� ��ȭ �ִ�.
    public void XAxisMove()
    {
        byte absX = (byte)(Mathf.Abs(x/10f)*255f);
        Color color = new Color32(255, 255,255,absX);
      //  Debug.Log("color is now" + color);


        if (x>0)
        {
            play.GetComponent<Image>().CrossFadeColor(color, 0, true, true);
            
          

        }
        else if(x<0)
        {
            movie.GetComponent<Image>().CrossFadeColor(color, 0, true, true);
        }
        else
        {
            play.GetComponent<Image>().CrossFadeColor(color, 0, true, true);
            movie.GetComponent<Image>().CrossFadeColor(color, 0, true, true);
        }
       
    }

}
