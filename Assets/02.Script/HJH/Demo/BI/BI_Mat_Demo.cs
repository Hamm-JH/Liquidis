using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class BI_Mat_Demo : MonoBehaviour
{
    public BIManager biManager;

    public GameObject targetObj;
    public SkinnedMeshRenderer smr;
    public Material targetMat;

    public TextMesh tm;
    private int lastSec;

    // Start is called before the first frame update
    void Start()
    {
        smr = targetObj.GetComponent<SkinnedMeshRenderer>();
        targetMat = smr.materials[0];

        lastSec = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(smr.materials[0].GetFloat("_ExtrudeSize"));
        //targetMat.SetFloat("_ExtrudeSize", 0.001f);
        //targetMat.SetFloat("_ExtrudeAnimationSpeed", 5);

        int currSec = (int)Time.time;

        if(currSec != lastSec)
		{
            //Debug.Log($"sec changed : {currSec}");

            Debug.Log(biManager.Delta.value);
            float dValue = (float)biManager.deltaV;

            float prev = targetMat.GetFloat("_ExtrudeSize");
            float toV = 0.001f + dValue / 100 * 10;
            targetMat.SetFloat("_ExtrudeSize", toV);

            tm.text = toV.ToString();
		}

        lastSec = currSec;
    }


}
