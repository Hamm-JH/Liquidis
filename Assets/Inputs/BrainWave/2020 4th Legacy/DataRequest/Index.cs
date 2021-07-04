using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Index
    {
        public float? timeStamp = null;
        public MindIndex mindIndex = MindIndex.Null;
        public CollectionStatus collectionStatus = CollectionStatus.Null;

        #region initialize

        public Index(MindIndex _mindIndex,
            CollectionStatus _collectionStatus = CollectionStatus.Null)
        {
            timeStamp = null;
            mindIndex = _mindIndex;
            collectionStatus = _collectionStatus;
        }

        public Index(MindIndex _mindIndex,
            float _timeStamp,
            CollectionStatus _collectionStatus = CollectionStatus.Null)
        {
            timeStamp = _timeStamp;
            mindIndex = _mindIndex;
            collectionStatus = _collectionStatus;
        }

        #endregion

        #region Set

        public void Set(MindIndex _mindIndex,
            CollectionStatus _collectionStatus = CollectionStatus.Null)
        {
            timeStamp = null;
            mindIndex = _mindIndex;
            collectionStatus = _collectionStatus;
        }

        public void Set(MindIndex _mindIndex,
            float? _timeStamp = null,
            CollectionStatus _collectionStatus = CollectionStatus.Null)
        {
            timeStamp = _timeStamp;
            mindIndex = _mindIndex;
            collectionStatus = _collectionStatus;
        }

        public void Set(MindIndex _mindIndex,
            CollectionStatus _collectionStatus = CollectionStatus.Null,
            float? _timeStamp = null)
        {
            timeStamp = _timeStamp;
            mindIndex = _mindIndex;
            collectionStatus = _collectionStatus;
        }

        #endregion


    }
}
