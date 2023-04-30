using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Water : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private MeshFilter meshFilter;

    private Vector3[] waterVerts;

    private int width = 20;
    private int height = 20;

    private float scale = 1.0f;

    public static float WaterTime = 0.0f;


    Mesh generateMeshGrid(int width, int height, float scale) {
        Mesh mesh = new Mesh();
        int[,] arrayValues = new int[width, height];
        Vector3[] vertices = new Vector3[width * height];
        int index = 0;
        for (int x = 0;x < width;x++)
        {
            for (int z = 0;z < height;z++)
            {
                vertices[index++] = new Vector3((x - ((float)width/2.0f)) * scale, 0.0f, (z - ((float)height/2.0f)) * scale); // Some scaling might be needed
            }
        }
        int[] triangles = new int[(width - 1) * (height - 1) * 2 * 3]; // 64x64 quads, 2 triangles per quad, 3 vertices per triangle
        index = 0;
        for (int x = 0;x < width - 1;x++)
        {
            for (int z = 0;z < height - 1;z++)
            {
                // Triangle 1 of quad
                triangles[index++] = x + z * width;
                triangles[index++] = x + 1 + z * width; // Swap with line below to change triangle front face
                triangles[index++] = x + 1 + (z + 1) * width;
                // Triangle 2 of quad
                triangles[index++] = x + z * width;
                triangles[index++] = x + 1 + (z + 1) * width; // Swap with line below to change triangle front face
                triangles[index++] = x + (z + 1) * width;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.MarkDynamic();
        mesh.MarkModified();
        // oh god a side effect make it stop!!!!
        this.waterVerts = vertices;
        return mesh;
    }

    void Start()
    {
        // generate mesh
        //meshFilter.sharedMesh = generateMeshGrid(width, height, scale);
    }

    public static float ComputeWaterHeight(float x, float z, float time) {
       return (Mathf.PerlinNoise(x / 15.0f + time / 5.0f, z / 15.0f + time / 5.0f) - 0.5f) * 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        /*WaterTime += Time.deltaTime;
        int index = 0;
        Vector3[] displayVerts = new Vector3[width * height];
        for (int x = 0;x < width;x++)
        {
            for (int z = 0;z < height;z++)
            {
                int currIdx = index++;
                Vector3 scaleJumper = new Vector3(
                        Mathf.Floor(transform.position.x / scale) * scale,
                        Mathf.Floor(transform.position.y / scale) * scale,
                        Mathf.Floor(transform.position.z / scale) * scale);
                Vector3 transformedVert = this.waterVerts[currIdx] - transform.position + scaleJumper;
                
                Debug.Log("-debuggz-");
                Debug.Log(this.waterVerts[currIdx]);
                Debug.Log(transform.position);
                Debug.Log(scale);
                Debug.Log(scaleJumper);
                Debug.Log(transformedVert);

                transformedVert.y = ComputeWaterHeight(this.waterVerts[currIdx].x + scaleJumper.x, this.waterVerts[currIdx].z + scaleJumper.z, WaterTime);
                displayVerts[currIdx] = transformedVert;
            }
        }
        Debug.Log("mfilter");
        Debug.Log(meshFilter.transform.position);
        meshFilter.sharedMesh.vertices = displayVerts;
        meshFilter.sharedMesh.RecalculateNormals();*/
    }
}
