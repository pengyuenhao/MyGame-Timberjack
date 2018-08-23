using UnityEngine;
using System.Collections;
namespace MySpace.Animation
{
    public class ChopAnimation : MonoBehaviour
    {
        bool isHide;
        void Start()
        {
            StartCoroutine(UpdateStatus());
        }
        public void Display()
        {
            isHide = false;
            //GetComponent<Collider>().enabled = true;
        }
        public void Hide()
        {
            isHide = true;
            //GetComponent<Collider>().enabled = false;
        }
        IEnumerator UpdateStatus()
        {
            Vector3 localPosition = gameObject.transform.localPosition;
            Vector3 localScale = gameObject.transform.localScale;
            float alpha = 1;
            float current = 0;
            float speed = 0.1f;
            while (true)
            {
                if (isHide)
                {
                    alpha -= 0.1f;
                }
                else
                {
                    alpha += 0.05f;
                }
                if (alpha > 1) alpha = 1;
                if (alpha < 0) alpha = 0;

                if (current > 1) speed = -0.25f;
                if (current < -1) speed = 0.5f;
                current += speed;
                gameObject.transform.localScale = localScale + current * new Vector3(0.05f, 0.05f);
                gameObject.transform.localPosition = localPosition + current * new Vector3(localPosition.x * 0.05f,0);
                GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<Renderer>())
                    {
                        transform.GetChild(i).GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
                    }
                }
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}