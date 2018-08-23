using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySpace.Type;

namespace MySpace.Animation
{
    public class BridAnimation : MonoBehaviour
    {
        public BridType[] brids;
        public int counterMax;
        public float speedMax;
        public float speedMin;
        List<Brid> bridList;

        class Brid
        {
            public GameObject gameObject;
            public BridType type;
            public int status;
            public float duration;
            public float speed;
        }
        // Use this for initialization
        void Start()
        {
            bridList = new List<Brid>();
            StartCoroutine(MotionClound());
        }
        IEnumerator MotionClound()
        {
            float speed = Random.Range(speedMin, speedMax);
            while (true)
            {
                if (bridList.Count == counterMax / 2)
                {
                    speed = Random.Range(speedMin, speedMax);
                }
                if (bridList.Count < counterMax)
                {
                    if (Random.Range(0, 1000) > 997)
                    {
                        Brid brid = new Brid();
                        brid.gameObject = new GameObject();
                        brid.type = brids[Random.Range(0, brids.Length)].GetComponent<BridType>();
                        brid.gameObject.AddComponent<SpriteRenderer>().sprite = brid.type.sprites[0];
                        brid.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -3;
                        brid.gameObject.transform.parent = transform;
                        brid.gameObject.transform.position = new Vector3(-2, 0.1f + Random.Range(0, 0.3f));
                        brid.gameObject.transform.localScale = Random.Range(0.75f, 1f) * new Vector3(1, 1);
                        brid.speed = speed;
                        bridList.Add(brid);
                    }
                }
                if (bridList.Count > 0)
                {
                    try
                    {
                        for(int i=0;i<bridList.Count;i++)
                        {
                            if (bridList[i].gameObject.transform.position.x > 2f)
                            {
                                Destroy(bridList[i].gameObject);
                                bridList.Remove(bridList[i]);
                            }
                            else
                            {
                                bridList[i].gameObject.transform.position += new Vector3(bridList[i].speed, 0);
                                bridList[i].status++;
                                if (bridList[i].status == 10)
                                {
                                    bridList[i].gameObject.GetComponent<SpriteRenderer>().sprite = bridList[i].type.sprites[0];
                                    bridList[i].gameObject.transform.position += new Vector3(0, -0.01f);
                                }
                                if(bridList[i].status == 20)
                                {
                                    bridList[i].gameObject.GetComponent<SpriteRenderer>().sprite = bridList[i].type.sprites[1];
                                    bridList[i].gameObject.transform.position += new Vector3(0, 0.01f);
                                    bridList[i].status = 0;
                                }
                            }
                        }
                    }
                    catch(System.Exception ex) { Debug.Log(ex.StackTrace); }
                }
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}
