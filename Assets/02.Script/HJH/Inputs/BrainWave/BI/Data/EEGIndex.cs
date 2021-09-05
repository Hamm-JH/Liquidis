using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;

namespace Looxid.Link
{
	public class EEGIndex
	{
		private static EEGIndex instance;

		public static EEGIndex Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new EEGIndex();
				}
				return instance;
			}
		}

		// 최대 timestamp 제한값
		public double timeResist;

		private double m_TimeSum = 0;

		public Queue<EEGFeatureIndex> indexes;

		/// <summary>
		/// 새로운 데이터 발생시 큐를 업데이트한다.
		/// </summary>
		/// <param name="newData"></param>
		public void UpdateQueue(EEGFeatureIndex newData)
		{
			// 새 데이터를 큐에 넣는다.
			indexes.Enqueue(newData);

			// 지금 넣은 데이터의 timestamp
			double currTime = newData.timestamp;

			double outTime = 0;

			// 시간 제한자보다 데이터가 많이 쌓일때
			if(m_TimeSum + currTime > timeResist)
			{
				EEGFeatureIndex outData = indexes.Dequeue();

				outTime = outData.timestamp;
			}

			// 쌓인 시간에서 더해진 데이터의 시간을 더하고 빠진 데이터의 시간을 뺀다.
			m_TimeSum = m_TimeSum + currTime - outTime;
		}

		// 새로운 EEGFeatureIndex를 생성하는 메서드



		/// <summary>
		/// 데이터 수집
		/// </summary>
		/// <param name="timestamp"></param>
		public void GetByTimeStamp(double timestamp)
		{
			EEGFeatureIndex[] values = indexes.ToArray();

			int index = 0;

			double timeSum = 0;
			List<EEGFeatureIndex> datas = new List<EEGFeatureIndex>();

			// 요구된 시간 총량에 근사값으로 
			while(true)
			{
				EEGFeatureIndex data = values[index++];

				//timeSum += data.timestamp;

				// 요청 타이머 값보다 합이 작을때
				if(timeSum + data.timestamp <= timestamp)
				{
					datas.Add(data);

					timeSum += data.timestamp;
				}
				// 요청 타이머 값을 초과한 경우
				else
				{
					break;
				}
			}

			EEGFeatureIndex result = null;

			if(datas != null)
			{
				double[][] featureData = GetFeatureData(datas);

				result = new EEGFeatureIndex(timeSum, featureData);

				// 결과로 검출된 result의 사용
				// -> 1 그냥 반환하기엔 동기화 방식이라 문제
				// -> 2 새 EEGFeatureIndex를 인자값으로 뿌리는 메서드 실행
			}

			{
				//BandPower 가져오면 double[] 일반 주파수로 가정
				// 생성자 bandPower double[][]에서 첫 []는 센서 index값
				// 두 번째 []는 아래의 BandPower값인가?

				// bandPower[0][] : Sensor 1
				// bandPower[1][] : Sensor 2
				// bandPower[2][] : Sensor 3
				// bandPower[3][] : Sensor 4
				// bandPower[4][] : Sensor 5
				// bandPower[5][] : Sensor 6
			}
		}

		/// <summary>
		/// FeatureIndex 리스트의 데이터를 하나의 배열로 변환
		/// </summary>
		/// <param name="indexes"></param>
		/// <returns></returns>
		private double[][] GetFeatureData(List<EEGFeatureIndex> indexes)
		{
			List<double> sensor1 = new List<double>();
			List<double> sensor2 = new List<double>();
			List<double> sensor3 = new List<double>();
			List<double> sensor4 = new List<double>();
			List<double> sensor5 = new List<double>();
			List<double> sensor6 = new List<double>();

			int index = indexes.Count;
			for (int i = 0; i < index; i++)
			{
				SetSingleFeatureData(indexes[i],
					ref sensor1,
					ref sensor2,
					ref sensor3,
					ref sensor4,
					ref sensor5,
					ref sensor6);
			}

			double[][] result = new double[6][];
			result[0] = sensor1.ToArray();
			result[1] = sensor2.ToArray();
			result[2] = sensor3.ToArray();
			result[3] = sensor4.ToArray();
			result[4] = sensor5.ToArray();
			result[5] = sensor6.ToArray();

			return result;
		}

		/// <summary>
		/// 단일 FeatureIndex 데이터를 특정 리스트에 할당
		/// </summary>
		/// <param name="data"></param>
		/// <param name="sensor1"></param>
		/// <param name="sensor2"></param>
		/// <param name="sensor3"></param>
		/// <param name="sensor4"></param>
		/// <param name="sensor5"></param>
		/// <param name="sensor6"></param>
		private void SetSingleFeatureData(EEGFeatureIndex data,
			ref List<double> sensor1,	ref List<double> sensor2,	ref List<double> sensor3,
			ref List<double> sensor4,	ref List<double> sensor5,	ref List<double> sensor6)
		{
			sensor1.AddRange(data.BandPower(EEGSensorID.AF3));
			sensor2.AddRange(data.BandPower(EEGSensorID.AF4));
			sensor3.AddRange(data.BandPower(EEGSensorID.Fp1));
			sensor4.AddRange(data.BandPower(EEGSensorID.Fp2));
			sensor5.AddRange(data.BandPower(EEGSensorID.AF7));
			sensor6.AddRange(data.BandPower(EEGSensorID.AF8));
		}
	}
}
