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

		// �ִ� timestamp ���Ѱ�
		public double timeResist;

		private double m_TimeSum = 0;

		public Queue<EEGFeatureIndex> indexes;

		/// <summary>
		/// ���ο� ������ �߻��� ť�� ������Ʈ�Ѵ�.
		/// </summary>
		/// <param name="newData"></param>
		public void UpdateQueue(EEGFeatureIndex newData)
		{
			// �� �����͸� ť�� �ִ´�.
			indexes.Enqueue(newData);

			// ���� ���� �������� timestamp
			double currTime = newData.timestamp;

			double outTime = 0;

			// �ð� �����ں��� �����Ͱ� ���� ���϶�
			if(m_TimeSum + currTime > timeResist)
			{
				EEGFeatureIndex outData = indexes.Dequeue();

				outTime = outData.timestamp;
			}

			// ���� �ð����� ������ �������� �ð��� ���ϰ� ���� �������� �ð��� ����.
			m_TimeSum = m_TimeSum + currTime - outTime;
		}

		// ���ο� EEGFeatureIndex�� �����ϴ� �޼���



		/// <summary>
		/// ������ ����
		/// </summary>
		/// <param name="timestamp"></param>
		public void GetByTimeStamp(double timestamp)
		{
			EEGFeatureIndex[] values = indexes.ToArray();

			int index = 0;

			double timeSum = 0;
			List<EEGFeatureIndex> datas = new List<EEGFeatureIndex>();

			// �䱸�� �ð� �ѷ��� �ٻ簪���� 
			while(true)
			{
				EEGFeatureIndex data = values[index++];

				//timeSum += data.timestamp;

				// ��û Ÿ�̸� ������ ���� ������
				if(timeSum + data.timestamp <= timestamp)
				{
					datas.Add(data);

					timeSum += data.timestamp;
				}
				// ��û Ÿ�̸� ���� �ʰ��� ���
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

				// ����� ����� result�� ���
				// -> 1 �׳� ��ȯ�ϱ⿣ ����ȭ ����̶� ����
				// -> 2 �� EEGFeatureIndex�� ���ڰ����� �Ѹ��� �޼��� ����
			}

			{
				//BandPower �������� double[] �Ϲ� ���ļ��� ����
				// ������ bandPower double[][]���� ù []�� ���� index��
				// �� ��° []�� �Ʒ��� BandPower���ΰ�?

				// bandPower[0][] : Sensor 1
				// bandPower[1][] : Sensor 2
				// bandPower[2][] : Sensor 3
				// bandPower[3][] : Sensor 4
				// bandPower[4][] : Sensor 5
				// bandPower[5][] : Sensor 6
			}
		}

		/// <summary>
		/// FeatureIndex ����Ʈ�� �����͸� �ϳ��� �迭�� ��ȯ
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
		/// ���� FeatureIndex �����͸� Ư�� ����Ʈ�� �Ҵ�
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
