using UnityEngine;
using System.Collections;
namespace MySpace.Animation
{
    public class ScrollAnimation : MonoBehaviour
    {
        public bool isUpdate;
        public float valueMax = 1;
        public float valueA;
        public float valueB;
        public float valueWhite;
        public Color color;
        public bool isColor;
        public GameObject scrollA;
        public GameObject scrollB;
        public GameObject scrollWhite;
        SpriteRenderer rendererA;
        SpriteRenderer rendererB;
        SpriteRenderer rendererWhite;
        void Awake()
        {
            isUpdate = true;
            rendererA = Instantiate(scrollA).GetComponent<SpriteRenderer>();
            rendererA.gameObject.transform.parent = transform;
            rendererA.gameObject.transform.rotation = transform.rotation;
            rendererA.gameObject.transform.localScale = new Vector3(1, 1, 1);
            rendererA.gameObject.transform.position = transform.position;

            rendererWhite = Instantiate(scrollWhite).GetComponent<SpriteRenderer>();
            rendererWhite.gameObject.transform.parent = transform;
            rendererWhite.gameObject.transform.rotation = transform.rotation;
            rendererWhite.gameObject.transform.localScale = new Vector3(1, 1, 1);
            rendererWhite.gameObject.transform.position = transform.position;

            UpdateStatus = OnUpdateStatus();
        }
        public void Refresh()
        {
            rendererA.material.SetFloat("_Range", valueA);
            if (isColor)
            {
                rendererA.material.color = color;
            }
        }
        public void Flash(float value= 0.75f)
        {
            valueWhite += value;
            //Debug.Log("+++"+valueWhite);
        }
        IEnumerator UpdateStatus;
        IEnumerator OnUpdateStatus()
        {
            while (true)
            {
                rendererWhite.material.SetFloat("_Range", valueA*1.025f);
                rendererWhite.material.color = new Color(1,1,1, valueWhite);
                valueWhite *= 0.5f;
                //Debug.Log(valueWhite);
                yield return new WaitForSeconds(0.025f);
            }
        }
        void OnEnable()
        {
            StartCoroutine(UpdateStatus);
        }
        void OnDisable()
        {
            StopCoroutine(UpdateStatus);
        }
    }
}