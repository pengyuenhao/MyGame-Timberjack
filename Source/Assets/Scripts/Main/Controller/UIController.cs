using UnityEngine;
using MySpace.Text;
using System.Collections;
using MySpace.Info;
using MySpace.Animation;
using MySpace.Event;

namespace MySpace.Main
{
    public class UIController : MonoBehaviour
    {
        public AudioSource audio;
        public AudioClip count;
        public AudioClip count_go;
        public AudioClip dieMarch;

        public GameObject scrollMain;
        public GameObject audioOption;
        public GameObject chopLeft;
        public GameObject chopRight;
        public GameObject welcomeMobile;
        public GameObject welcomeArcade;
        public GameObject scroll;
        public GameObject mainPanel;
        public GameObject[] texts;
        public static CustomText playerText;
        public static CustomText scoreText;
        public static CustomText gameText;
        public static CustomText bestText;
        public static int gameScore;
        public static int playerScore;
        public static int bestScore;
        public static int gameCounter;
        public static int allScore;
        public static float averageScore { get { return allScore / gameCounter; } }
        public static int awards = 0;
        public static int profit = 0;
        PanelAnimation mMainPanel;
        ScrollAnimation mScroll;
        CustomText lastScoreText;
        ChopAnimation mChopLeft;
        ChopAnimation mChopRight;
        OptionAnimation mAuidoOption;
        ScrollAnimation mScrollMain;
        AwardsAnimation mAwards;

        WelcomeAnimation mWelcome;
        void Start()
        {
            //PlayerPrefs.SetInt("Profit", 0);
            //PlayerPrefs.SetInt("AllScore",0);
            //PlayerPrefs.SetInt("GameCounter", 0);
            //PlayerPrefs.SetInt("BestScore", Random.Range(500,1000));
            audio = gameObject.AddComponent<AudioSource>();
            profit = PlayerPrefs.GetInt("Profit");
            bestScore = PlayerPrefs.GetInt("BestScore");
            allScore = PlayerPrefs.GetInt("AllScore");
            gameCounter = PlayerPrefs.GetInt("GameCounter");
            if (allScore < 1000) allScore = 1000;
            if (gameCounter < 10) gameCounter = 10;
            
            if (JavaInterface.Java())
            {
                if (bestScore < 500) bestScore = Random.Range(500, 1000);

                playerText = Instantiate(texts[5]).GetComponent<CustomText>();
                playerText.transform.parent = transform;
                playerText.transform.position = new Vector3(-1.125f, 0.5f);
                playerText.transform.localScale = new Vector3(0.15f, 0.15f);

                scoreText = Instantiate(texts[7]).GetComponent<CustomText>();
                scoreText.transform.parent = transform;
                scoreText.transform.position = new Vector3(-1.125f, 0.40f);
                scoreText.transform.localScale = new Vector3(0.075f, 0.075f);
                //scoreText.Init();
                //scoreText.Display(0);
                //playerText.gameObject.SetActive(false);
            }
            else
            {
                mAuidoOption = Instantiate(audioOption).GetComponent<OptionAnimation>();
                mAuidoOption.transform.parent = transform;
                mAuidoOption.transform.localScale = new Vector3(1.5f, 1.5f,1f);
                mAuidoOption.transform.position = new Vector3(1.15f, 0.575f);

                //Debug.Log(mAuidoOption);
                EventListener.Get(mAuidoOption.gameObject).onClick += (GameObject obj) =>
                {
                    mAuidoOption.OnClick();
                };
                EventListener.Get(mAuidoOption.gameObject).onPress += (GameObject obj) =>
                {
                    mAuidoOption.OnPress();
                };
                EventListener.Get(mAuidoOption.gameObject).onFree += (GameObject obj) =>
                {
                    mAuidoOption.OnFree();
                };
            }
            mChopLeft = Instantiate(chopLeft).GetComponent<ChopAnimation>();
            mChopRight = Instantiate(chopRight).GetComponent<ChopAnimation>();
            mChopLeft.transform.parent = transform;
            mChopRight.transform.parent = transform;
            mChopLeft.transform.position = new Vector3(-0.75f,-0.125f);
            mChopRight.transform.position = new Vector3(0.75f, -0.125f);

            mChopLeft.Hide();
            mChopRight.Hide();



            gameText = Instantiate(texts[4]).GetComponent<CustomText>();
            gameText.transform.parent = transform;
            gameText.transform.position = new Vector3(0, 0.25f);
            gameText.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            gameText.gameObject.SetActive(false);

            mMainPanel = Instantiate(mainPanel).GetComponent<PanelAnimation>();

            lastScoreText = Instantiate(texts[6]).GetComponent<CustomText>();
            lastScoreText.transform.parent = mMainPanel.transform;
            lastScoreText.transform.localScale = new Vector3(0.1f, 0.2f);
            lastScoreText.transform.position = mMainPanel.transform.position + new Vector3(0, -0.675f);

            bestText = Instantiate(texts[6]).GetComponent<CustomText>();
            bestText.transform.parent = mMainPanel.transform;
            bestText.transform.position = mMainPanel.transform.position + new Vector3(0, -0.425f);
            bestText.Display(bestScore);

            mScroll = Instantiate(scroll).GetComponent<ScrollAnimation>();
            mScroll.transform.position = new Vector3(0, 0.65f);
            mScroll.transform.parent = transform;
            mScroll.gameObject.SetActive(false);

            mScrollMain = Instantiate(scrollMain).GetComponent<ScrollAnimation>();
            mScrollMain.transform.position = new Vector3(1.2f, -0.15f);
            mScrollMain.transform.parent = transform;

            mScrollMain.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            for (int i = 0; i < mScrollMain.transform.childCount; i++)
            {
                mScrollMain.transform.GetChild(i).GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            }

            mAwards = mScrollMain.GetComponent<AwardsAnimation>();
            mScrollMain.gameObject.SetActive(false);

            if (JavaInterface.Java() == true)
            {
                mWelcome = Instantiate(welcomeArcade).GetComponent<WelcomeAnimation>();
            }
            else
            {
                mWelcome = Instantiate(welcomeMobile).GetComponent<WelcomeAnimation>();
            }
            mWelcome.transform.position = transform.position;
            mWelcome.transform.parent = transform;
            //mWelcome.transform.localScale = new Vector3(2, 2);
            mWelcome.Hide();
            mWelcome.gameObject.GetComponent<Collider>().enabled = false;

            GameEvent.onGetScore += OnGetScore;
            GameEvent.onDeath += OnDeath;
            GameEvent.onFinishGame += OnFinishGame;
            GameEvent.onReset += OnReset;
            GameEvent.startReset += StartReset;
            GameEvent.finishReset += FinishReset;
            GameEvent.onHealth += OnHealth;
            GameEvent.onStartGame += OnStartGame;
            GameEvent.onWelcome += OnWelcome;
            GameEvent.offWelcome += OffWelcome;
            GameEvent.waitStartGame += WaitStartGame;
            GameEvent.onLaunchGame += OnLaunchGame;

            EventListener.Get(mWelcome.gameObject).onClick += (GameObject obj) =>
            {
                GameController.isLaunchGame = true;
            };
            EventListener.Get(mChopLeft.gameObject).onPress += (GameObject obj) =>
            {
                GameController.isLaunchGame = true;
                GameEvent.OnAttackLeft(PlayerController.players[0]);
            };
            EventListener.Get(mChopRight.gameObject).onPress += (GameObject obj) =>
            {
                GameController.isLaunchGame = true;
                GameEvent.OnAttackRight(PlayerController.players[0]);
            };

            InputPort.onChangeScoreEvent += (int value) =>
            {
                playerText.Display(value/10, 0.1f);
                scoreText.Display(value % 10, 0.1f);
            };
        }
        void OnGetScore(PlayerInfo player)
        {

            if (player.index == 0)
            {
                float value = 0;
                AddGameScore(1);
                if (gameScore < averageScore)
                {
                    value = 0.5f * gameScore /averageScore;
                }
                else
                {
                    value = 1f - 0.5f * (Mathf.Pow(averageScore / gameScore,1f));
                }
                if (awards < 1 && gameScore >= mAwards.cups[0].price * averageScore)
                {
                    awards = 1;
                    mAwards.cups[0].Flash();
                }
                if (awards < 2 && gameScore >= mAwards.cups[1].price * averageScore)
                {
                    awards = 2;
                    mAwards.cups[1].Flash();
                }
                if (awards < 3 && gameScore >= mAwards.cups[2].price * averageScore)
                {
                    awards = 3;
                    mAwards.cups[2].Flash();
                }
                if (awards < 4 && gameScore >= mAwards.cups[3].price * averageScore)
                {
                    awards = 4;
                    mAwards.cups[3].Flash();
                }
                mScrollMain.valueA = value;
                //mScrollMain.color = new Color(mScrollMain.valueA,(215f / 255f)*  mScrollMain.valueA,0.5f - mScrollMain.valueA/2, 1);
                mScrollMain.Refresh();
                //mScrollMain.Flash(0.25f);
            }
        }
        void OnDeath(PlayerInfo player)
        {
            if (player.index == 0)
            {
                int award = 0;
                audio.clip = dieMarch;
                audio.Play();
                mScroll.gameObject.SetActive(false);
                mScrollMain.gameObject.SetActive(false);
                gameText.gameObject.SetActive(false);
                allScore = allScore + gameScore;
                gameCounter = gameCounter + 1;
                switch (awards)
                {
                    case 0:break;
                    case 1:
                        award = 1;
                        //Debug.Log("铜奖");
                        break;
                    case 2:
                        award = 2;
                        //Debug.Log("银奖");
                        break;
                    case 3:
                        award = 3;
                        allScore = (int)(0.8f * allScore + gameScore);
                        gameCounter = (int)(0.8f * gameCounter + 1);
                        //Debug.Log("金奖");
                        break;
                    case 4:
                        award = 5;
                        allScore = (int)(0.5f * allScore + gameScore);
                        gameCounter = (int)(0.5f * gameCounter + 1);
                        //Debug.Log("最高奖");
                        break;
                    default:break;
                }
                if (gameScore > bestScore)
                {
                    bestScore = gameScore;
                    PlayerPrefs.SetInt("BestScore",bestScore);
                }
                if (JavaInterface.Java())
                {
                    if (award!=0&&JavaInterface.GetBonus() > (10 * award))
                    {
                        JavaInterface.AddScorePlayer(award * 10);
                        JavaInterface.CoinOut1Limit(award);
                        //开始退币检查
                        GameController.isOutCoin = true;
                        Debug.Log("退出" + award + "个币");
                    }
                    else
                    {
                        Debug.Log("奖金不足无法退出");
                    }
                }
                if (profit >= award)
                {
                    profit -= award;
                    PlayerPrefs.SetInt("Profit", profit);
                    //Debug.Log("退出"+ award + "个币");
                }
                else
                {
                    Debug.Log("奖金不足无法退出");
                }
                PlayerPrefs.SetInt("AllScore", allScore);
                PlayerPrefs.SetInt("GameCounter", gameCounter);
                PlayerPrefs.Save();
                lastScoreText.Display(gameScore, 0.05f);
                bestText.Display(bestScore);

                mMainPanel.Award(awards);
                mMainPanel.Display();

                SetGameScore(0);
                awards = 0;
                Debug.Log("利润:"+profit+" 第"+gameCounter +"局" + averageScore);
            }
        }
        void SetGameScore(int value)
        {
            gameScore = value;
            gameText.Display(gameScore);
        }
        void AddGameScore(int value)
        {
            gameScore += value;
            gameText.Display(gameScore);
        }

        void OnReset()
        {
            if (mMainPanel != null) lastScoreText.Display(0, 0.1f);
            if (JavaInterface.Java() && playerText != null)
            {
                playerText.Display(JavaInterface.GetScorePlayer() / 10, 0.1f);
                scoreText.Display(JavaInterface.GetScorePlayer() % 10, 0.1f);
            }
        }
        void StartReset()
        {
            if (mWelcome != null) mWelcome.Hide();
        }
        void FinishReset()
        {
            //if (mWelcome != null) mWelcome.Display();
        }
        void OnHealth(PlayerInfo player)
        {
            if (player.index == 0)
            {
                mScroll.valueA = player.health / player.healthMax;
                mScroll.Refresh();
                if (player.health > player.healthLast)
                {
                    mScroll.Flash();
                    //Debug.Log("!!!");
                }
                player.healthLast = player.health;
            }
        }
        void OnLaunchGame()
        {
            if (JavaInterface.Java())
            {
                if (JavaInterface.CoinOutCheck())Debug.Log("玩家分数为零停止退币");

                JavaInterface.AddShuffleScore(10);
                JavaInterface.AddGameCounter(1);

            }
            profit += 1;
            PlayerPrefs.SetInt("Profit", profit);
            PlayerPrefs.Save();
            mScroll.gameObject.SetActive(true);
            mScrollMain.gameObject.SetActive(true);
            gameText.gameObject.SetActive(true);
            gameText.Display(gameScore);
            mScroll.valueA = PlayerController.players[0].health / PlayerController.players[0].healthMax;
            mScroll.Refresh();
            mScrollMain.valueA = 0;
            mScrollMain.Refresh();
            if (mWelcome != null)
            {
                mWelcome.Hide();
                mWelcome.gameObject.GetComponent<Collider>().enabled = false;
            }
            if (mChopLeft != null) mChopLeft.Display();
            if (mChopRight != null) mChopRight.Display();
        }
        void OnFinishGame()
        {
            if (mMainPanel != null) mMainPanel.Hide();
        }
        void OnStartGame()
        {
            if (mChopLeft != null) mChopLeft.Hide();
            if (mChopRight != null) mChopRight.Hide();
        }
        void OnWelcome()
        {
            if (mWelcome != null)
            {
                mWelcome.Display();
                mWelcome.gameObject.GetComponent<Collider>().enabled = true;
            }
        }
        void OffWelcome()
        {

        }
        void WaitStartGame(int value)
        {
            if (value > 0)
            {
                audio.clip = count;
            }
            else
            {
                audio.clip = count_go;
            }
            audio.Play();
            //Instantiate(texts[7]);
            StartCoroutine(DisplayText(value, 0.5f));
            //Debug.Log(value);
        }
        IEnumerator DisplayText(int value,float time)
        {
            CustomText text = Instantiate(texts[6]).GetComponent<CustomText>();
            text.Display(value);
            text.gameObject.transform.position = new Vector3(0, -0.25f);
            float current = 0;
            float alpha = 1;
            while (current < time)
            {
                text.gameObject.transform.localScale = new Vector3(0.1f, 0.1f) + alpha * new Vector3(0.1f, 0.1f);
                text.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
                alpha = 1f - current / time;
                current += 0.05f;
                yield return new WaitForSeconds(0.025f);
            }
            Destroy(text.gameObject);
        }
    }
}