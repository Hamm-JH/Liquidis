using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TexOut : MonoBehaviour
{
	[System.Serializable]
	public struct EEG
	{
		public List<string> baseTexts;
	}

    public Text text;

	[SerializeField] string baseText;

	public EEG eegTxt;

	public void Set(string _in)
	{
		text.text = string.Format("{0} : {1}", baseText, _in);
	}

	public void Set(string[] _in)
	{
		StringBuilder sb = new StringBuilder();

		if (eegTxt.baseTexts.Count > _in.Length)
		{
			Debug.LogWarning("input text count not matched");
			return;
		}

		int index = eegTxt.baseTexts.Count;
		for (int i = 0; i < index; i++)
		{
			Debug.Log("Hello");
			sb.AppendLine(string.Format("{0}\t: {1}", eegTxt.baseTexts[i], _in[i]));
		}

		text.text = sb.ToString();

		sb.Clear();
	}
}
