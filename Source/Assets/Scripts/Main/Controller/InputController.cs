using UnityEngine;
using System.Collections;
using MySpace.Event;
using MySpace.Animation;
using MySpace.Info;

namespace MySpace.Main
{
    public class InputController : MonoBehaviour
    {
        public delegate void PlayerHandler(PlayerInfo player);
        public static event PlayerHandler onAttackLeft;
        public static event PlayerHandler onAttackRight;
        public static event PlayerHandler onEveryKey;
        public static void SingleListenKey()
        {
            #region 为单个玩家分配按键监听
            //清除按键监听
            InputEvent.RemoveKey("a");
            InputEvent.RemoveKey("d");
            InputEvent.RemoveKey("left");
            InputEvent.RemoveKey("right");
            if (PlayerController.players.Count > 0)
            {
                //注册玩家按键事件0
                InputPort.onKeyDownEventList[0] += () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackLeft(PlayerController.players[0]);
                };
                //注册玩家按键事件1
                InputPort.onKeyDownEventList[1] += () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackRight(PlayerController.players[0]);
                };

                InputEvent.ListenKeyDown("a", () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackLeft(PlayerController.players[0]);
                });
                InputEvent.ListenKeyDown("d", () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackRight(PlayerController.players[0]);
                });
                InputEvent.ListenKeyDown("left", () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackLeft(PlayerController.players[0]);
                });
                InputEvent.ListenKeyDown("right", () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackRight(PlayerController.players[0]);
                });
            }
            #endregion
        }
        public static void DoubleListenKey()
        {
            //清除按键监听
            InputEvent.RemoveKey("a");
            InputEvent.RemoveKey("d");
            InputEvent.RemoveKey("left");
            InputEvent.RemoveKey("right");
            #region 为两个玩家分配按键监听
            if (PlayerController.players.Count > 1)
            {
                //注册玩家按键事件0
                InputPort.onKeyDownEventList[0] += () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackLeft(PlayerController.players[0]);
                };
                //注册玩家按键事件1
                InputPort.onKeyDownEventList[1] += () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackRight(PlayerController.players[0]);
                };

                InputEvent.ListenKeyDown("a", () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackLeft(PlayerController.players[0]);
                });
                InputEvent.ListenKeyDown("d", () =>
                {
                    OnEveryKey(PlayerController.players[0]);
                    OnAttackRight(PlayerController.players[0]);
                });
                //注册玩家按键事件0
                InputPort.onKeyDownEventList[2] += () =>
                {
                    OnEveryKey(PlayerController.players[1]);
                    OnAttackLeft(PlayerController.players[1]);
                };
                //注册玩家按键事件1
                InputPort.onKeyDownEventList[3] += () =>
                {
                    OnEveryKey(PlayerController.players[1]);
                    OnAttackRight(PlayerController.players[1]);
                };
                InputEvent.ListenKeyDown("left", () =>
                {
                    OnEveryKey(PlayerController.players[1]);
                    OnAttackLeft(PlayerController.players[1]);
                });
                InputEvent.ListenKeyDown("right", () =>
                {
                    OnEveryKey(PlayerController.players[1]);
                    OnAttackRight(PlayerController.players[1]);
                });
            }
            #endregion
        }
        static void OnAttackLeft(PlayerInfo player)
        {
            if (onAttackLeft != null) onAttackLeft(player);
        }
        static void OnAttackRight(PlayerInfo player)
        {
            if (onAttackRight != null) onAttackRight(player);
        }
        static void OnEveryKey(PlayerInfo player)
        {
            if (onEveryKey != null) onEveryKey(player);
        }

        void Start()
        {
            onAttackLeft += GameEvent.OnAttackLeft;
            onAttackRight += GameEvent.OnAttackRight;
            onEveryKey += GameEvent.OnEveryKey;
            InputEvent.onPress += (GameObject obj) =>
            {
                GameEvent.OnEveryKey(PlayerController.players[0]);
            };
            if (JavaInterface.Java())
            {
                InputPort.onKeyDownEvent += ()=> { GameEvent.OnEveryKey(PlayerController.players[0]); };
            }
        }
    }
}