using UnityEngine;
using System.Collections;
using MySpace.Event;
using System.Collections.Generic;

public static class DetectionTouch {
    /// <summary>
    /// 触摸判断范围
    /// </summary>
    public static float touchRange = 10;
    /// <summary>
    /// 误差判断范围
    /// </summary>
    public static float diffRange = 200;
    private static Vector2 postion;
    public static GameObject click;
    public static Vector2 touchPostion { get { return postion; } }
    public static bool isRunning = true;
    public static bool isClick;
    public static List<TouchInfo> TouchList = new List<TouchInfo>();

    //新建的触摸点列表
    static List<Vector2> newTouchList = new List<Vector2>();
    //释放的触摸点列表
    static List<TouchInfo> freeTouchList = new List<TouchInfo>(); 

    public static int lastTouchCount;


    public class TouchInfo
    {
        public Vector2 currentPostion;
        public Vector2 beginPostion;
        public Vector2 endPostion;
        public GameObject hovering;
        public bool isPress;
    }
    public static void GetStatus()
    {

        //如果正在被触摸
        if (Input.touchCount > 0)
        {
            #region 检查触摸列表
            #region 重置触摸状态
            Vector2 touchPostion;
            bool isExist;
            int index;
            //Debug.Log("触控列表计数:" + TouchList.Count);
            //设置触控列表内所有的点为非按压状态
            for (int i = 0; i < TouchList.Count; i++)
            {
                TouchList[i].isPress = false;
                //Debug.Log("触控点列表:"+i + "," + TouchList[i].currentPostion);
            }
            #endregion
            #region 检查触摸状态
            //检查每个触摸点
            for (int i = 0; i < Input.touchCount; i++)
            {
                touchPostion = Input.touches[i].position;
                isExist = false;
                index = 0;
                //从触摸列表中查询是否有该触摸点的记录
                for (int j = 0; j < TouchList.Count; j++)
                {
                    //触摸点距离在判断范围以内时认为其存在
                    if (Vector2.Distance(touchPostion, TouchList[j].currentPostion) < touchRange)
                    {
                        //触摸点存在于列表中
                        isExist = true;
                        index = j;
                        break;
                    }
                }
                if(isExist == true)
                {
                    //更新触摸列表
                    TouchList[index].currentPostion = touchPostion;
                    TouchList[index].isPress = true;
                }
                else
                {
                    //Debug.Log("不存在于触控列表内的点:"+touchPostion);
                    //foreach(TouchInfo info in TouchList)
                    //{
                    //    Debug.Log("当前列表数据:" + info.currentPostion);
                    //}
                    //Debug.Log("以上");
                    //加入新建触摸点列表进行缓存
                    newTouchList.Add(touchPostion);
                }
            }
            #endregion
            #region 释放触摸列表
            //判断触控列表内是否有点被释放
            for (int i = 0; i < TouchList.Count; i++)
            {
                if (TouchList[i].isPress == false)
                {
                    //如果只有一个新的点触摸点并且触摸点数量和之前一致则有可能是滑动过快导致的误触发
                    if (newTouchList.Count == 1 && Input.touchCount == lastTouchCount)
                    {
                        //如果间距小于判断值则认为是误触发
                        if (Vector2.Distance(TouchList[i].currentPostion, newTouchList[0]) < diffRange)
                        {
                            TouchList[i].isPress = true;
                            TouchList[i].currentPostion = newTouchList[0];
                            newTouchList.Clear();
                            continue;
                        }
                    }
                    //当触控点被释放时
                    if (TouchList[i].hovering != null)
                    {
                        InputEvent.OnFree(TouchList[i].hovering);
                        GameObject hovering = DetectionRay.RayCast(TouchList[i].currentPostion);
                        if (hovering != null && hovering == TouchList[i].hovering)
                        {
                            InputEvent.OnClick(hovering);
                        }
                    }
                    else
                    {
                        InputEvent.OnFree(null);
                    }
                    //Debug.Log("释放"+ TouchList[i].currentPostion);
                    //移除触摸点
                    TouchList.Remove(TouchList[i]);
                }
            }
            #endregion
            #endregion
            #region 创建新触摸点
            for (int i = 0; i < newTouchList.Count; i++)
            {
                //Debug.Log("新建触控:" + i + "," + newTouchList[i]);
                //加入触摸列表
                TouchInfo touchInfo = new TouchInfo();
                touchInfo.beginPostion = newTouchList[i];
                touchInfo.currentPostion = newTouchList[i];
                touchInfo.isPress = true;

                //检查是否悬浮于某物体
                touchInfo.hovering = DetectionRay.RayCast(newTouchList[i]);
                if (touchInfo.hovering != null)
                {
                    InputEvent.OnPress(touchInfo.hovering);
                }
                else
                {
                    InputEvent.OnPress(null);
                }
                //添加触摸列表
                TouchList.Add(touchInfo);
            }
            newTouchList.Clear();
            #endregion
            #region 触摸状态更新
            //第一个触摸位置
            if (TouchList.Count >0)click = TouchList[0].hovering;
            postion = Input.touches[0].position;
            if (isClick == false)
            {
                isClick = true;
            }
            //存储本次触控点计数
            lastTouchCount = Input.touchCount;
            #endregion
        }
        else
        {
            //Debug.Log("释放所有触摸点");
            #region 释放触控列表
            if (isClick == true)
            {
                isClick = false;
            }
            for(int i=0;i< TouchList.Count; i++)
            {
                if (TouchList[i].hovering != null)
                {
                    InputEvent.OnFree(TouchList[i].hovering);
                    GameObject hovering = DetectionRay.RayCast(TouchList[i].currentPostion);
                    if (hovering != null && hovering == TouchList[i].hovering)
                    {
                        InputEvent.OnClick(hovering);
                    }
                }
                else
                {
                    InputEvent.OnFree(null);
                }
            }
            TouchList.Clear();
            newTouchList.Clear();
            #endregion
        }
    }
}
