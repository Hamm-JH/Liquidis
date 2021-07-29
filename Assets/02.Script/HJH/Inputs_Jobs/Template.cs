using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
	public abstract class Template<T> : MonoBehaviour where T : Template<T>
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if(instance == null)
				{
					instance = FindObjectOfType<T>() as T;
					if(instance == null)
					{
						GameObject obj = new GameObject(typeof(T).Name);
						instance = obj.AddComponent<T>();
					}
				}
				return instance;
			}
		}
	}
}
