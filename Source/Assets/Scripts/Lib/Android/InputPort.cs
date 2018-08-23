using UnityEngine;
using System.Collections;

public class InputPort :MonoBehaviour {
    public static InputPort port;
    void Awake()
    {
        if (port == null) port = this;
    }
    void Start()
    {
        Check();
    }
    public bool Check()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.Log("这是安卓设备");
            StartInput();
            return true;
        }
        else
        {
            //Debug.LogError("安卓端口无法再非安卓设备上运行");
            enabled = false;
            //this.enabled = false;
            return false;
        }
    }
    public delegate void VoidEventHandler();
    public delegate void BoolEventHandler(bool b);
    public delegate void IntEventHandler(int i);

    int[] hold = new int[32];
    public static VoidEventHandler[] onKeyDownEventList = new VoidEventHandler[32];
    public static VoidEventHandler[] onKeyUpEventList = new VoidEventHandler[32];
    public static VoidEventHandler[] onKeyHoldEventList = new VoidEventHandler[32];
    public static IntEventHandler[] onKeyHoldTimerEventList = new IntEventHandler[32];

    public static event VoidEventHandler onKeyDownEvent;
    public static event VoidEventHandler onKeyUpEvent;

    public event VoidEventHandler onKeyUp0Event;
    public event VoidEventHandler onKeyDown0Event;
    public event VoidEventHandler onKeyUp1Event;
    public event VoidEventHandler onKeyDown1Event;
    public event VoidEventHandler onKeyUp2Event;
    public event VoidEventHandler onKeyDown2Event;
    public event VoidEventHandler onKeyUp3Event;
    public event VoidEventHandler onKeyDown3Event;

    public static event IntEventHandler onChangeScoreEvent;

    public event VoidEventHandler onChangeInCoin1Event;
    public event VoidEventHandler onChangeOutCoin1Event;
    public event VoidEventHandler onChangeInCoin2Event;
    public event VoidEventHandler onChangeOutCoin2Event;

    public event VoidEventHandler onKeyInCoinUpEvent;
    public event VoidEventHandler onKeyOutCoinUpEvent;
    public event VoidEventHandler onKeyStartUpEvent;
    public event VoidEventHandler onKeyQuitUpEvent;

    public event VoidEventHandler onKeyInCoinDownEvent;
    public event VoidEventHandler onKeyOutCoinDownEvent;
    public event VoidEventHandler onKeyStartDownEvent;
    public event VoidEventHandler onKeyQuitDownEvent;

    void OnKeyUpEvent() { if (onKeyUp0Event != null) onKeyUp0Event(); }
    void OnKeyDownEvent() { if (onKeyUp0Event != null) onKeyUp0Event(); }

    void OnKeyDownEvent(int id)
    {
        if (onKeyDownEventList[id] != null)  onKeyDownEventList[id]();
        hold[id] = 1;
    }
    void OnKeyUpEvent(int id)
    {
        if (onKeyUpEventList[id] != null)onKeyUpEventList[id]();
        hold[id] = 0;
    }
    /// <summary>
    /// 每秒钟检测四十并返回持续的时间
    /// </summary>
    void OnKeyHoldTimerEvent(int id)
    {
        if (onKeyHoldTimerEventList[id] != null) onKeyHoldTimerEventList[id](hold[id]);
    }
    /// <summary>
    /// 智能持续按键检测
    /// </summary>
    void OnKeyHoldEvent(int id)
    {
        if (onKeyHoldEventList[id] != null) onKeyHoldEventList[id]();
    }


    void OnChangeScoreEvent(int i) { if (onChangeScoreEvent != null) onChangeScoreEvent(i); }

    void OnChangeInCoin1Event() { if (onChangeInCoin1Event != null) onChangeInCoin1Event(); }
    void OnChangeOutCoin1Event() { if (onChangeOutCoin1Event != null) onChangeOutCoin1Event(); }
    void OnChangeInCoin2Event() { if (onChangeInCoin2Event != null) onChangeInCoin2Event(); }
    void OnChangeOutCoin2Event() { if (onChangeOutCoin2Event != null) onChangeOutCoin2Event(); }

    void OnKeyUp0Event() { if (onKeyUp0Event != null) onKeyUp0Event(); }
    void OnKeyUp1Event() { if (onKeyUp1Event != null) onKeyUp1Event(); }
    void OnKeyUp2Event() { if (onKeyUp2Event != null) onKeyUp2Event(); }
    void OnKeyUp3Event() { if (onKeyUp3Event != null) onKeyUp3Event(); }

    void OnKeyDown0Event() { if (onKeyDown0Event != null) onKeyDown0Event(); }
    void OnKeyDown1Event() { if (onKeyDown1Event != null) onKeyDown1Event(); }
    void OnKeyDown2Event() { if (onKeyDown2Event != null) onKeyDown2Event(); }
    void OnKeyDown3Event() { if (onKeyDown3Event != null) onKeyDown3Event(); }

    void OnKeyInCoinUpEvent() { if (onKeyInCoinUpEvent != null) onKeyInCoinUpEvent(); }
    void OnKeyInCoinDownEvent() { if (onKeyInCoinDownEvent != null) onKeyInCoinDownEvent(); }
    void OnKeyOutCoinUpEvent() { if (onKeyOutCoinUpEvent != null) onKeyOutCoinUpEvent(); }
    void OnKeyOutCoinDownEvent() { if (onKeyOutCoinDownEvent != null) onKeyOutCoinDownEvent(); }

    void OnKeyStartUpEvent() { if (onKeyStartUpEvent != null) onKeyStartUpEvent(); }
    void OnKeyStartDownEvent() { if (onKeyStartDownEvent != null) onKeyStartDownEvent(); }
    void OnKeyQuitUpEvent() { if (onKeyQuitUpEvent != null) onKeyQuitUpEvent(); }
    void OnKeyQuitDownEvent() { if (onKeyQuitDownEvent != null) onKeyQuitDownEvent(); }

    private void StartInput()
    {
        //Debug.Log("Start Key Scan");
        //StartCoroutine(GetKeyEventList(0.05f));
        StartCoroutine(GetCoinEventList(0.1f));
    }

    bool []KeyEventNew = new bool[32];
    bool []KeyEventOld = new bool[32];
    void OnKeyEvent(int id)
    {
        if (JavaInterface.getKey(id) == true)
        {
            KeyEventNew[id] = true;
            //检查按键是否被持续按住
            if (hold[id] != 0)
            {
                hold[id] += 1;
                OnKeyHoldTimerEvent(id);
                //if (hold[id] > 10 && hold[id] % 3 == 0) OnKeyHoldEvent(id);
                if (hold[id] > 3) OnKeyHoldEvent(id);
            }
        }
        else KeyEventNew[id] = false;
        //按键被按下时触发事件
        if ((KeyEventNew[id] == true) && (KeyEventOld[id] == false))
        {
            //Debug.Log("key " + id + " is click!!!");
            OnKeyDownEvent();
            OnKeyDownEvent(id);
            switch (id)
            {
                case 0: OnKeyDown0Event(); break;
                case 1: OnKeyDown1Event(); break;
                case 2: OnKeyDown2Event(); break;
                case 3: OnKeyDown3Event(); break;
                default: break;
            }
        }
        //按键被释放时触发事件
        if((KeyEventNew[id] == false) && (KeyEventOld[id] == true))
        {
            OnKeyUpEvent();
            OnKeyUpEvent(id);
            switch (id)
            {
                case 0: OnKeyUp0Event(); break;
                case 1: OnKeyUp1Event(); break;
                case 2: OnKeyUp2Event(); break;
                case 3: OnKeyUp3Event(); break;
                default: break;
            }
        }
        KeyEventOld[id] = KeyEventNew[id];
    }
    IEnumerator GetKeyEventList(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        while (true)
        {
            for (int i = 0; i < 32; i++) OnKeyEvent(i);
            yield return wait;
        }
    }
    int keyCounter = 0;
    //每一帧检测一次按键
    void Update()
    {
        for (int i = 0; i < 32; i++)
        {
            OnKeyEvent(i);
        }
        //if (keyCounter >= 32)keyCounter = 0;
        ////每帧检测16条
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
        //OnKeyEvent(keyCounter++);
    }
    IEnumerator GetCoinEventList(float time)
    {
        int newInCoin1 = JavaInterface.getInCoin1();
        int newInCoin2 = JavaInterface.getInCoin2();
        int newOutCoin1 = JavaInterface.getOutCoin1();
        int newOutCoin2 = JavaInterface.getOutCoin2();

        int newScorePlayer = JavaInterface.GetScorePlayer();

        int oldInCoin1 = newInCoin1;
        int oldInCoin2 = newInCoin2;
        int oldOutCoin1 = newOutCoin1;
        int oldOutCoin2 = newOutCoin2;
        int oldScorePlayer = newScorePlayer;
        while (true)
        {
            newInCoin1 = JavaInterface.getInCoin1();
            newInCoin2 = JavaInterface.getInCoin2();
            newOutCoin1 = JavaInterface.getOutCoin1();
            newOutCoin2 = JavaInterface.getOutCoin2();
            
            newScorePlayer = JavaInterface.GetScorePlayer();

            if (newScorePlayer != oldScorePlayer) { oldScorePlayer = newScorePlayer; OnChangeScoreEvent(newScorePlayer); }

            if (newInCoin1 != oldInCoin1) { oldInCoin1 = newInCoin1; OnChangeInCoin1Event(); }
            if (newInCoin2 != oldInCoin2) { oldInCoin2 = newInCoin2; OnChangeInCoin2Event(); }
            if (newOutCoin1 != oldOutCoin1) { oldOutCoin1 = newOutCoin1; OnChangeOutCoin1Event(); }
            if (newOutCoin2 != oldOutCoin2) { oldOutCoin2 = newOutCoin2; OnChangeOutCoin2Event(); }
            yield return new WaitForSeconds(time);
        }
    }
}
