using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MovieScene
{
    public class MovieControl : MonoBehaviour
    {
        public Transform targetX;
        public Transform targetY;

        #region X

        [Header("targetX")]
        public float minX;
        public float maxX;

        [Header("�̵� ��������")]
        [SerializeField] private float cacheX;

        #endregion

        #region Y
        [Header("center ��ü�� �ִ� �ּ� ȸ����")]
        public float minY;
        public float maxY;

        [Header("ȸ�� ��������")]
        [SerializeField] private float cacheY;
        #endregion

        public void UpdateX(float value)
        {
            cacheX = targetX.localPosition.z;
            cacheX += value * Time.deltaTime;

            cacheX = Mathf.Clamp(cacheX, minX, maxX);

            Vector3 pos = targetX.localPosition;

            targetX.localPosition = new Vector3(
                pos.x,
                pos.y,
                cacheX
                );
        }

        public void UpdateY(float value)
        {
            cacheY = targetY.rotation.eulerAngles.y;
            cacheY += value /** Time.deltaTime*/;

            cacheY = Mathf.Clamp(cacheY, minY, maxY);

            targetY.rotation = Quaternion.Euler(0, cacheY, 0);
        }

        public void DebugToIntro()
        {
            SceneManager.LoadScene("01_Intro");
        }
    }
}
