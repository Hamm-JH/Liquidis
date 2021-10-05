using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
	using Looxid.Link;
	using UnityEngine.Events;

	/// <summary>
	/// API ��û ��ǥ
	/// </summary>
	public enum Objective
	{
		EEG,
		Relaxation,
		Attention
	}

	public enum MindIndex
	{
		Attention,
		Relaxation
	}

	public sealed class Brainwave : _API
	{
		/// <summary>
		/// ���� �������̽��� ������. �ʿ� �����͸� �����ڷ� ������
		/// </summary>
		/// <param name="objective"> ��û ��ǥ </param>
		/// <param name="targetId"> ��û ���������� </param>
		/// <param name="targetSecond"> ��û ���� �� ���� </param>
		/// <param name="targetCallBack"> ������ ���� �� ���� �̺�Ʈ </param>
		public Brainwave(Objective obj, EEGSensorID targetId, float targetSecond, UnityAction<Brainwave> targetCallBack)
		{
			objective = obj;
			id = targetId;
			second = targetSecond;
			callBack = targetCallBack;
		}

		public Brainwave(Objective obj, UnityAction<Brainwave> targetCallBack)
		{
			objective = obj;
			callBack = targetCallBack;
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
			beta  = _beta;
			gamma = _gamma;
		}

		public void Set(Objective obj, float value)
		{
			if(obj == Objective.Attention)
			{
				attention = value;
			}
			else if(obj == Objective.Relaxation)
			{
				relaxation = value;
			}
		}

		/// <summary>
		/// ���� ������. ������, �޼��忡 ���� �����Ͱ� �Ҵ��
		/// </summary>
		private Objective objective;

		#region private EEG
		private EEGSensorID id;
		private float second;
		private UnityAction<Brainwave> callBack;

		private float delta;
		private float theta;
		private float alpha;
		private float beta;
		private float gamma;
		#endregion

		#region private mind Index

		private float relaxation;
		private float attention;

		#endregion

		/// <summary>
		/// �ܺ� ��� ������Ƽ. �����͸� �о�ö��� ������Ƽ�� ��� �����ϴ�.
		/// </summary>
		public Objective Objective { get => objective; } // ��û ���� ������ ��ǥ

		#region public EEG
		public EEGSensorID Id { get => id; }		// ��û : EEG ��������
		public float Second { get => second; }		// ��û : ���� �ð�����
		public UnityAction<Brainwave> CallBack { get => callBack;}	// ��û : ������ ��ȯ �̺�Ʈ

		public float Delta { get => delta; }		// ��� : ��Ÿ�� ����ġ 0 ~ 1
		public float Theta { get => theta; }		// ��� : ��Ÿ�� ����ġ 0 ~ 1
		public float Alpha { get => alpha; }		// ��� : ������ ����ġ 0 ~ 1
		public float Beta { get => beta; }			// ��� : ��Ÿ�� ����ġ 0 ~ 1
		public float Gamma { get => gamma; }        // ��� : ������ ����ġ 0 ~ 1
		#endregion

		#region public mind Index
		public float Relaxation { get => relaxation; }  // ���� ���� 0 ~ 1
		public float Attention { get => attention; }    // ���� ���� 0 ~ 1
		#endregion
	}
}
