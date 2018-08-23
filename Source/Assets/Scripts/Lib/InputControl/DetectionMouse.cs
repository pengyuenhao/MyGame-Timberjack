using UnityEngine;
using System.Collections;
using System.Threading;
using MySpace.Event;

public static class DetectionMouse {
    public static float frame = 0.05f;
    public static bool isRunning = true;
    #region 持续状态记录
    public static Vector2 mousePostion { get { return postion; } }

    //private static Curve curve;

    public static GameObject hover;
    public static GameObject click;
    public static GameObject rightClick;
    public static Vector3 clickPostion;
    public static Vector3 rightClickPostion;

    public static bool isHover;
    public static bool isMove;
    public static bool isClick;
    public static bool isRightClick;

    private static float startHover = 0;
    public static float timeHover{get{return Time.time - startHover;}}
    private static float startMove = 0;
    public static float timeMove{get { return Time.time - startMove; }}
    private static float startClick = 0;
    public static float timeClick{get { return Time.time - startClick; }}
    private static float startRightClick = 0;
    public static float timeRightClick{get { return Time.time - startRightClick; }}
    #endregion
    #region 状态检测方法
    private static Vector2 postion;
    private static int overtimeMove;
    /// <summary>
    /// 悬停检测
    /// </summary>
    static bool HoverDectection()
    {
        GameObject hovering;
        hovering = DetectionRay.RayCastUI(Input.mousePosition);
        if (hovering == null)
        {
            hovering = DetectionRay.RayCastGame(Input.mousePosition);
            if (hovering == null)
            {
                hovering = DetectionRay.RayCastAll(Input.mousePosition);
            }
        }
        if (hover != hovering)
        {
            //Debug.Log(hovering);
            if (hover != null) InputEvent.OnLeave(hover);
            hover = hovering;
            startHover = Time.time;
            isHover = true;
            if(hover!=null)InputEvent.OnHover(hover);
            return true;
        }
        if (hover != null)
        {
            isHover = true;
        }
        else
        {
            isHover = false;
        }

        return false;
    }
    /// <summary>
    /// 运动检测
    /// </summary>
    static bool MoveDectection()
    {
        if ((Mathf.Abs(postion.x - Input.mousePosition.x)) >= 1f ||
            (Mathf.Abs(postion.y - Input.mousePosition.y)) >= 1f)
        {
            if (click != null) InputEvent.OnDrag(click);
            postion = Input.mousePosition;
            InputEvent.OnMoving();
            if (isMove == false)
            {
                isMove = true;
                overtimeMove = 3;
                startMove = Time.time;
                InputEvent.OnMove();
            }
            else
            {
                overtimeMove = 3;
            }
        }
        else
        {
            if (isMove == true)
            {
                if (overtimeMove < 0)
                {
                    isMove = false;
                    startMove = Time.time;
                    InputEvent.OnStop();
                    return true;
                }
                else
                {
                    overtimeMove -= 1;
                }
            }
        }
        return false;
    }
    #endregion
    public static void DectectionHandler()
    {
        HoverDectection();
        MoveDectection();
    }
    public static void GetStatus()
    {
        if (isRunning != true) return;
        if(
            Input.GetMouseButtonDown(0)|| 
            Input.GetMouseButtonDown(1)||
            Input.GetMouseButtonUp(0)||
            Input.GetMouseButtonUp(1)
            )
        {
            DectectionHandler();
        }
        else
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isClick = true;
            startClick = Time.time;
            if (hover == null)
            {
                clickPostion = DetectionInput.worldPostion;
                InputEvent.OnPress(null);
            }
            else
            {
                click = hover;
                InputEvent.OnPress(click);
            }
        }
        //鼠标左键松开
        if (Input.GetMouseButtonUp(0))
        {
            if (hover != null && click == hover)
            {
                InputEvent.OnClick(click);
            }
            else
            {
                if (Vector3.Distance(clickPostion, DetectionInput.worldPostion) < 10f && timeClick < 0.5f)
                    InputEvent.OnClick(null);
            }
            if(click!=null)InputEvent.OnFree(click);
            isClick = false;
            startClick = Time.time;
            click = null;
        }
        //鼠标右键按下
        if (Input.GetMouseButtonDown(1))
        {
            isRightClick = true;
            startRightClick = Time.time;
            if (hover == null)
            {
                rightClickPostion = DetectionInput.worldPostion;
                InputEvent.OnRightClickDown(null);
            }
            else
            {
                rightClick = hover;
                InputEvent.OnRightClickDown(rightClick);
            }
        }
        //鼠标右键松开
        if (Input.GetMouseButtonUp(1))
        {
            if (hover != null && rightClick == hover)
            {
                InputEvent.OnRightClick(rightClick);
            }
            else
            {
                if (Vector3.Distance(rightClickPostion, DetectionInput.worldPostion) < 10f && timeRightClick < 0.5f)
                    InputEvent.OnRightClick(null);
            }
            if (hover == null)
                InputEvent.OnRightClickUp(null);
            else
                InputEvent.OnRightClickUp(hover);
            isRightClick = false;
            startRightClick = Time.time;
            rightClick = null;
        }
    }
}
