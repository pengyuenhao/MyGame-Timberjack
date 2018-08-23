using UnityEngine;
using System.Collections;
using MySpace.Motion;

namespace MySpace.Animation
{
    public class PanelAnimation : MonoBehaviour
    {
        public GameObject[] cups;
        public Vector3 defaultPostion;
        public Vector3 targetPostion;
        public float motionTime;
        public bool isSmooth;
        GameObject cupLeft;
        GameObject cupRight;

        public void Award(int value)
        {
            GameObject cup = null;

            switch (value)
            {
                case 1:
                    cup = cups[0];
                    break;
                case 2:
                    cup = cups[1];
                    break;
                case 3:
                    cup = cups[2];
                    break;
                case 4:
                    cup = cups[3];
                    break;
                default:
                    break;
            }
            if (cupLeft != null) Destroy(cupLeft);
            if (cupRight != null) Destroy(cupRight);
            if (cup != null)
            {
                cupLeft = Instantiate(cup);
                cupRight = Instantiate(cup);
                cupLeft.transform.parent = transform;
                cupRight.transform.parent = transform;
                //cupLeft.transform.position = transform.position;
                //cupRight.transform.position = transform.position;
                cupLeft.transform.localPosition = new Vector3(0.3f, -0.5f);
                cupRight.transform.localPosition = new Vector3(-0.3f, -0.5f);
                cupLeft.transform.localScale = new Vector3(0.5f, 1, 1);
                cupRight.transform.localScale = new Vector3(0.5f, 1, 1);
                cupLeft.GetComponent<CupAnimation>().time = 0.75f;
                cupRight.GetComponent<CupAnimation>().time = 0.75f;
                cupLeft.GetComponent<CupAnimation>().rate = 0.25f;
                cupRight.GetComponent<CupAnimation>().rate = 0.25f;
                cupLeft.GetComponent<CupAnimation>().Flash();
                cupRight.GetComponent<CupAnimation>().Flash();
                //MyMotion.Get(cupLeft)
                //.Scale(new Vector3(1f,0.5f))
                //.Mode(MyMotion.Modes.Relative)
                //.BlendTime(0.25f)
                //.AddOnFinishListener(() => 
                //{
                //    MyMotion.Get(cupLeft).WaitReset();
                //})
                //.Run();
                //MyMotion.Get(cupRight)
                //.Scale(new Vector3(1f, 0.5f))
                //.Mode(MyMotion.Modes.Relative)
                //.BlendTime(0.25f)
                //.AddOnFinishListener(() =>
                //{
                //    MyMotion.Get(cupRight).WaitReset();
                //})
                //.Run();
            }
        }
        public void Display()
        {
            MyMotion.Get(gameObject)
                .Smooth(isSmooth)
                .Postion(targetPostion)
                .Mode(MyMotion.Modes.Absolute)
                .BlendTime(motionTime)
                .Run();
        }
        public void Hide()
        {
            MyMotion.Get(gameObject)
                .Smooth(isSmooth)
                .Postion(defaultPostion)
                .Mode(MyMotion.Modes.Absolute)
                .BlendTime(motionTime)
                .Run();
        }
    }
}