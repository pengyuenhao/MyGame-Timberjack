using UnityEngine;
using System.Collections;
using MySpace.Type;
using MySpace.Info;
using MySpace.Event;
using MySpace.Animation;

namespace MySpace.Main
{
    public class GameController : MonoBehaviour
    {
        #region 设置变量
        public static GameController game;
        public static SceneType sceneType;

        public GameObject scene;
        public GameObject[] scenes;
        public GameObject[] units;
        public Sprite[] filter;

        public Vector3 offset = new Vector3(-0.2625f, -0.48f);
        #region 状态判定
        //正在进行游戏(指进行游戏的状态，例如暂停时为不在进行游戏，为非)
        public static bool isPlay;
        //正在进行游戏过程(指全局状态，直到游戏完成重新初始化之前都是正在进行游戏过程)
        public static bool isGame;
        //正在确认游戏结果(游戏完成之后进行结果确认，完成确认后表示可以重新初始化以结束一个游戏周期)
        public static bool isVerify;
        //正在进行初始化过程(游戏进行初始化需要一定的时间，完成后标志位值非)
        public static bool isOnInit;
        //完成初始化过程(游戏完成初始化之后才可以进入下个状态)
        public static bool isFinishInit;
        //由外部输入控制的是否启动游戏标志位
        public static bool isLaunchGame;
        //正式开始游戏
        public static bool isStartGame;
        //重新开始游戏进程
        public static bool isResetGame;
        //是否为单人模式
        public static bool isSingle;
        //阶段超时设置
        public static int overtime;

        public static bool isOutCoin;
        #endregion
        #region 配置变量
        /// <summary>
        /// 难度
        /// </summary>
        public static int diff = 0;
        /// <summary>
        /// 等级
        /// </summary>
        public static int level = 0;
        /// <summary>
        /// 每秒收到伤害
        /// </summary>
        public static float damage = 0.075f;
        #endregion
        #endregion
        #region 初始化
        void Awake()
        {
            if (game == null) game = this;
        }
        void Start()
        {
            isSingle = true;
            //注册事件
            GameEvent.onFinishGame += OnFinishGame;
            GameEvent.onStartGame += OnStartGame;
            GameEvent.onLaunchGame += OnLaunchGame;
            GameEvent.onVerifyGame += OnVerifyGame;
            GameEvent.onPlayGame += OnPlayGame;
            GameEvent.onAttackLeft += OnAttackLeft;
            GameEvent.onAttackRight += OnAttackRight;
            GameEvent.onEveryKey += OnEveryKey;
            GameEvent.onDeath += OnDeath;
            GameEvent.onGetScore += OnGetScore;

            Init();
        }
        void Init()
        {
            //启动游戏状态检测
            StartCoroutine(UpdateStatus());
            //以非重新开始的方式刷新场景
            SwitchScene(false);
            GameObject tmp = new GameObject();
            tmp.transform.parent = transform;
            SpriteRenderer renderer = tmp.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 100;
            renderer.sprite = filter[0];
            renderer.material.color = new Color(0, 0, 0, 1);
            Destroy(tmp,0.5f);
        }
        #endregion
        #region 游戏场景
        //刷新游戏场景
        private void RefreshScene()
        {
            //从场景列表中随机选择一种作为当前场景
            sceneType = scenes[Random.Range(0, scenes.Length)].GetComponent<SceneType>();
            //销毁之前的场景并重新实例化场景
            if (scene != null) Destroy(scene);
            scene = Instantiate(sceneType.scenes[Random.Range(0, sceneType.scenes.Length)]);
            scene.transform.parent = transform;
            foreach (PlayerInfo player in PlayerController.players)
            {
                RefreshPlayer(player);
            }
        }
        //刷新玩家控制的角色和树
        private void RefreshPlayer(PlayerInfo player)
        {
            Destroy(player.unit);
            Destroy(player.tree);
            player.unit = Instantiate(units[Random.Range(0, units.Length)]);
            player.tree = Instantiate(GameController.sceneType.trees[Random.Range(0, GameController.sceneType.GetComponent<SceneType>().trees.Length)]);
            player.isAlive = true;
            player.unit.transform.position = offset;
            player.unit.transform.parent = transform;
            player.tree.transform.parent = transform;
            player.healthMax = 100;
            player.health = player.healthMax/2;
            player.score = 0;
        }
        //刷新游戏场景(可以选择以重新开始的方式刷新或者以初始化的方式刷新)
        public void SwitchScene(bool isReset)
        {
            if (!isOnInit&&!isFinishInit)
            {
                StartCoroutine(OnSwitchScene(isReset));
            }
        }
        IEnumerator OnSwitchScene(bool isReset)
        {
            if (isFinishInit)
            {
                Debug.LogError("错误:场景已初始化完成");
                isFinishInit = false;
            };
            //如果正在进行初始化则认为是错误的触发
            if (!isOnInit)
            {
                //开始进行初始化
                isOnInit = true;
                GameObject tmp = new GameObject();
                tmp.transform.parent = transform;
                SpriteRenderer renderer = tmp.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = 100;
                renderer.sprite = filter[0];
                GameEvent.StartReset();
                for (int i = 0; i < 10; i++)
                {
                    renderer.material.color = new Color(0, 0, 0, (0.1f * i));
                    yield return new WaitForSeconds(0.025f);
                }
                RefreshScene();
                GameEvent.OnReset();
                diff = 0;
                level = 0;
                for (int i = 0; i < 10; i++)
                {
                    renderer.material.color = new Color(0, 0, 0, 1f - (0.1f * i));
                    yield return new WaitForSeconds(0.025f);
                }
                Destroy(tmp);
                GameEvent.FinishReset();
                if (isReset)
                {
                    isLaunchGame = true;
                }
                else
                {
                    GameEvent.OnWelcome();
                }
                //完成初始化
                isOnInit = false;
                isFinishInit = true;
            }
            else
            {
                Debug.LogError("错误:重复刷新场景");
            }
        }
        #endregion
        #region 时序功能
        IEnumerator UpdateStatus()
        {
            //临时变量
            int temp;

            WaitForSeconds wait = new WaitForSeconds(0.1f);
            //判断循环
            while (true)
            {
                //Debug.Log("启动游戏周期");
                //等待直到游戏初始化完成
                while (!isFinishInit){yield return wait;}
                //Debug.Log("完成初始化");
                //等待直到游戏进程启动
                while (!isLaunchGame)
                {
                    //街机模式下自动启动游戏进程
                    if (JavaInterface.Java())
                    {
                        //退币完成才能开始游戏
                        while (isOutCoin)
                        {
                            isOutCoin = JavaInterface.GetCoinOutFlag();
                            //Debug.Log(isOutCoin);
                            yield return wait;
                        }
                        if (JavaInterface.GetScorePlayer() >= 10)
                        {
                            JavaInterface.AddScorePlayer(-10);
                            isLaunchGame = true;
                        }
                    }
                    yield return wait;
                }
                //Debug.Log("启动游戏进程");
                GameEvent.OnLaunchGame();
                //等待直到倒计时完毕
                temp = 4;
                while (temp-- > 0)
                {
                    GameEvent.WaitStartGame(temp);
                    yield return new WaitForSeconds(0.375f);
                }
                GameEvent.OnPlayGame();
                //Debug.Log("完成倒计时");
                //等待直到正式开始游戏
                overtime = 100;
                while (!isPlay&& overtime-->0) { yield return wait; }
                if (overtime <= 0)
                {
                    //Debug.Log("等待超时");
                    isPlay = true;
                }
                //Debug.Log("开始游戏进程");
                GameEvent.OnStartGame();
                //等待直到验证完成
                while (!isVerify) { yield return wait; }
                //Debug.Log("验证游戏结果");
                //等待直到完成游戏进程
                while (isGame){yield return wait;}
                //Debug.Log("完成游戏进程");
                //完成后执行清理工作
                GameEvent.OnFinishGame();
                //检测间隔(默认0.1秒)
                yield return wait;
                //Debug.Log("完成整个周期");
            }
        }
        IEnumerator OnWaitDeath()
        {
            int overtime;
            bool isReset;
            yield return new WaitForSeconds(0.5f);
            foreach (PlayerInfo player in PlayerController.players)
            {
                player.isReset = false;
            }
            overtime = 100;
            while (true)
            {
                isReset = true;
                foreach (PlayerInfo player in PlayerController.players)
                {
                    //Debug.Log(player.name +","+ player.isReset);
                    if (player.isReset == false)
                    {
                        isReset = false;
                        break;
                    }
                }
                if (isReset == true) break;
                if (overtime-- <= 0) break;
                yield return new WaitForSeconds(0.1f);
            }
            //验证游戏结果
            GameEvent.OnVerifyGame();
        }
        IEnumerator OnGame()
        {
            while (isGame)
            {
                if (isPlay)
                {
                    foreach (PlayerInfo player in PlayerController.players)
                    {
                        if (player.health > 0)
                        {
                            player.health -= GameController.damage * (5 + (3 + GameController.diff) * GameController.level);
                            GameEvent.OnHealth(player);
                        }
                        else
                        {
                            GameEvent.OnDeath(player);
                        }
                    }
                }
                else
                {
                    break;
                }
                yield return new WaitForSeconds(0.025f);
            }
            //Debug.Log("结束游戏进程");
        }
        #endregion
        #region 事件处理
        void OnFinishGame()
        {
            isVerify = false;
            isFinishInit = false;
            isGame = false;
            isLaunchGame = false;
            isPlay = false;
            isStartGame = false;

            if (JavaInterface.Java())
            {
                if (JavaInterface.GetScorePlayer() >= 10)
                {
                    JavaInterface.AddScorePlayer(-10);
                    isResetGame = true;
                }
                else
                {
                    isResetGame = false;
                }
            }
            else
            {
                isResetGame = true;
            }
            if (isResetGame) {SwitchScene(true);}else{SwitchScene(false);}
        }
        void OnStartGame()
        {
            StartCoroutine(OnGame());
        }
        void OnLaunchGame()
        {
            isGame = true;
        }
        void OnVerifyGame()
        {
            if (!isVerify)
            {
                isVerify = true;
                isGame = false;
                isPlay = false;
                Debug.Log("验证游戏结果");
            }
        }
        void OnPlayGame()
        {
            isStartGame = true;
        }
        void OnDeath(PlayerInfo player)
        {
            if (isSingle)
            {
                //Debug.Log("!!!");
                //暂停游戏
                overtime = 0;
                isPlay = false;
                player.isAlive = false;
                player.unit.GetComponent<UnitAnimation>().Death();
                StartCoroutine(OnWaitDeath());
            }
        }
        void OnGetScore(PlayerInfo player)
        {
            if (player.index == 0)
            {
                if (player.score > (Mathf.Pow(GameController.level, 2f) * 10))
                {
                    GameController.level += 1;
                    //Debug.Log("Level Up" + GameController.level + "," + GameController.damage * (5 + (3+GameController.diff) * GameController.level));
                }
            }
            player.health += (GameController.damage * 100) * (player.healthMax / (player.health + 50));
            if (player.health > player.healthMax) player.health = player.healthMax;
            player.score += 1;
        }

        void OnAttackLeft(PlayerInfo player)
        {
            if (!player.isAlive) return;
            if (!isPlay) return;

            player.unit.GetComponent<UnitAnimation>().Exchange(false);
            player.unit.GetComponent<UnitAnimation>().Attack();

            if (player.tree.GetComponent<TreeAnimation>().isLeft)
            {
                GameEvent.OnDeath(player);
            }
            else
            {
                player.tree.GetComponent<TreeAnimation>().RemoveTrunk(new Vector3(2, 1f), new Vector3(0, 0, 90));
                if (player.tree.GetComponent<TreeAnimation>().isLeft)
                {
                    GameEvent.OnDeath(player);
                }
            }
            if (player.isAlive) GameEvent.OnGetScore(player);
        }
        void OnAttackRight(PlayerInfo player)
        {
            if (!player.isAlive) return;
            if (!isPlay) return;

            player.unit.GetComponent<UnitAnimation>().Exchange(true);
            player.unit.GetComponent<UnitAnimation>().Attack();
            if (player.tree.GetComponent<TreeAnimation>().isRight)
            {
                GameEvent.OnDeath(player);
            }
            else
            {
                player.tree.GetComponent<TreeAnimation>().RemoveTrunk(new Vector3(-2, 1f), new Vector3(0, 0, -90));
                if (player.tree.GetComponent<TreeAnimation>().isRight)
                {
                    GameEvent.OnDeath(player);
                }
            }
            if (player.isAlive) GameEvent.OnGetScore(player);
        }
        void OnEveryKey(PlayerInfo player)
        {
            if (isGame)
            {
                if (!player.isAlive)
                {
                    player.isReset = true;
                }
                else
                {
                    if (isStartGame)
                    {
                        isPlay = true;
                    }
                    //Debug.Log("!!!");
                }
            }
        }

        #endregion
    }
}