using UnityEngine;
using System.Collections;
namespace MySpace.Animation
{
    public class CupAnimation : MonoBehaviour
    {
        public float price;
        public float time = 0.5f;
        public float rate = 0.1f;
        IEnumerator OnFlashIE;
        Vector3 localScale;
        Quaternion rotate;
        void Start()
        {
            localScale = transform.localScale;
        }
        void OnDisable()
        {
            transform.localScale = localScale;
            Stop();
            //OnFlashIE = OnFlash();
        }
        public void Flash()
        {
            localScale = transform.localScale;
            rotate = transform.rotation;
            OnFlashIE = OnFlash(time);
            StartCoroutine(OnFlashIE);
        }
        public void Stop()
        {
            if(OnFlashIE!=null)StopCoroutine(OnFlashIE);
            //StopCoroutine(OnFlashIE);
        }
        public bool isFlash;
        IEnumerator OnFlash(float time = 0.5f)
        {
            isFlash = true;
            float current = 0;
            float value = 0;
            float speed = 0.1f;
            //localScale = transform.localScale;
            rotate = transform.rotation;
            while (current < time)
            {
                current += 0.025f;
                value = Mathf.Pow(current / time,0.5f);
                transform.localScale = localScale + value * new Vector3(0.35f, 0.25f);
                //transform.Rotate(new Vector3(0, 0,2.5f * Mathf.Sin(value * Mathf.PI)));
                yield return new WaitForSeconds(0.025f);
            }
            current = 0;
            time = 2 * time;
            while (current < time)
            {
                current += 0.025f;
                value = 1f- Mathf.Pow(current / time, 2);
                transform.localScale = localScale + value * new Vector3(0.35f, 0.25f);
                //transform.Rotate(new Vector3(0, 0, 2.5f * Mathf.Sin(value * Mathf.PI)));
                yield return new WaitForSeconds(0.025f);
            }
            while (isFlash)
            {
                if (value > 1) speed = -rate;
                if (value < -1) speed = rate;
                value += speed;
                transform.localScale = localScale + value * new Vector3(0.03f, 0.02f);
                yield return new WaitForSeconds(0.025f);
            }
            transform.localScale = localScale;
        }
    }
}