using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMesh : MonoBehaviour
{
    [SerializeField] MeshFilter filter;
    [SerializeField] Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = filter.mesh;

        Vector2[] uvs = mesh.uv;
        List<Vector2> newUV = new List<Vector2>();

        int index = uvs.Length;
		for (int i = 0; i < index; i++)
		{
            //Debug.Log(uvs[i]);
            newUV.Add(
                new Vector2(
                    uvs[i].x > 0.5f ? 0 : 1,
                    uvs[i].y)
                );
		}

        mesh.uv = newUV.ToArray();

        filter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
