using UnityEngine;
using System.Collections;
namespace MySpace.Demo
{
    public class Demo : MonoBehaviour
    {
        void Start()
        {
            if (JavaInterface.Java())
            {
                InputPort.onChangeScoreEvent += (int value) =>
                {
                    Debug.Log(value);
                };
                InputPort.onKeyDownEventList[0] += () =>
                {
                    Debug.Log("0 键被按下");
                };
                InputPort.onKeyDownEvent += () =>
                {
                    Debug.Log("按键被按下");
                };
                //获取玩家分数
                Debug.Log(JavaInterface.GetScorePlayer());
                //设置玩家分数
                JavaInterface.SetInt("ScorePlayer", 100);
                Debug.Log(JavaInterface.GetScorePlayer());
                //添加玩家分数
                JavaInterface.AddScorePlayer(100);
                Debug.Log(JavaInterface.GetScorePlayer());
                //JavaInterface.PlayVideo();
                Debug.Log(JavaInterface.GetInt("ScorePlayer"));
            }
        }
    }
}