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
                // 공의 움직임 속도를 늦춘다.####### called by frame 주의 ########
                other.gameObject.GetComponent<StartSphere>().SetSphereSpeed(0.5f);
                Debug.Log("cur Time : " + enterTime);

            }
            else // 제한시간 이상 머물러있을 경우 
            {
                Debug.Log("3초 이상");
                
               // 게임씬 로드하는 코드를 작성하는 자리

                if(boxType == _boxType.LEFT)
                {
                    // 왼쪽 블럭일 경우 플레이씬 로드
                    SceneManager.LoadScene("02_Play_Guide");
                    
                }else if (boxType == _boxType.RIGHT)
                {
                    // 오른쪽 블럭일 경우 무비씬 로드
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
            // 공이 밖으로 나가면 시간 기록 초기화
            enterTime = 0f;

            // 공의 움직임 속도를 원래대로 회복
            other.gameObject.GetComponent<StartSphere>().SetSphereSpeed(1f);

            firstEnter = false;
        }
    } 
    // 공이 3초 채우고 밖에 나갔다가 다시 0초부터 실행하고 싶으면 여기 살리면 된다
    //enterTime = 0f;
    // firstEnter = false;

}
