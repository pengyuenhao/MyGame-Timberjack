using UnityEngine;
using System.Collections;
namespace MySpace.Animation
{
    public class WelcomeAnimation : MonoBehaviour
    {
        public Vector3 scale = new Vector3(1,1,1);
        public AudioSource audio;
        public AudioClip clip;
        void Start()
        {
            audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.volume = 0.25f;
            audio.loop = true;
            audio.Play();
            StartCoroutine(UpdateStatus());
        }
        bool isHide;
        public void Display()
        {
            isHide = false;
        }
        public void Hide()
        {
            isHide = true;
        }
        IEnumerator UpdateStatus()
        {
            float alpha = 1;
            while (true)
            {
                if (isHide)
                {
                    alpha -= 0.1f;
                }
                else
                {
                    alpha += 0.1f;
                }
                if (alpha > 1) alpha = 1;
                if (alpha < 0) alpha = 0;

                gameObject.transform.localScale = new Vector3(scale.x * 0.8f, scale.y * 0.8f) + alpha * new Vector3(scale.x * 0.2f, scale.y * 0.2f);
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