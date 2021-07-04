using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum CollectionStatus
    {
        Null = -1,
        Await = 0,
        Reference = 1,
        Content01 = 2,
    }

    public enum EEG
    {
        Delta = 0,
        Theta = 1,
        Alpha = 2,
        Beta = 3,
        Gamma = 4
    }

    public enum MindIndex
    {
        Null = -1,
        Excitement = 1, // 흥분도
        Positivity = 2, // 긍부정도 (100 긍정)
        Empathy = 3,    // 공감도
        Attention = 4,  // 집중도 (링크 제공)
        Relaxation = 5, // 이완도  (링크 제공)
    }
}
