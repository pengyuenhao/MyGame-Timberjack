using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MySpace.Structure
{
    /**自定义链表栈*/
    public class MySeqStack<T> : MyStack<T>
    {
        /**
         * 栈顶指针*/
        private Node top;
        /**
         * 栈底指针*/
        private Node bottom;

        private int size;
        public int Counter { get { return size; } }
        public MySeqStack()
        {
            bottom = null;
            top = null;
            size = 0;
        }

        public bool IsEmpty{get { return size == 0; }}

        public void Clear()
        {
            bottom = null;
            top = null;
            size = 0;
        }

        public bool Push(T item)
        {
            /*新建一个节点*/
            Node node = new Node();
            node.item = item;
            /*设置新建立的节点的前一个节点为原先的栈顶*/
            node.pre = top;
            if (top != null)
                /*设置原先的栈顶的后一个节点为新建立的节点*/
                top.post = node;
            else
                /*如果栈顶为空则设置新建立的节点为栈底*/
                bottom = node;
            /*改变栈顶指针*/
            top = node;
            size++;
            //Log.i("Debug","栈顶"+top+"栈顶"+bottom);
            return true;
        }
        public T Pull()
        {
            /*判断节点指针是否为空*/
            if (top != null)
            {
                //Node node = new Node();
                //node.data = top.data;
                T data = top.item;
                Node pre = top.pre;
                //移除节点
                top.Clear();
                //Log.i("System","节点:"+pre+"出栈:"+data);
                /*改变栈顶指针*/
                top = pre;
                size--;
                /*栈内为空*/
                if (top == null) Clear();
                return data;
            }
            else
            {
                /*栈内为空*/
                Clear();
                return default(T);
            }
        }
        public T Pump()
        {
            /*判断节点指针是否为空*/
            if (bottom != null)
            {
                Node post = bottom.post;
                T data = bottom.item;
                //移除节点
                bottom.Clear();
                //Log.i("System","节点:"+post+"出栈:"+data);

                /*改变栈底指针*/
                bottom = post;
                size--;
                /*栈内为空*/
                if (bottom == null) Clear();
                return data;
            }
            else
            {
                /*栈内为空*/
                Clear();
                return default(T);
            }
        }
        /** 
* 将数据封装成结点 
*/
        private class Node
        {
            /**
             * 存储该节点之前的节点*/
            public Node pre = null;
            /**
             * 存储该节点之后的节点*/
            public Node post = null;
            /**
             * 存储节点携带的数据*/
            public T item;

            public void Clear()
            {
                pre = null;
                post = null;
                item = default(T);
            }
        }
    }
}