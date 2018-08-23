using UnityEngine;
using System.Collections;
namespace MySpace.Structure
{
    /**自定义栈接口*/
    public interface MyStack<T>
    {
        /// <summary>
        /// 判断是否为空栈
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// 清空栈
        /// </summary>
        void Clear();

        /// <summary>
        /// 顺序入栈
        /// </summary>
        bool Push(T data);
        /// <summary>
        /// 顺序出栈
        /// </summary>
        T Pull();
        /// <summary>
        /// 抽取栈底
        /// </summary>
        T Pump();

        int Counter { get; }
    }
}