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

    // ���� �����ϸ� �ð� ���, 5��, 10���� �Ǹ� Ÿ�̸� �ڽ� �ִ� Ʈ����
    // Ÿ�̸� �ڽ� �ִϿ� �����ؼ� �θ���


    // ���� ���� �ϸ�
    // TimerGameOver_Shake �ϳ��� ������.(Trigger: GameOverShake)
    // TimerBox �ִϿ��� timer loop ����
    // 

    // ������ ����
    // speed interval current -> targetSpeed �� lerp (N�ʵ��� lerp)(10��)
    // geo �� ���� -> targetValue�� �ٲ�(2�� 3)
    // ������ duration �Ŀ� (2��)
    // ���ÿ� MirrorRoom - Set trigger: MirrorRoomShake
    // ������ duration �Ŀ� (2��)
    // Volume �� VolumeManager���� isEnd = true �� targetDuration ���� �ʿ�
    // ������ duration �Ŀ� (2��)
    // CounterHead(Set tirgger: PlayerHeadOut)
    // ������ duration �Ŀ� (1��)
    // CounterHead(Set trigger: CounterHeadBye)
    // ������ duration �Ŀ� (2��)
    // ��Ʈ�ξ� �ε�


    // VFX�� ���� ����
    // CounterHead>MirrorGoup>Mirror1,2,3�� ���̴� ������Ƽ�� ����. 
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
