using UnityEngine;
using System.Collections;
namespace MySpace.Weaver
{
    #region 网格编织2D模式:Top and Bottom
    public class Weaver2D
    {
        float MarginTop = 0;
        float MarginBottom = 0;
        float MarginLeft = 0;
        float MarginRight = 0;

        Vector3[] top;
        Vector3[] bottom;

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;

        Vector3 anchor;
        public Weaver2D SetMargin(float top, float bottom, float left, float right)
        {
            MarginTop = top;
            MarginBottom = bottom;
            MarginLeft = left;
            MarginRight = right;
            return this;
        }
        public Weaver2D SetAnchor(Vector3 anchor)
        {
            this.anchor = anchor;
            return this;
        }
        public Weaver2D SetVertices(Vector3[] top, Vector3[] bottom)
        {
            this.top = top;
            this.bottom = bottom;
            return this;
        }
        /// <summary>
        /// 根据顶层和底层顶点编织顶点UV贴图和三角形
        /// </summary>
        public void Complect(out Vector3[] vertices, out int[] triangles, out Vector2[] uvs)
        {
            vertices = new Vector3[top.Length + bottom.Length];
            for (int i = 0; i < bottom.Length; i++)
            {
                vertices[i] = bottom[i] + anchor;
            }
            for (int i = 0; i < top.Length; i++)
            {
                vertices[i + bottom.Length] = top[i] + anchor;
            }
            triangles = CreateTriangles(top.Length, bottom.Length);
            uvs = CreateUVs(top.Length, bottom.Length);
        }
        public Mesh Complect()
        {
            Mesh mesh = new Mesh();
            Complect(out vertices, out triangles, out uvs);
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            return mesh;
        }
        /// <summary>
        /// 根据上下两层顶点建立UV贴图映射
        /// </summary>
        protected Vector2[] CreateUVs(int lengthTop, int lengthBottom)
        {
            Vector2[] uvs = new Vector2[lengthTop + lengthBottom];
            float top = (1f - (MarginRight + MarginLeft)) / (lengthTop - 1);
            float buttom = (1f - (MarginRight + MarginLeft)) / (lengthBottom - 1);
            //Debug.Log("顶:" + top);
            //Debug.Log("底:" + buttom);
            int queue = 0;
            for (int i = 0; i < lengthBottom; i++)
            {
                uvs[queue++] = new Vector2(MarginLeft + i * buttom, 0f + MarginBottom);
            }
            for (int i = 0; i < lengthTop; i++)
            {
                uvs[queue++] = new Vector2(MarginLeft + i * top, 1f - MarginTop);
            }
            return uvs;
        }
        /// <summary>
        /// 根据上下两层顶点建立三角形映射
        /// </summary>
        protected int[] CreateTriangles(int lengthTop, int lengthBottom)
        {
            /*三角形总数等于顶点总数减二*/
            int size = lengthTop + lengthBottom - 2;
            if (size < 2) return null;
            int[] triangles = new int[3 * size];
            /*构建三角形*/
            int top = 0;
            int bottom = 0;
            int queue = 0;
            for (int i = 0; i < (lengthBottom - 1); i++)
            {
                triangles[queue++] = bottom;
                triangles[queue++] = bottom + 1;
                triangles[queue++] = lengthBottom + top;
                if (bottom < lengthBottom) bottom++;
                if (top < lengthTop) top++;
            }
            top = 0;
            if (lengthBottom != 1)
                bottom = 1;
            else
                bottom = 0;
            for (int i = 0; i < (lengthTop - 1); i++)
            {
                triangles[queue++] = lengthBottom + top;
                triangles[queue++] = lengthBottom + top + 1;
                triangles[queue++] = bottom;
                if (bottom < lengthBottom && lengthBottom != 1) bottom++;
                if (top < lengthTop) top++;
            }
            return triangles;
        }
    }
    #endregion
}