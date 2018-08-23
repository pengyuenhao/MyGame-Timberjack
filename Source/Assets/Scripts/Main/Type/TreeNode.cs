using UnityEngine;
using System.Collections;
namespace MySpace.Tree
{
    public class TreeNode : MonoBehaviour
    {
        //0表示无树枝，1表示左边有树枝，2表示右边有树枝
        public int status;
        public GameObject gameObject;
        public TreeNode pre;
        public TreeNode post;
        public Bounds bounds;
        public void Clear()
        {
            post.pre = null;
            pre.post = null;
            pre = null;
            post = null;
            Destroy(gameObject);
        }
    }
}