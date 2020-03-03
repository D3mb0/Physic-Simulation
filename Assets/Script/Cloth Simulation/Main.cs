using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public CRenderer cRenderer = new CRenderer();

    public bool stop = false;

    public int rowNodeSize = 17;
    public int columnNodeSize = 17;  //17 nnode =16 grid
    public float KS = 1000f;
    public float KD = 0.1f;

    public float dt = 0.008f;
    public float lineSize = 16f;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    Mesh front_mesh;

    [SerializeField] Material material;


    Vector3[] vertices;
    int[] triangles;

    List<CNode> mNodeArray = new List<CNode>();
    void CreateShape()
    {
        //triangles = new int[cRenderer.Cloth_Model_list[0].getTriangle().Length];
        //triangles = cRenderer.Cloth_Model_list[0].getTriangle();
        mNodeArray = cRenderer.Cloth_Model_list[0].getNodeArray();
        vertices = new Vector3[mNodeArray.Count];


        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
         

        for (int i = 0, y = 0; y <= columnNodeSize-1; y++)
        {
            for (int x = 0; x <= rowNodeSize-1; x++, i++)
            {
               
                tangents[i] = new Vector4(1f, 0f, 0f, -1f);
                uv[i] = new Vector2((float)x /rowNodeSize, (float)y / columnNodeSize);
            }
        }

        for (int i = 0; i < vertices.Length;i++)
        {
            vertices[i] = mNodeArray[i].mPosition;
            //Debug.Log("vertices[" + i + "] :" + vertices[i]);
        }

        triangles = new int[(rowNodeSize-1) * (columnNodeSize - 1) * 6 *2];

        for (int ti = 0, vi = 0, y = 0; y < columnNodeSize -1; y++, vi++)
        {
            for (int x = 0; x < rowNodeSize -1; x++, ti += 12, vi++)
            {
                // back side triangle generator
                triangles[ti] = vi;
                triangles[ti + 1] = vi + rowNodeSize-1 + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + rowNodeSize - 1 + 1;
                triangles[ti + 5] = vi + rowNodeSize - 1 + 2;
                // front side triangle generator
                triangles[ti + 6] = vi;
                triangles[ti + 7] = vi + 1;
                triangles[ti + 8] = vi + rowNodeSize - 1 + 1;

                triangles[ti + 9] = vi + 1;
                triangles[ti + 10] = vi + rowNodeSize - 1 + 2;
                triangles[ti + 11] = vi + rowNodeSize - 1 + 1;
            }
        }
        mesh.uv = uv;
        mesh.tangents = tangents;
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
    // Start is called before the first frame update
    void Start()
    {
        CCloth cloth = new CCloth(rowNodeSize, columnNodeSize, 0, 1, 1, dt,lineSize, KS, KD);
        cRenderer.AddClothModel(cloth);
        Debug.Log(cRenderer.Cloth_Model_list[0].mNodeArray.Count);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
        //meshRenderer = gameObject.AddComponent<MeshRenderer>();
       

    }
    private void OnDrawGizmos()
    {
        cRenderer.render();
    }
    // Update is called once per frame
    void Update()
    {
        CreateShape();
        UpdateMesh();
        cRenderer.update();
    }
}
