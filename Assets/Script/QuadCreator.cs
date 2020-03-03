using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadCreator : MonoBehaviour
{
    protected MeshFilter meshFilter;
    protected Mesh Mesh;

    public float UpdownFactor = 0.1f;
    public float UpdownSpeed = 6f;
    public float LeftFactor = 0.3f;
    public float LeftSpeed = 3f;
    public float LeftOffset = 2.3f;
    public float StrectFactor = -0.1f;
    public float StrectSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        Mesh = new Mesh();
        Mesh.name = "GeneratedMesh";

        Mesh.vertices = GenerateVertices();
        Mesh.triangles = GenerateTriangles();

        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = Mesh;
    }
    private Vector3[] GenerateVertices(float up = 0f,float left = 0f,float strect = 0f)
    {
        return new Vector3[]
        {
            //bottom
            new Vector3(-1,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1),

            //top
            new Vector3(-1,2+up,1),
            new Vector3(1,2+up,1),
            new Vector3(1,2+up,-1),
            new Vector3(-1,2+up,-1),

            //left
            new Vector3(-1,0,1),
            new Vector3(-1,0,-1),
            new Vector3(-1,2+up,1),
            new Vector3(-1,2+up,-1),

            //right
            new Vector3(1,0,1),
            new Vector3(1,0,-1),
            new Vector3(1,2+up,1),
            new Vector3(1,2+up,-1),

            //front
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1),
            new Vector3(1,2+up,-1),
            new Vector3(-1,2+up,-1),

            //back
            new Vector3(-1,0,1),
            new Vector3(1,0,1),
            new Vector3(-1,2+up,1),
            new Vector3(1,2+up,1),


        };
    }
    private int[] GenerateTriangles()
    {
        return new int[] { 
            //bottomtop
            1,0,2,
            2,0,3,
            4,5,6,
            4,6,7,

            //leftright
            9,10,11,
            8,10,9,
            12,13,15,
            14,12,15,

            //frontback
            16,17,19,
            18,16,19,
            20,21,23,
            22,20,23

        };
    }

    

    // Update is called once per frame
    void Update()
    {
        Mesh.vertices = GenerateVertices(Mathf.Sin(Time.realtimeSinceStartup * UpdownSpeed )* UpdownFactor,
                                         Mathf.Sin(Time.realtimeSinceStartup * LeftSpeed + LeftOffset) * LeftFactor,
                                         Mathf.Sin(Time.realtimeSinceStartup * StrectSpeed) * StrectFactor);

    }
}
