using UnityEngine;
using System.Collections;
namespace MySpace.Text
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CustomText : MonoBehaviour
    {
        void OnDestroy()
        {
            //Debug.Log("销毁" + this + mCamera.counter--);
            call = null;
            //mesh.Clear(false);
            Object.Destroy(mesh);
        }
        ~CustomText()
        {
            mesh = null;
            //Debug.Log("Free");
        }
        public Alignment alignment;
        public Material material;
        public Texture texture;

        public int order = 0;
        public int number = 0;
        public float current = 0;
        public float rate = 0.1f;
        public int target = 0;
        public bool isInit = false;

        public delegate void Call();
        public Call call;
        void OnFinish()
        {
            if (call != null) call();
            //call?.Invoke();
        }

        public bool isDelay = false;
        //public float delay = 0;
        // Use this for initialization
        public enum Alignment
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        protected Mesh mesh;

        //protected Vector3[] vert;
        protected Vector3[] vert = new Vector3[] { new Vector3(0, -0.5f), new Vector3(1, -0.5f), new Vector3(0, 0.5f), new Vector3(1, 0.5f) };
        protected int[] tri = new int[] { 0, 1, 2, 2, 3, 1 };
        protected Vector2[] uv = new Vector2[] { new Vector3(0, 0), new Vector3(1, 0), new Vector3(0, 1), new Vector3(1, 1) };
        public void Init()
        {
            GetComponent<MeshRenderer>().sortingOrder = order;
            if (true || GetComponent<MeshRenderer>().material == null || GetComponent<MeshRenderer>().material.name == "Default-Material (Instance)")
            {
                GetComponent<MeshRenderer>().material = material;
            }
            else
            {
                Debug.Log(GetComponent<MeshRenderer>().material.name);
            }
            if (true || GetComponent<MeshRenderer>().material.mainTexture == null)
            {
                GetComponent<MeshRenderer>().material.mainTexture = texture;
//#if UNITY_EDITOR
//                Debug.Log("!!!");
//#else
//                GetComponent<MeshRenderer>().material.mainTexture = texture;
//#endif
            }
            //Debug.Log(number);
            if (isInit) Display(number, rate);
        }
        void Awake()
        {
            Init();
        }
        void Start()
        {

        }
        public Mesh Create()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vert;
            mesh.triangles = tri;
            mesh.uv = uv;
            return mesh;
        }
        public void Refresh()
        {
            mesh.Clear(false);
            mesh.vertices = vert;
            mesh.triangles = tri;
            mesh.uv = uv;
        }
        public void Display(int num, float speed)
        {
            rate = speed;
            target = num;
            //if ((current - (int)current) > 0.5f) number = (int)current + 1;
            //else number = (int)current;
            number = (int)current;
            //Debug.Log("输入:"+num);

            if (isDelay == false)
            {
                isDelay = true;
                StartCoroutine(DisplayDelay());
            }
        }
        public void Display(int num)
        {
            lock (this)
            {
                if (mesh == null)
                {
                    //Debug.Log(num);
                    mesh = Create();
                    GetComponent<MeshFilter>().mesh = mesh;
                }
            }
            int n = 0;
            int[] nums;
            if (num != 0)
            {
                int tmp = num;
                while (tmp != 0)
                {
                    tmp = tmp / 10;
                    n++;
                }
                nums = new int[n];
                for (int i = 0; i < n; i++)
                {
                    nums[n - i - 1] = num % 10;
                    num = num / 10;
                }
            }
            else
            {
                nums = new int[1];
                nums[0] = 0;
            }
            //Debug.Log("OK"+nums.Length);
            foreach (int a in nums)
            {
                //Debug.Log(""+a);
            }
            WeaveNumber(ref vert, ref tri, ref uv, nums);
            number = num;
            Refresh();
            //Create();
            //GetComponent<MeshFilter>().sharedMesh = Create();
        }
        Vector2[] NumberUV(int i)
        {
            if (i > 10) i = i % 10;
            if (i < 0) i = 0;
            Vector2[] uv = new Vector2[4];
            uv[0] = new Vector2(i * 0.1f + 0.0025f, 0f);
            uv[1] = new Vector2((i + 1) * 0.1f - 0.0025f, 0f);
            uv[2] = new Vector2(i * 0.1f + 0.0025f, 1f);
            uv[3] = new Vector2((i + 1) * 0.1f - 0.0025f, 1f);
            return uv;
        }
        Vector3[] NumberVert(int i)
        {
            Vector3[] vert = new Vector3[4];
            vert[0] = new Vector3(i * 1f, 0f);
            vert[1] = new Vector3((i + 1) * 1f, 0f);
            vert[2] = new Vector3(i * 1f, 1f);
            vert[3] = new Vector3((i + 1) * 1f, 1f);
            return vert;
        }
        int[] NumberTri(int i)
        {
            int[] tri = new int[6];
            tri[0] = 0 + 4 * i;
            tri[1] = 1 + 4 * i;
            tri[2] = 2 + 4 * i;

            tri[3] = 2 + 4 * i;
            tri[4] = 3 + 4 * i;
            tri[5] = 1 + 4 * i;

            return tri;
        }
        void WeaveNumber(ref Vector3[] vert, ref int[] tri, ref Vector2[] uv, int[] num)
        {
            Vector3 offset = new Vector3();
            switch (alignment)
            {
                case Alignment.Left: offset = new Vector3(0, 0, 0); break;
                case Alignment.Right: offset = new Vector3(-num.Length, 0, 0); break;
                case Alignment.Center: offset = new Vector3(-num.Length / 2f, 0, 0); break;
                default: break;
            }

            vert = new Vector3[num.Length * 4];
            tri = new int[num.Length * 6];
            uv = new Vector2[num.Length * 4];

            Vector3[] vertNum;
            int[] triNum;
            Vector2[] uvNum;

            int queue = 0;
            for (int i = 0; i < num.Length; i++)
            {
                vertNum = NumberVert(i);
                triNum = NumberTri(i);
                uvNum = NumberUV(num[i]);

                for (int j = 0; j < 4; j++)
                {
                    vert[4 * queue + j] = vertNum[j] + offset;
                }
                for (int j = 0; j < 6; j++)
                {
                    tri[6 * queue + j] = triNum[j];
                }
                for (int j = 0; j < 4; j++)
                {
                    uv[4 * queue + j] = uvNum[j];
                }
                queue += 1;
            }
        }
        IEnumerator DisplayDelay()
        {
            float time = 0.025f;

            float diff = 0;

            while (true)
            {
                //yield return new WaitForSeconds(time);

                //差值等于当前的数值减去目标数值

                diff = current - target;
                if (diff > 0 && diff < 3f) diff = 3f;
                if (diff < 0 && diff > -3f) diff = -3f;

                //Debug.Log(diff);

                if (rate == 0) current = target;
                else
                {
                    current = current - rate * diff;
                }
                //显示当前值
                Display((int)current);
                if (target == (int)current) break;
                yield return 1;
            }
            OnFinish();
            isDelay = false;
        }
    }
}