using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MySpace.Animation
{
    public class CloundAnimation : MonoBehaviour
    {
        public Sprite[] clounds;
        public int counterMax;
        public float speedMax;
        public float speedMin;
        List<Clound> cloundList;
        class Clound
        {
            public GameObject gameObject;
            public float speed;
        }
        // Use this for initialization
        void Start()
        {
            cloundList = new List<Clound>();
            StartCoroutine(MotionClound());
        }
        IEnumerator MotionClound()
        {
            float speed = Random.Range(speedMin, speedMax);
            while (true)
            {
                if(cloundList.Count == counterMax / 2)
                {
                    speed = Random.Range(speedMin, speedMax);
                }
                if (cloundList.Count < counterMax)
                {
                    if (Random.Range(0, 1000) > 995)
                    {
                        Clound clound = new Clound();
                        clound.gameObject = new GameObject();
                        clound.gameObject.AddComponent<SpriteRenderer>().sprite = clounds[Random.Range(0, clounds.Length - 1)];
                        clound.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -3;
                        clound.gameObject.transform.parent = transform;
                        clound.gameObject.transform.position = new Vector3(2, 0.2f + Random.Range(0, 0.5f));
                        clound.speed = speed;
                        cloundList.Add(clound);
                    }
                }
                for (int i = 0; i < cloundList.Count; i++)
                {
                    if (cloundList[i].gameObject.transform.position.x < -2f)
                    {
                        Destroy(cloundList[i].gameObject);
                        cloundList.Remove(cloundList[i]);
                    }
                    else
                    {
                        cloundList[i].gameObject.transform.position += new Vector3(cloundList[i].speed, 0);
                    }
                }
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}