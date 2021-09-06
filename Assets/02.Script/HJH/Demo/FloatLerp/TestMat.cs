using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMat : MonoBehaviour
{
    public SkinnedMeshRenderer render;
    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        mat = render.material;
        // 0.001 ~ 0.01 사이값
        //mat.SetFloat("_ExtrudeSize", 0.1f);

        // 0.1 ~ 3 사이값
        //mat.SetFloat("_ExtrudeAnimationSpeed", 20);
    }
}
