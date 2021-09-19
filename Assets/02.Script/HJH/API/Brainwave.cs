using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
	using Looxid.Link;

	public sealed class Brainwave : _API
	{
		/// <summary>
		/// ���� �������̽��� ������. �ʿ� �����͸� �����ڷ� ������
		/// </summary>
		/// <param name="targetId"> ��û ���������� </param>
		/// <param name="targetSecond"> ��û ���� �� ���� </param>
		public Brainwave(EEGSensorID targetId, float targetSecond)
		{
			id = targetId;
			second = targetSecond;
		}

		/// <summary>
		/// ���ĵ� ���� �������̽��� �Ҵ���. ������Ƽ�� �뵵�� �б� �������� ����� ����
		/// </summary>
		/// <param name="_delta"></param>
		/// <param name="_theta"></param>
		/// <param name="_alpha"></param>
		/// <param name="_beta"></param>
		/// <param name="_gamma"></param>
		public void Set(float _delta, float _theta, float _alpha, float _beta, float _gamma)
		{
			delta = _delta;
			theta = _theta;
			alpha = _alpha;
			beta = _beta;
			gamma = _gamma;
		}

		/// <summary>
		/// ���� ������. ������, �޼��忡 ���� �����Ͱ� �Ҵ��
		/// </summary>
		private EEGSensorID id;
		private float second;
		private float delta;
		private float theta;
		private float alpha;
		private float beta;
		private float gamma;

		/// <summary>
		/// �ܺ� ��� ������Ƽ. �����͸� �о�ö��� ������Ƽ�� ��� �����ϴ�.
		/// </summary>
		public EEGSensorID Id { get => id; }		// ��û : EEG ��������
		public float Second { get => second; }		// ��û : ���� �ð�����
		public float Delta { get => delta; }		// ��� : ��Ÿ�� ����ġ 0 ~ 1
		public float Theta { get => theta; }		// ��� : ��Ÿ�� ����ġ 0 ~ 1
		public float Alpha { get => alpha; }		// ��� : ������ ����ġ 0 ~ 1
		public float Beta { get => beta; }			// ��� : ��Ÿ�� ����ġ 0 ~ 1
		public float Gamma { get => gamma; }		// ��� : ������ ����ġ 0 ~ 1
	}
}
