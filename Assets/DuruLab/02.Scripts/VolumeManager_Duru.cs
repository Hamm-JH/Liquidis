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
    // Start is called before the first frame update
    void Start()
    {
        postFxProfile.TryGet<DepthOfField>(out dof);
      
    }

    // Update is called once per frame
    void Update()
    {
       if(isEnd)
        {
            StartCoroutine("DOF");
        }
    }

    IEnumerator DOF()
    {
        float time = 0;
        float duration = 10f;

        while(time<duration)
        {
            //focusDistance가 10에서 1이 되어야 한다.
            dof.focusDistance.value = 10-(10* time / duration);
            time += Time.deltaTime;
            Debug.Log("time is " + dof.focusDistance.value);
            yield return null;

        }
        isEnd = false;
    }
}
