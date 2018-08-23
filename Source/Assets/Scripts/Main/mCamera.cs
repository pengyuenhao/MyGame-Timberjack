using UnityEngine;
using System.Collections;
namespace MySpace.Camera
{
    public class mCamera : MonoBehaviour
    {
        public static int targetFrameRate = 60;
        // Use this for initialization
        //当程序唤醒时
        void Awake()
        {
            //Debug.Log("新建" + this + mCamera.counter++);
            //修改当前的FPS
            Application.targetFrameRate = targetFrameRate;
        }
        void Start()
        {
            Debug.Log("版本号:1.1.1a");
            Screen.fullScreen = false;
        }
    }
}