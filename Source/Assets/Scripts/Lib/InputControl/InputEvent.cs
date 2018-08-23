using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MySpace.Event
{
    public class InputEvent
    {
        public delegate void VoidHandlerEvent();
        public delegate void IntHandlerEvent(int value);
        public delegate void FloatHandlerEvent(float value);
        public delegate void ObjHandlerEvent(GameObject obj);

        public static event ObjHandlerEvent onPress;
        public static event ObjHandlerEvent onFree;
        /// <summary>
        /// 点击开始和结束都是同一目标时触发
        /// </summary>
        public static event ObjHandlerEvent onClick;

        public static event ObjHandlerEvent onRightClickDown;
        public static event ObjHandlerEvent onRightClickUp;
        /// <summary>
        /// 点击开始和结束都是同一目标时触发
        /// </summary>
        public static event ObjHandlerEvent onRightClick;

        public static event ObjHandlerEvent onHover;
        public static event ObjHandlerEvent onLeave;
        public static event ObjHandlerEvent onDrag;

        public static event VoidHandlerEvent onMove;
        public static event VoidHandlerEvent onMoving;
        public static event VoidHandlerEvent onStop;

        //public static event IntHandlerEvent onKeyDown;
        //public static event IntHandlerEvent onKeyUp;
        //public static event IntHandlerEvent onKeyHold;

        public static void OnClick(GameObject obj) { if (onClick != null) onClick(obj); }
        public static void OnPress(GameObject obj) { if (onPress != null) onPress(obj); }
        public static void OnFree(GameObject obj) { if (onFree != null) onFree(obj); }

        public static void OnRightClick(GameObject obj) { if (onRightClick != null) onRightClick(obj); }
        public static void OnRightClickDown(GameObject obj) { if (onRightClickDown != null) onRightClickDown(obj); }
        public static void OnRightClickUp(GameObject obj) { if (onRightClickUp != null) onRightClickUp(obj); }

        public static void OnHover(GameObject obj) { if (onHover != null) onHover(obj); }
        public static void OnLeave(GameObject obj) { if (onLeave != null) onLeave(obj); }
        public static void OnDrag(GameObject obj) { if (onDrag != null) onDrag(obj); }
        public static void OnMove() { if (onMove != null) onMove(); }
        public static void OnStop() { if (onStop != null) onStop(); }
        public static void OnMoving() { if (onMoving != null) onMoving(); }

        //public static void OnKeyDown(int value){if (onKeyDown != null) onKeyDown(value);}
        //public static void OnKeyUp(int value) { if (onKeyUp != null) onKeyUp(value); }
        //public static void OnKeyHold(int value) { if (onKeyHold != null) onKeyHold(value); }

        /// <summary>
        /// 按键状态
        /// </summary>
        public class KeyStatus
        {
            public event FloatHandlerEvent onKeyHold;
            public event VoidHandlerEvent onKeyDown;
            public event VoidHandlerEvent onKeyUp;
            bool isHold = false;
            bool isDown = false;
            float holdTime = 0;
            float overTime = 0;
            /// <summary>
            /// 最少保持时间，超过该值才可判断保持按压(0.25秒)
            /// </summary>
            float lessTime = 0.25f;
            /// <summary>
            /// 按键保持事件触发间隔时间(默认0.1秒)
            /// </summary>
            float frameTime = 0.1f;
            public void KeyDownCall()
            {
                if (!isDown)
                {
                    isDown = true;
                    if (onKeyDown != null) onKeyDown();
                }
            }
            public void KeyUpCall()
            {
                holdTime = 0;
                overTime = 0;
                isHold = false;
                isDown = false;
                if (onKeyUp != null) onKeyUp();
            }
            public void KeyHoldCall()
            {
                holdTime += Time.deltaTime;
                if (!isHold)
                {
                    if (holdTime > lessTime)
                    {
                        isHold = true;
                    }
                }
                else
                {
                    overTime += Time.deltaTime;
                    if (overTime > frameTime)
                    {
                        overTime -= frameTime;
                        if (onKeyHold != null) onKeyHold(holdTime);
                    }
                }
            }
        }
        public static Dictionary<string, KeyStatus> dic = new Dictionary<string, KeyStatus>();
        /// <summary>
        /// 监听按键方法
        /// 因为不知道具体需要监听的按键
        /// 所以加入一个监听字典
        /// </summary>
        public static void ListenKeyDown(string value, VoidHandlerEvent call)
        {
            //临时变量
            KeyStatus status;
            if (!dic.ContainsKey(value))
            {
                dic.Add(value,new KeyStatus());
            }
            //尝试获取
            dic.TryGetValue(value, out status);
            status.onKeyDown += call;
        }
        public static void ListenKeyUp(string value, VoidHandlerEvent call)
        {
            //临时变量
            KeyStatus status;
            if (!dic.ContainsKey(value))
            {
                dic.Add(value, new KeyStatus());
            }
            //尝试获取
            dic.TryGetValue(value, out status);
            status.onKeyUp += call;
        }
        public static void ListenKeyHold(string value, FloatHandlerEvent call)
        {
            //临时变量
            KeyStatus status;
            if (!dic.ContainsKey(value))
            {
                dic.Add(value, new KeyStatus());
            }
            //尝试获取
            dic.TryGetValue(value, out status);
            status.onKeyHold += call;
        }
        /// <summary>
        /// 移除某个按键全部的监听方法
        /// </summary>
        public static bool RemoveKey(string value)
        {
            if (dic.ContainsKey(value))
            {
                //临时变量
                //KeyStatus status;
                //dic.TryGetValue(value, out status);
                return dic.Remove(value);
            }
            else
            {
                return false;
            }
        }
        ///// <summary> 
        ///// 移除所有的事件绑定 
        ///// </summary> 
        ///// <param name="clearEvent"></param> 
        //private static void clear_event(VoidHandlerEvent clearEvent)
        //{
        //    Delegate[] dels = clearEvent.GetInvocationList();
        //    foreach (Delegate d in dels)
        //    {
        //        //得到方法名   
        //        object delObj = d.GetType().GetProperty("Method").GetValue(d, null);
        //        string funcName = (string)delObj.GetType().GetProperty("Name").GetValue(delObj, null);
        //        clearEvent -= d as VoidHandlerEvent;
        //    }
        //}
    }
}