using UnityEngine;
using System.Collections;
using MySpace.Info;

namespace MySpace.Event
{
    public class GameEvent
    {
        #region 委托类型
        public delegate void PlayerHandler(PlayerInfo player);
        public delegate void IntHandler(int value);
        public delegate void VoidHandler();
        #endregion
        #region 事件列表
        #region 重置事件
        public static event VoidHandler startReset;
        public static event VoidHandler onReset;
        public static event VoidHandler finishReset;

        public static void StartReset(){if (startReset != null) startReset();}
        public static void OnReset() { if (onReset != null) onReset(); }
        public static void FinishReset(){if (finishReset != null) finishReset();}
        #endregion
        #region 启动事件
        //完成游戏过程
        public static event VoidHandler onFinishGame;
        //开始游戏过程
        public static event VoidHandler onStartGame;
        //进入游戏欢迎界面
        public static event VoidHandler onWelcome;
        //关闭游戏欢迎界面
        public static event VoidHandler offWelcome;

        public static event VoidHandler onLaunchGame;
        public static event VoidHandler onVerifyGame;
        public static event VoidHandler onPlayGame;
        //等待游戏开始倒计时
        public static event IntHandler waitStartGame;
        public static void OnFinishGame() { if (onFinishGame != null) onFinishGame(); }
        public static void OnStartGame() { if (onStartGame != null) onStartGame(); }
        public static void OnLaunchGame() { if (onLaunchGame != null) onLaunchGame(); }
        public static void OnVerifyGame() { if (onVerifyGame != null) onVerifyGame(); }
        public static void OnPlayGame() { if (onPlayGame != null) onPlayGame(); }
        #endregion
        #region 游戏事件
        //玩家获得分数时
        public static event PlayerHandler onGetScore;
        //玩家死亡时
        public static event PlayerHandler onDeath;
        //玩家生命值变动时
        public static event PlayerHandler onHealth;

        public static event PlayerHandler onAttackLeft;
        public static event PlayerHandler onAttackRight;
        public static event PlayerHandler onEveryKey;
        
        public static void OnAttackLeft(PlayerInfo player) { if (onAttackLeft != null) onAttackLeft(player); }
        public static void OnAttackRight(PlayerInfo player) { if (onAttackRight != null) onAttackRight(player); }
        public static void OnEveryKey(PlayerInfo player) { if (onEveryKey != null) onEveryKey(player); }


        public static void OnGetScore(PlayerInfo player) { if (onGetScore != null) onGetScore(player); }
        public static void OnDeath(PlayerInfo player) { if (onDeath != null) onDeath(player); }
        public static void OnHealth(PlayerInfo player) { if (onHealth != null) onHealth(player); }

        public static void OnWelcome(){if (onWelcome != null) onWelcome();}
        public static void OffWelcome(){if (offWelcome != null) offWelcome();}
        public static void WaitStartGame(int value) { if (waitStartGame != null) waitStartGame(value); }
        #endregion
        #endregion
    }
}