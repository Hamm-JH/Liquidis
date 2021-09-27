using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadBox : MonoBehaviour
{
    public enum _boxType
    {
        LEFT,
        RIGHT
    }
    public _boxType boxType;
    public bool firstEnter = false;
    private float enterTime = 0f;
    private float limitTime = 3f;

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sphere"))
        {
            firstEnter = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (firstEnter)
        {
            if(enterTime < limitTime)
            {
                enterTime += Time.deltaTime;
                // ���� ������ �ӵ��� �����.####### called by frame ���� ########
                other.gameObject.GetComponent<StartSphere>().SetSphereSpeed(0.5f);
                Debug.Log("cur Time : " + enterTime);

            }
            else // ���ѽð� �̻� �ӹ������� ��� 
            {
                Debug.Log("3�� �̻�");
                
               // ���Ӿ� �ε��ϴ� �ڵ带 �ۼ��ϴ� �ڸ�

                if(boxType == _boxType.LEFT)
                {
                    // ���� ���� ��� �÷��̾� �ε�
                    SceneManager.LoadScene("02_Play_Guide");
                    
                }else if (boxType == _boxType.RIGHT)
                {
                    // ������ ���� ��� ����� �ε�
                }
                firstEnter = false;
                return;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (firstEnter)
        {
            // ���� ������ ������ �ð� ��� �ʱ�ȭ
            enterTime = 0f;

            // ���� ������ �ӵ��� ������� ȸ��
            other.gameObject.GetComponent<StartSphere>().SetSphereSpeed(1f);

            firstEnter = false;
        }
    } 
    // ���� 3�� ä��� �ۿ� �����ٰ� �ٽ� 0�ʺ��� �����ϰ� ������ ���� �츮�� �ȴ�
    //enterTime = 0f;
    // firstEnter = false;

}
