using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySpace.Info;
using MySpace.Animation;
using MySpace.Type;

namespace MySpace.Main
{
    public class PlayerController : MonoBehaviour
    {
        //玩家列表
        public static List<PlayerInfo> players = new List<PlayerInfo>();
        //创建玩家
        public void CreatePlayer()
        {
            PlayerInfo player = new PlayerInfo();
            //创建玩家0
            players.Add(player);
        }

        void Start()
        {
            CreatePlayer();
            InputController.SingleListenKey();
        }

    }
}