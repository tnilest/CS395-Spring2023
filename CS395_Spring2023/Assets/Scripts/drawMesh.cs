using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawMesh : MonoBehaviour
{
    Mesh m;
    MeshFilter mf;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        m = new Mesh();
        mf.mesh = m;
        drawTriangle();
        MeshCollider c = GetComponent<MeshCollider>();
        c.sharedMesh = m;
    }

    void drawTriangle()
    {
        Vector3[] vertArr = new Vector3[3];
        int[] triArr = new int[3];

        vertArr[0] = new Vector3(-1, 1, 0);
        vertArr[1] = new Vector3(-1, 0, 0);
        vertArr[2] = new Vector3(1, 0, 0);

        triArr[0] = 0;
        triArr[1] = 1;
        triArr[2] = 2;

        m.vertices = vertArr;
        m.triangles = triArr;
    }
}
