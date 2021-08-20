using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;

public class EventGetter : MonoBehaviour
{
    public GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        Inputs.InputInterface.Instance.Events.OnDrag.AddListener(GetDragData);
    }

    public void GetDragData(Inputs.OnDragData data)
	{
        Debug.Log("Hello");

        Vector3 pos = sphere.transform.position;
        sphere.transform.position = new Vector3(
            pos.x, pos.y, pos.z + data.Delta.y);
	}
}
