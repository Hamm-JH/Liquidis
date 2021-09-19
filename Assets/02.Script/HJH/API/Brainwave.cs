using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
	using Looxid.Link;

	public sealed class Brainwave : _API
	{
		/// <summary>
		/// 뇌파 인터페이스의 생성자. 필요 데이터를 생성자로 강제함
		/// </summary>
		/// <param name="targetId"> 요청 센서데이터 </param>
		/// <param name="targetSecond"> 요청 검출 초 간격 </param>
		public Brainwave(EEGSensorID targetId, float targetSecond)
		{
			id = targetId;
			second = targetSecond;
		}

		/// <summary>
		/// 정렬된 뇌파 인터페이스를 할당함. 프로퍼티의 용도를 읽기 전용으로 만들기 위함
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
		/// 내부 데이터. 생성자, 메서드에 의해 데이터가 할당됨
		/// </summary>
		private EEGSensorID id;
		private float second;
		private float delta;
		private float theta;
		private float alpha;
		private float beta;
		private float gamma;

		/// <summary>
		/// 외부 출력 프로퍼티. 데이터를 읽어올때만 프로퍼티가 사용 가능하다.
		/// </summary>
		public EEGSensorID Id { get => id; }		// 요청 : EEG 센서정보
		public float Second { get => second; }		// 요청 : 추출 시간간격
		public float Delta { get => delta; }		// 출력 : 델타파 비중치 0 ~ 1
		public float Theta { get => theta; }		// 출력 : 세타파 비중치 0 ~ 1
		public float Alpha { get => alpha; }		// 출력 : 알파파 비중치 0 ~ 1
		public float Beta { get => beta; }			// 출력 : 베타파 비중치 0 ~ 1
		public float Gamma { get => gamma; }		// 출력 : 감마파 비중치 0 ~ 1
	}
}
