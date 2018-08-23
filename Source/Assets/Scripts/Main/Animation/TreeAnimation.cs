using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySpace.Structure;
using MySpace.Tree;
using System;
using MySpace.Motion;

namespace MySpace.Animation
{
    public class TreeAnimation : MonoBehaviour
    {
        public Vector3 scale = new Vector3(1,1,1);
        public Vector3 originOffset;
        public AudioClip clip;
        public AudioSource audio;
        static System.Random rand = new System.Random(Environment.TickCount);
        public TreeNode lastNode;
        public Vector3 anchor;
        public Sprite stump;
        public Sprite[] trunk;
        public Sprite[] branch;
        public bool isLeft {
            get
            {
                if (nodeList.Count != 0 && nodeList[0].status == 1)
                    return true;
                else
                    return false;
            }
        }
        public bool isRight
        {
            get
            {
                if (nodeList.Count != 0 && nodeList[0].status == 2)
                    return true;
                else
                    return false;
            }
        }
        public int counter;
        //public NodeInfo[] nodes;
        public List<TreeNode> nodeList;
        //栈模式
        //public MySeqStack<Node> nodeStack;
        GameObject origin;
        int index;
        Vector3 offset;
        public int leftCounter = 0;
        public int rightCounter = 0;
        //双边最大差值
        public int diffMax;
        //交互最大差值
        public int exchangeMax;
        public int value;

        void Start()
        {
            diffMax = rand.Next(3, 5);
            exchangeMax = rand.Next(1, 3);
            CreateTree();
            //transform.localScale = scale;
        }
        void CreateTree()
        {
            audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clip;
            origin = new GameObject();
            origin.transform.parent = transform;
            origin.transform.position = transform.position + anchor;
            origin.AddComponent<SpriteRenderer>().sprite = stump;
            origin.GetComponent<SpriteRenderer>().sortingOrder = 2;
            origin.name = "origin";
            //origin.transform.localScale = scale;
            //nodes = new NodeInfo[counter];
            nodeList = new List<TreeNode>();
            //nodeStack = new MySeqStack<Node>();
            //offset = origin.transform.position - new Vector3(0, stump.bounds.size.y, 0);

            for (int i = 0; i < counter; i++)
            {
                CreateTrunk();
            }
        }
        void CreateTrunk()
        {
            Vector3 offset;
            index = rand.Next(0, trunk.Length);
            if (nodeList.Count == 0)
            {
                offset = origin.transform.position + new Vector3(0, 0.5f*stump.bounds.size.y, 0) + new Vector3(0, 0.5f*trunk[index].bounds.size.y, 0) + originOffset;
            }
            else
            {
                offset = nodeList[nodeList.Count-1].transform.position + new Vector3(0, trunk[index].bounds.size.y, 0);
            }
            GameObject tmp = new GameObject();
            tmp.transform.parent = transform;
            tmp.transform.position = offset;
            //tmp.transform.localScale = new Vector3(1,1,1);

            tmp.AddComponent<SpriteRenderer>().sprite = trunk[index];
            tmp.name = "node"+nodeList.Count;
            TreeNode node = tmp.AddComponent<TreeNode>();
            node.gameObject = tmp;
            node.bounds = tmp.GetComponent<SpriteRenderer>().bounds;
            node.GetComponent<SpriteRenderer>().sortingOrder = 2;

            if (nodeList.Count > 0)
            {
                node.post = nodeList[nodeList.Count - 1];
                nodeList[nodeList.Count - 1].pre = node;
            }
            CreateBranch(node, value);
            nodeList.Add(node);
            if (rand.Next(0, 100) > 95)
            {
                diffMax = rand.Next(3, 5);
                exchangeMax = rand.Next(1, 3);
            }
        }
        /// <summary>
        /// 0表示不创建树枝，100表示尽可能创建树枝，0-100之间表示创建树枝的概率
        /// </summary>
        /// <param name="node">进行操作的树枝节点</param>
        /// <param name="value">设置参数</param>
        void CreateBranch(TreeNode node,int value)
        {
            if (value > rand.Next(0,100))
            {
                //根节点不能生成树枝
                if (node.post == null || node.post.post==null)
                {
                    node.status = 0;
                }
                else
                {
                    if (node.post.status != 0&&Mathf.Abs(leftCounter - rightCounter) > exchangeMax)
                    {
                        node.status = 0;
                    }
                    else
                    {
                        //下面一节没有树枝时则左右都可以生成树枝
                        // o||o
                        //  ||
                        if (node.post.status == 0)
                        {
                            //超出最大差值范围后将主动条件树枝生长方向
                            if (Mathf.Abs(leftCounter - rightCounter) > diffMax)
                            {
                                if ((leftCounter - rightCounter) > 0)
                                {
                                    node.status = 2;
                                }
                                else
                                {
                                    node.status = 1;
                                }
                            }
                            else
                            {
                                node.status = rand.Next(1, 3);
                            }
                        }
                        //下面一节树枝在左边时右边则不能生成树枝
                        // o||x
                        // -||
                        if (node.post.status == 1)
                        {
                            node.status = 1;
                        }
                        //下面一节树枝在右边时左边则不能生成树枝
                        // x||o
                        //  ||-
                        if (node.post.status == 2)
                        {
                            node.status = 2;
                        }
                    }
                }
            }
            else
            {
                node.status = 0;
            }
            index = rand.Next(0, branch.Length);
            if (node.status == 1)
            {
                GameObject tmp = new GameObject();
                tmp.AddComponent<SpriteRenderer>().sprite = branch[index];
                tmp.transform.parent = node.gameObject.transform;
                tmp.transform.position = node.gameObject.transform.position
                    - new Vector3(0.5f*node.bounds.size.x+0.5f*branch[index].bounds.size.x, 0);
                //Debug.Log(branch[index].bounds.size.x);
                tmp.transform.localScale = new Vector3(1.25f, 1.25f);

                tmp.name = "Branch Left";
                leftCounter++;
            }
            if(node.status == 2)
            {
                GameObject tmp = new GameObject();
                tmp.AddComponent<SpriteRenderer>().sprite = branch[index];
                tmp.transform.parent = node.gameObject.transform;
                tmp.transform.position = node.gameObject.transform.position
                    + new Vector3(0.5f*node.bounds.size.x+0.5f*branch[index].bounds.size.x, 0);
                tmp.transform.localScale = new Vector3(1.25f, 1.25f);
                tmp.GetComponent<SpriteRenderer>().flipX = true;
                tmp.name = "Branch Right";
                rightCounter++;
            }
        }
        //砍去底部树干
        public void RemoveTrunk(Vector3 postion,Vector3 rotate)
        {
            if (nodeList.Count == 0) return;
            audio.Play();
            TreeNode bottomNode = nodeList[0];
            bottomNode.GetComponent<SpriteRenderer>().sortingOrder = 3;
            lastNode = bottomNode;
            nodeList.Remove(bottomNode);
            MyMotion.Get(bottomNode.gameObject)
                .Mode(MyMotion.Modes.Absolute)
                .Postion(postion)
                .Gravity(new Vector3(0,0.05f,0))
                .Rotate(rotate)
                .BlendTime(0.75f)
                .AddOnFinishListener(()=> 
                {
                    Destroy(bottomNode.gameObject);
                })
                .Run();
            CreateTrunk();
            nodeList[0].transform.position = origin.transform.position + new Vector3(0, 0.5f * stump.bounds.size.y, 0) + new Vector3(0, 1.5f * trunk[index].bounds.size.y, 0) + originOffset;
            //TreeMotion();

            //只运动最后一节树干
            MyMotion.Get(nodeList[0].gameObject).WaitFinish(() =>
            {
                MyMotion.Get(nodeList[0].gameObject)
                    .Mode(MyMotion.Modes.Relative)
                    .Postion(new Vector3(0, -bottomNode.GetComponent<SpriteRenderer>().sprite.bounds.size.y))
                    .BlendTime(0.075f)
                    .AddOnBlendListener(()=>
                    {
                        TreeMotion();
                    })
                    .AddOnFinishListener(()=>
                    {
                        TreeMotion();
                    })
                    .Run();
            }, true);
        }
        void TreeMotion()
        {
            TreeNode node = nodeList[0];
            while (node.pre != null)
            {
                node.pre.gameObject.transform.position = node.gameObject.transform.position + new Vector3(0, node.bounds.size.y);
                node = node.pre;
            }
            //Debug.Log("更新");
        }
    }
}