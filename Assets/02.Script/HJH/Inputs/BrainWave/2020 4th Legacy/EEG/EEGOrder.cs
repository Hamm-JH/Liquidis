using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEG
{
    public class EEGOrder
    {
        public Data.CollectionStatus collectionStatus;
        public double[] eegDataArray;

        public EEGOrder()
        {
            eegDataArray = new double[Enum.GetValues(typeof(Data.EEG)).Length];
        }
    }
}
