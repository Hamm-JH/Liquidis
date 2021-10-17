using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

namespace CrazyMinnow.SALSA.Addons.Photon
{
    public class SalsaPhotonRecorder : MonoBehaviour
    {
        private SimpleProcessorFloat simpleProcessorFloat;
        private SimpleProcessorShort simpleProcessorShort;

        private float PeakAmplitude
        {
            get
            {
                if (simpleProcessorFloat != null)
                    return simpleProcessorFloat.PeakAmplitude;

                if (simpleProcessorShort != null)
                    return simpleProcessorShort.PeakAmplitude * (1.0f / short.MaxValue);

                return 0f;
            }
        }

        public float GetAnalysis()
        {
            return PeakAmplitude;
        }

        private void PhotonVoiceCreated(PhotonVoiceCreatedParams p)
        {
            if (p.Voice is LocalVoiceAudioFloat)
            {
                LocalVoiceAudioFloat v = p.Voice as LocalVoiceAudioFloat;
                simpleProcessorFloat = new SimpleProcessorFloat();
                v.AddPreProcessor(simpleProcessorFloat);
            }
            else if (p.Voice is LocalVoiceAudioShort)
            {
                LocalVoiceAudioShort v = p.Voice as LocalVoiceAudioShort;
                simpleProcessorShort = new SimpleProcessorShort();
                v.AddPreProcessor(simpleProcessorShort);
            }
        }

        private void PhotonVoiceRemoved()
        {
            if (simpleProcessorFloat != null)
                simpleProcessorFloat.Dispose();

            if (simpleProcessorShort != null)
                simpleProcessorShort.Dispose();
        }
    }

    public class SimpleProcessorFloat : IProcessor<float>, IPeakAmplitude<float>
    {
        private bool disposed;

        public float PeakAmplitude { get; private set; }

        // maybe calculate db here: https://stackoverflow.com/questions/4152201/calculate-decibels
        public float[] Process(float[] buffer)
        {
            PeakAmplitude = 0f;
            float bufferAbsCalc;

            if (disposed)
                return null;

            for (int i = 0; i < buffer.Length; i++)
            {
                bufferAbsCalc = buffer[i] < 0f ? buffer[i] * -1 : buffer[i];
                if (bufferAbsCalc > PeakAmplitude)
                    PeakAmplitude = bufferAbsCalc;
            }
            return buffer;
        }

        public void Dispose()
        {
            disposed = false;
        }
    }

    public class SimpleProcessorShort : IProcessor<short>, IPeakAmplitude<short>
    {
        private bool disposed;

        public short PeakAmplitude { get; private set; }

        public void Dispose()
        {
            disposed = false;
        }

        public short[] Process(short[] buffer)
        {
            PeakAmplitude = 0;
            short bufferAbsCalc;

            if (disposed)
                return null;

            for (int i = 0; i < buffer.Length; i++)
            {
                bufferAbsCalc = buffer[i] < 0f ? (short)(buffer[i] * -1) : buffer[i];
                if (bufferAbsCalc > PeakAmplitude)
                    PeakAmplitude = bufferAbsCalc;
            }
            return buffer;
        }
    }

    interface IPeakAmplitude<T>
    {
        T PeakAmplitude { get; }
    }
}