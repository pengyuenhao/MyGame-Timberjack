using UnityEngine;
using System.Collections;
using MySpace.Event;

public class DetectionInput : MonoBehaviour
{
    #region 变量设置
#if UNITY_EDITOR
    #region 获取分辨率
    static bool Editor__getGameViewSizeError = false;
    public static bool Editor__gameViewReflectionError = false;

    // 尝试获取 GameView 的分辨率  
    // 当正确获取到 GameView 的分辨率时，返回 true  
    static float Editor_GetGameAspect()
    {
        float width;
        float height;
        float aspect;
        Editor__GetGameViewSize(out width, out height, out aspect);
        if (aspect == 0)
        {
            aspect = Camera.main.pixelWidth / Camera.main.pixelHeight;
        }
        return aspect;
    }
    static bool Editor__GetGameViewSize(out float width, out float height, out float aspect)
    {
        try
        {
            Editor__gameViewReflectionError = false;

            System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetMainGameView = gameViewType.GetMethod("GetMainGameView", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            object mainGameViewInst = GetMainGameView.Invoke(null, null);
            if (mainGameViewInst == null)
            {
                width = height = aspect = 0;
                return false;
            }
            System.Reflection.FieldInfo s_viewModeResolutions = gameViewType.GetField("s_viewModeResolutions", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            if (s_viewModeResolutions == null)
            {
                System.Reflection.PropertyInfo currentGameViewSize = gameViewType.GetProperty("currentGameViewSize", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                object gameViewSize = currentGameViewSize.GetValue(mainGameViewInst, null);
                System.Type gameViewSizeType = gameViewSize.GetType();
                int gvWidth = (int)gameViewSizeType.GetProperty("width").GetValue(gameViewSize, null);
                int gvHeight = (int)gameViewSizeType.GetProperty("height").GetValue(gameViewSize, null);
                int gvSizeType = (int)gameViewSizeType.GetProperty("sizeType").GetValue(gameViewSize, null);
                //Debug.Log(gvWidth / gvHeight);
                if (gvWidth == 0 || gvHeight == 0)
                {
                    width = height = aspect = 0;
                    return false;
                }
                else if (gvSizeType == 0)
                {
                    width = height = 0;
                    aspect = (float)gvWidth / (float)gvHeight;
                    return true;
                }
                else
                {
                    width = gvWidth; height = gvHeight;
                    aspect = (float)gvWidth / (float)gvHeight;
                    return true;
                }
            }
            else
            {
                Vector2[] viewModeResolutions = (Vector2[])s_viewModeResolutions.GetValue(null);
                float[] viewModeAspects = (float[])gameViewType.GetField("s_viewModeAspects", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
                string[] viewModeStrings = (string[])gameViewType.GetField("s_viewModeAspectStrings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
                if (mainGameViewInst != null
                    && viewModeStrings != null
                    && viewModeResolutions != null && viewModeAspects != null)
                {
                    int aspectRatio = (int)gameViewType.GetField("m_AspectRatio", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(mainGameViewInst);
                    string thisViewModeString = viewModeStrings[aspectRatio];
                    if (thisViewModeString.Contains("Standalone"))
                    {
                        width = UnityEditor.PlayerSettings.defaultScreenWidth; height = UnityEditor.PlayerSettings.defaultScreenHeight;
                        aspect = width / height;
                    }
                    else if (thisViewModeString.Contains("Web"))
                    {
                        width = UnityEditor.PlayerSettings.defaultWebScreenWidth; height = UnityEditor.PlayerSettings.defaultWebScreenHeight;
                        aspect = width / height;
                    }
                    else
                    {
                        width = viewModeResolutions[aspectRatio].x; height = viewModeResolutions[aspectRatio].y;
                        aspect = viewModeAspects[aspectRatio];
                        // this is an error state  
                        if (width == 0 && height == 0 && aspect == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            if (Editor__getGameViewSizeError == false)
            {
                Debug.LogError("GameCamera.GetGameViewSize - has a Unity update broken this?\nThis is not a fatal error !\n" + e.ToString());
                Editor__getGameViewSizeError = true;
            }
            Editor__gameViewReflectionError = true;
        }
        width = height = aspect = 0;
        return false;
    }
    #endregion
    public static float aspect { get { return Editor_GetGameAspect(); } }
    public static float wigth { get { return height * aspect; } }
    public static float height { get { return Mathf.Abs(Camera.main.transform.position.z) * Mathf.Tan(Mathf.PI * Camera.main.fieldOfView / 360); } }
#else
    public static float aspect { get { return (float)Screen.width / (float)Screen.height; } }
    public static float wigth { get { return height * aspect; } }
    public static float height { get { return Mathf.Abs(Camera.main.transform.position.z) * Mathf.Tan(Mathf.PI * Camera.main.fieldOfView / 360); } }
#endif
    public static Vector3 worldPostion
    {
        get
        {
            return Camera.main.transform.position + new Vector3(wigth * screenPostion.x, height * screenPostion.y, -Camera.main.transform.position.z);
        }
    }
    //public static float screenWidth { get { } }
    //返回鼠标位于屏幕的位置，屏幕的中心点为0,范围是-1到1
    public static Vector2 screenPostion
    {
        get
        {
            return new Vector2(2 * centerPostion.x / Screen.width, 2 * centerPostion.y / Screen.height);
        }
    }
    public static Vector2 centerPostion
    {
        get
        {
            return postion - 0.5f * new Vector2(Screen.width,Screen.height);
        }
    }
    public static Vector2 postion {
        get
        {
            if (DetectionMouse.isRunning)
            {
                return DetectionMouse.mousePostion;
            }
            else
            {
                return DetectionTouch.touchPostion;
            }
        }
    }
    public static bool isRunning =true;
    public bool isMove;
    public float timeMove;
    public bool isHover;
    public float timeHover;
    public bool isClick;
    public float timeClick;
    public bool isRightClick;
    public float timeRightClick;
    public GameObject click;
    public GameObject rightClick;
    public GameObject hover;
    #endregion
    void Start()
    {
        StartCoroutine(Detection());
    }
    void RegisterEvent()
    {
        InputEvent.onClick += OnClick;
        InputEvent.onPress += OnPress;
        InputEvent.onFree += OnFree;
        InputEvent.onRightClick += OnRightClick;
        InputEvent.onHover += OnHover;
        InputEvent.onMove += OnMove;
        InputEvent.onStop += OnStop;
        InputEvent.onLeave += OnLeave;
        InputEvent.onDrag += OnDrag;

        //InputEvent.onKeyDown += OnKeyDown;
    }
    void Update()
    {
        if (Input.mousePresent == true)
        {
            DetectionMouse.GetStatus();
        }
        else
        {
            DetectionTouch.GetStatus();
        }
        if (InputEvent.dic.Count != 0)
        {
            foreach (var item in InputEvent.dic)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    item.Value.KeyDownCall();
                }
                if (Input.GetKey(item.Key))
                {
                    item.Value.KeyHoldCall();
                }
                if (Input.GetKeyUp(item.Key))
                {
                    item.Value.KeyUpCall();
                }

            }
        }
    }
    IEnumerator Detection()
    {
        yield return new WaitForEndOfFrame();
        RegisterEvent();
        while (true) {
            while (isRunning) {
                if (Input.mousePresent != true)
                {
                    DetectionMouse.isRunning = false;
                }
                else
                {
                    DetectionTouch.isRunning = false;
                }
                if (DetectionMouse.isRunning)
                {
                    DetectionMouse.DectectionHandler();
                    isMove = DetectionMouse.isMove;
                    timeMove = DetectionMouse.timeMove;
                    isHover = DetectionMouse.isHover;
                    timeHover = DetectionMouse.timeHover;
                    isClick = DetectionMouse.isClick;
                    timeClick = DetectionMouse.timeClick;
                    isRightClick = DetectionMouse.isRightClick;
                    timeRightClick = DetectionMouse.timeRightClick;

                    rightClick = DetectionMouse.rightClick;
                    click = DetectionMouse.click;
                    hover = DetectionMouse.hover;
                }
                else
                {

                }
                yield return new WaitForSeconds(DetectionMouse.frame);
            }
            Debug.Log("暂停");
            yield return new WaitForSeconds(1);
        }
    }
    void OnClick(GameObject obj)
    {
        if (obj != null)EventListener.Get(obj).OnClick();
    }
    void OnPress(GameObject obj)
    {
        if (obj != null)EventListener.Get(obj).OnPress();
    }
    void OnFree(GameObject obj)
    {
        if (obj != null)EventListener.Get(obj).OnFree();
    }
    void OnRightClick(GameObject obj)
    {
        if(obj!=null)EventListener.Get(obj).OnRightClick();
    }
    void OnHover(GameObject obj)
    {
        if (obj != null)EventListener.Get(obj).OnHover();
    }
    void OnLeave(GameObject obj)
    {
        if (obj != null) EventListener.Get(obj).OnLeave();
    }
    void OnMove()
    {

    }
    void OnStop()
    {

    }
    void OnDrag(GameObject obj)
    {
        if (obj != null) EventListener.Get(obj).OnDrag();
    }
    void OnKeyDown(int value)
    { 

    }
}
