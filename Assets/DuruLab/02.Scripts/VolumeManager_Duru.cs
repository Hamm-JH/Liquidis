using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class VolumeManager_Duru : MonoBehaviour
{

    public VolumeProfile postFxProfile;
    DepthOfField dof;
    public bool isEnd = false;
    public float targetDuration = 10f;
    float currentDOF;

    // Start is called before the first frame update
    void Start()
    {
        postFxProfile.TryGet<DepthOfField>(out dof);
        currentDOF = dof.focusDistance.value;
      
    }

    // Update is called once per frame
    void Update()
    {
       if(isEnd)
        {
            StartCoroutine("DOF");
            isEnd = false;
            return;
        }
    }

    IEnumerator DOF()
    {
        float time = 0;
        //float duration = 10f;

        while(time<targetDuration)
        {
            //focusDistance�� 10���� 1�� �Ǿ�� �Ѵ�.
            dof.focusDistance.value = currentDOF - (currentDOF * time / targetDuration);//20-20
            time += Time.deltaTime;
            Debug.Log("time is " + dof.focusDistance.value);
            yield return null;

        }
        //isEnd = false;
    }
}
