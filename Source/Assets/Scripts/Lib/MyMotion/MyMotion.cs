using UnityEngine;
using System.Collections;
namespace MySpace.Motion
{
    public class MyMotion : MonoBehaviour
    {
        public const float WaitFrame = 0.0125f;
        public delegate void CallHandler();
        private event CallHandler onFinishEvent;
        private event CallHandler onBlendEvent;
        private int listenerCounter;

        public bool isSmooth = false;
        public Modes mode;
        public enum Modes
        {
            /// <summary>
            /// 绝对坐标模式
            /// </summary>
            Absolute = 0,
            /// <summary>
            /// 相对坐标模式
            /// </summary>
            Relative = 1
        }
        public float blendTime;
        public float progress;
        private float maxBlendTime;
        public Vector3 targetPostion;
        public Vector3 targetScale;
        //public Vector3 targetRotate;
        public Quaternion targetRotate;
        public Vector3 gravity;

        //偏移值记录
        public Vector3 offsetPostion = new Vector3();
        public Vector3 offsetScale = new Vector3();
        public Quaternion offsetRotate = new Quaternion();
        Vector3 lastOffsetPostion = new Vector3();
        Vector3 lastOffsetScale = new Vector3();
        Quaternion lastOffsetRotate = new Quaternion();

        public Vector3 anchor;
        Vector3 currentPostion;
        Vector3 currentScale;
        Quaternion currentRotate;

        bool isMotion = false;
        bool isPostion = false;
        bool isScale = false;
        bool isRotate = false;
        bool isGravity = false;
        public bool isActive = false;
        void OnEnable()
        {
            isActive = true;
            if (isMotion)
            {
                OnFinish();
                OnClear();
            }
        }
        void OnDisable()
        {
            isActive = false;
            if (isMotion)
            {
                OnFinish();
                OnClear();
            }
        }
        public bool IsMotion { get { return isMotion; } }
        void OnFinishEvent()
        {
            if (onFinishEvent != null)
            {
                onFinishEvent();
                //执行完成之后全部删除
                //onFinishEvent = null;
            }
        }
        void OnBlendEvent()
        {
            if (onBlendEvent != null)
            {
                onBlendEvent();
            }
        }
        public MyMotion AddOnBlendListener(CallHandler listener)
        {
            onBlendEvent += listener;
            return this;
        }
        public MyMotion AddOnFinishListener(CallHandler listener)
        {
            onFinishEvent += listener;
            listenerCounter++;
            return this;
        }
        public MyMotion RemoveFinishListener(CallHandler listener)
        {
            onFinishEvent -= listener;
            listenerCounter--;
            return this;
        }
        #region 参数传递
        public MyMotion Mode(Modes value)
        {
            mode = value;
            return this;
        }
        public MyMotion Gravity(Vector3 value)
        {
            gravity = value;
            isGravity = true;
            return this;
        }
        public MyMotion Anchor(Vector3 value)
        {
            //Debug.Log("目标点:" + value);
            anchor = value;
            return this;
        }
        public MyMotion Postion(Vector3 value)
        {
            //Debug.Log("目标点:" + value);
            targetPostion = value;
            isPostion = true;
            return this;
        }
        public MyMotion Smooth(bool value)
        {
            isSmooth = value;
            return this;
        }
        public MyMotion Scale(Vector3 value)
        {
            targetScale = value;
            isScale = true;
            return this;
        }
        public MyMotion Rotate(Quaternion value)
        {
            targetRotate = value;
            isRotate = true;
            return this;
        }
        public MyMotion Rotate(Vector3 value)
        {
            //Debug.Log("角度值:" + value);
            targetRotate = Quaternion.Euler(value);
            isRotate = true;
            return this;
        }
        public MyMotion BlendTime(float value)
        {
            currentPostion = gameObject.transform.position;
            currentScale = gameObject.transform.localScale;
            currentRotate = gameObject.transform.rotation;
            blendTime = value;
            maxBlendTime = value;
            return this;
        }
        public MyMotion Run()
        {
            if (isMotion == false)
            {
                isMotion = true;
                if (isActive)
                {
                    StartCoroutine(OnMotion());
                }
            }
            return this;
        }
        #endregion
        IEnumerator OnMotion()
        {
            WaitForSeconds wait = new WaitForSeconds(WaitFrame);
            while (true)
            {
                if (blendTime > 0)
                {
                    blendTime -= WaitFrame;
                    if (blendTime < 0) blendTime = 0;
                    Blend(blendTime);
                }
                else
                {
                    break;
                }
                yield return wait;
            }
            //yield return new WaitForSeconds(WaitFrame);
            OnFinish();
            OnClear();
        }
        /// <summary>
        /// 如果正在执行则等待任务完成并执行新任务可以选择终止之前的任务
        /// </summary>
        public void WaitFinish(CallHandler onFinish, bool isStop = false)
        {
            if (isStop)
            {
                //跳过所有等待直接结算
                OnFinish();
                //执行任务
                onFinish();
            }
            else
            {
                if (isMotion)
                {
                    //加入等待序列等待任务执行完毕之后才执行
                    AddOnFinishListener(onFinish);
                }
                else
                {
                    onFinish();
                }
            }
        }
        public void WaitReset(float time = 0.1f)
        {
            OnClear();
            Get(gameObject).WaitFinish(() =>
            {
                Get(gameObject)
                .Mode(Modes.Relative)
                .Postion(-Get(gameObject).offsetPostion)
                .Scale(-Get(gameObject).offsetScale)
                .BlendTime(time)
                .Run();
            }, true);
        }
        void Blend(float time)
        {
            if (isGravity)
            {
                targetPostion -= gravity;
            }
            //绝对运动模式
            if (mode == Modes.Absolute)
            {
                //currentPostion = gameObject.transform.position;
                //currentScale = gameObject.transform.localScale;
                //currentRotate = gameObject.transform.rotation;
                #region 错误的实现方式
                //Vector3 veloPostion = new Vector3();
                //Vector3 veloScale = new Vector3();
                //Vector3 veloRotate = new Vector3();

                //if (isPostion) Vector3.SmoothDamp(currentPostion, targetPostion, ref veloPostion, time);
                //if (isScale) Vector3.SmoothDamp(currentScale, targetScale, ref veloScale, time);
                //if (isRotate) Vector3.SmoothDamp(currentRotate, targetRotate, ref veloRotate, time);

                //currentPostion += veloPostion;
                //currentScale += veloScale;
                //currentRotate += veloRotate;

                //gameObject.transform.position = currentPostion;
                //gameObject.transform.localScale = currentScale;
                //gameObject.transform.rotation = currentRotate;
                #endregion
                //if (isPostion) gameObject.transform.position = Vector3.Slerp(currentPostion, targetPostion, (1 - time / maxBlendTime));
                if (isPostion) gameObject.transform.position = Slerp(currentPostion, targetPostion, (1 - time / maxBlendTime));
                if (isScale) gameObject.transform.localScale = Slerp(currentScale, targetScale, (1 - time / maxBlendTime));
                if (isRotate) gameObject.transform.rotation = Quaternion.Slerp(currentRotate, targetRotate, (1 - time / maxBlendTime));
                //Debug.Log((1 - time / maxBlendTime));
            }
            //相对运动模式
            else if (mode == Modes.Relative)
            {
                #region Postion
                if (isPostion)
                {
                    Vector3 valuePostion;
                    float blend;
                    //还没到达最终混合时间前
                    if (time != 0)
                    {
                        if (isSmooth)
                        {
                            blend = 1f - time / maxBlendTime;
                            blend = (6F * blend - 6F * blend * blend);
                            //Debug.Log(gameObject + "," + blend);
                        }
                        else
                        {
                            blend = 1;
                        }

                        //线性逼近目标值
                        valuePostion = blend * (targetPostion / (maxBlendTime / WaitFrame));
                        //记录当前偏移值
                        offsetPostion += valuePostion;
                        gameObject.transform.position += valuePostion;
                    }
                }
                #endregion
                #region Scale
                if (isScale)
                {
                    Vector3 valueScale;
                    float blend;
                    //还没到达最终混合时间前
                    if (time != 0)
                    {
                        if (isSmooth)
                        {
                            blend = 1f - time / maxBlendTime;
                            blend = (6F * blend - 6F * blend * blend);
                            //Debug.Log(gameObject + "," + blend);
                        }
                        else
                        {
                            blend = 1;
                        }

                        //线性逼近目标值
                        valueScale = blend * (targetScale / (maxBlendTime / WaitFrame));
                        //记录当前偏移值
                        offsetScale += valueScale;
                        gameObject.transform.localScale += valueScale;
                    }
                }
            }
            #endregion
                #region Rotate
                if (isRotate)
                {
                    Quaternion valueRotate;
                    float blend;
                    //还没到达最终混合时间前
                    if (time != 0)
                    {
                        if (isSmooth)
                        {
                            blend = 1f - time / maxBlendTime;
                            blend = (6F * blend - 6F * blend * blend);
                            //Debug.Log(gameObject + "," + blend);
                        }
                        else
                        {
                            blend = 1;
                        }

                        //线性逼近目标值
                        valueRotate = Quaternion.Euler(blend * (targetRotate.eulerAngles / (maxBlendTime / WaitFrame)));
                        //记录当前偏移值
                        offsetRotate = Quaternion.Euler(offsetRotate.eulerAngles + valueRotate.eulerAngles);
                        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + valueRotate.eulerAngles);
                    }
                    #endregion
            }
            //Debug.Log("!!!");
            OnBlendEvent();
        }
        Vector3 Slerp(Vector3 a, Vector3 b, float t)
        {
            return a * (1 - t) + b * t;
        }
        static public MyMotion Get(GameObject go)
        {
            //Debug.Log(go);
            MyMotion motion = go.GetComponent<MyMotion>();
            if (motion == null) motion = go.AddComponent<MyMotion>();
            return motion;
        }
        void OnFinish()
        {
            if (mode == Modes.Absolute)
            {
                if (isPostion) gameObject.transform.position = targetPostion;
                if (isScale) gameObject.transform.localScale = targetScale;
                if (isRotate) gameObject.transform.rotation = targetRotate;
            }
            else
            {
                if (isPostion)
                {
                    Vector3 valuePostion;
                    //到达最终混合时间时
                    valuePostion = lastOffsetPostion + targetPostion - offsetPostion;
                    //Debug.Log(valuePostion+","+lastOffsetPostion + "," + targetPostion + "," + offsetPostion);
                    //记录当前偏移值
                    offsetPostion += valuePostion;
                    //记录为上一次的偏移值
                    lastOffsetPostion = offsetPostion;
                    gameObject.transform.position += valuePostion;
                }
                if (isScale)
                {
                    Vector3 valueScale;
                    valueScale = lastOffsetScale + targetScale - offsetScale;
                    offsetScale += valueScale;
                    lastOffsetScale = offsetScale;
                    gameObject.transform.localScale += valueScale;
                }
                if(isRotate)
                {
                    Quaternion valueRotate;
                    valueRotate = Quaternion.Euler(lastOffsetRotate.eulerAngles + targetRotate.eulerAngles - offsetRotate.eulerAngles);
                    offsetRotate = Quaternion.Euler(offsetRotate.eulerAngles + valueRotate.eulerAngles);
                    lastOffsetRotate = offsetRotate;
                    gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles+valueRotate.eulerAngles);
                }
            }
            blendTime = 0;
            isMotion = false;
            isPostion = false;
            isScale = false;
            isRotate = false;
            isGravity = false;
            OnFinishEvent();
            //OnClear();
        }
        void OnClear()
        {
            if (onFinishEvent == null)
            {
                //Debug.Log("列表为空");
                //Destroy(this);
            }
            else { onFinishEvent = null; }
            if(onBlendEvent == null)
            {

            }
            else { onBlendEvent = null; }
        }
        void OnDestroy()
        {
            //Debug.Log("运动完成");
        }
    }
}