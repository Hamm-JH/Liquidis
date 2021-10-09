using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class MeetingRoom : MonoBehaviour
{
    public GameObject meetingHead;
    public VisualEffect vfxObject;

    public UnityAction<API.Brainwave> biGetter;

    [Header("Ani")]
    public GameObject counterHead_ani;
    public GameObject timer;
    public float timerStartTime = 2f;

    // 게임 시작하면 시간 재기, 5분, 10분이 되면 타이머 박스 애니 트리거
    // 타이머 박스 애니에 접근해서 부르기


    // 게임 강종 하면
    // TimerGameOver_Shake 하나로 끝낸다.(Trigger: GameOverShake)
    // TimerBox 애니에서 timer loop 끈다
    // 

    // 마지막 엔딩
    // speed interval current -> targetSpeed 로 lerp (N초동안 lerp)(10초)
    // geo 값 변경 -> targetValue로 바꿈(2나 3)
    // 임의의 duration 후에 (2초)
    // 동시에 MirrorRoom - Set trigger: MirrorRoomShake
    // 임의의 duration 후에 (2초)
    // Volume 에 VolumeManager에서 isEnd = true 로 targetDuration 설정 필요
    // 임의의 duration 후에 (2초)
    // CounterHead(Set tirgger: PlayerHeadOut)
    // 임의의 duration 후에 (1초)
    // CounterHead(Set trigger: CounterHeadBye)
    // 임의의 duration 후에 (2초)
    // 인트로씬 로드


    // VFX랑 같이 맵핑
    // CounterHead>MirrorGoup>Mirror1,2,3의 셰이더 프로퍼티에 접근. 
    // <Renderer>().SetFloat  "Reflection Intensity" : 0~1 .

    private void Start()
    {
        StartCoroutine(InitialMappingParameters());


        // ani sequence
        counterHead_ani.GetComponent<Animator>().SetTrigger("LightOn");
        StartCoroutine(StartTimerAni());
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialMeeintRoomParameters(meetingHead, vfxObject);
        

    }
    private void Update()
    {
     
    }

    IEnumerator StartTimerAni()
    {
        yield return new WaitForSeconds(timerStartTime);

        timer.SetActive(true);
    }

}
