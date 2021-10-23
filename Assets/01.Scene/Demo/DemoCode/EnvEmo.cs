using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvEmo : MonoBehaviour
{
    [System.Serializable]
    public struct Initializer
    {
        public string value1;
        public int value2;

        public void Reset()
        {
            value1 = "aa";
            value2 = 1;
        }
    }

    public Initializer init;

    // Start is called before the first frame update
    void Start()
    {
        init.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
