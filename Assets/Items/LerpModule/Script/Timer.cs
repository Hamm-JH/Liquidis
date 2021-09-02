using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Debugging
{
	public class Timer : MonoBehaviour
	{
		public Stopwatch sw;
		public float value;

		// Start is called before the first frame update
		void Start()
		{
			sw = new Stopwatch();
			sw.Start();
		}

		// Update is called once per frame
		void Update()
		{
			value = sw.ElapsedMilliseconds;
		}
	}
}
