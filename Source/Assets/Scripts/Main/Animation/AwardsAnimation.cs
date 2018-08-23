using UnityEngine;
using System.Collections;
namespace MySpace.Animation
{
    public class AwardsAnimation : MonoBehaviour
    {
        public GameObject[] awards;
        public CupAnimation[] cups;


        void Start()
        {
            float lenght = GetComponent<SpriteRenderer>().bounds.size.y;
            //Debug.Log(lenght);
            cups = new CupAnimation[awards.Length];
            for (int i = 0; i < awards.Length; i++)
            {
                cups[i] = Instantiate(awards[i]).GetComponent<CupAnimation>();
                cups[i].transform.parent = transform;
                //cups[i].transform.localScale = new Vector3(
                //    cups[i].transform.localScale.x *transform.localScale.x ,
                //    cups[i].transform.localScale.y * transform.localScale.y);
                cups[i].transform.position = transform.position;
                cups[i].transform.localPosition = new Vector3(0.25f * lenght * (1f-1f/cups[i].price), 0);
                //Debug.Log(cups[i].transform.localPosition);
                cups[i].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            }
        }
    }
}