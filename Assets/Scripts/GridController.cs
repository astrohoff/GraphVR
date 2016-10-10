using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour {
    public float renderDistance = 100;
    public float minorUnits = 1;
    public float majorUnits = 4;
    Mesh gridMesh;

	void Start () {
        gridMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = gridMesh;
        BuildGrid();
	}

    void BuildGrid()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> majorIndices = new List<int>();
        List<int> minorIndices = new List<int>();
        // Generate vertices and indices for grid mesh.
        for(float z = -renderDistance; z <= renderDistance; z += minorUnits)
        {
            for (float y = -renderDistance; y <= renderDistance; y += minorUnits)
            {
                for (float x = -renderDistance; x <= renderDistance; x += minorUnits)
                {
                    if(x == -renderDistance)
                    {
                        if(Mathf.Abs(y % majorUnits) < minorUnits / 10 && Mathf.Abs(z % majorUnits) < minorUnits / 10)
                        {
                            AddXLine(y, z, ref vertices, ref majorIndices);
                        }
                        else
                        {
                            AddXLine(y, z, ref vertices, ref minorIndices);
                        }
                    }
                    if(y == -renderDistance)
                    {
                        if(Mathf.Abs(x % majorUnits) < minorUnits / 10 && Mathf.Abs(z % majorUnits) < minorUnits / 10)
                        {
                            AddYLine(x, z, ref vertices, ref majorIndices);
                        }
                        else
                        {
                            AddYLine(x, z, ref vertices, ref minorIndices);
                        }
                    }
                    if(z == -renderDistance)
                    {
                        if(Mathf.Abs(x % majorUnits) < minorUnits / 10 && Mathf.Abs(y % majorUnits) < minorUnits / 10)
                        {
                            AddZLine(x, y, ref vertices, ref majorIndices);
                        }
                        else
                        {
                            AddZLine(x, y, ref vertices, ref minorIndices);
                        }
                    }
                }
            }
        }
        gridMesh.SetVertices(vertices);
        gridMesh.subMeshCount = 2;
        gridMesh.SetIndices(majorIndices.ToArray(), MeshTopology.Lines, 0);
        gridMesh.SetIndices(minorIndices.ToArray(), MeshTopology.Lines, 1);
        gridMesh.RecalculateBounds();
    }
    void AddXLine(float y, float z, ref List<Vector3> vertices, ref List<int> indices)
    {
        indices.Add(vertices.Count);
        vertices.Add(new Vector3(-renderDistance, y, z));
        indices.Add(vertices.Count);
        vertices.Add(new Vector3(renderDistance, y, z));
    }
    void AddYLine(float x, float z, ref List<Vector3> vertices, ref List<int> indices)
    {
        indices.Add(vertices.Count);
        vertices.Add(new Vector3(x, -renderDistance, z));
        indices.Add(vertices.Count);
        vertices.Add(new Vector3(x, renderDistance, z));
    }
    void AddZLine(float x, float y, ref List<Vector3> vertices, ref List<int> indices)
    {
        indices.Add(vertices.Count);
        vertices.Add(new Vector3(x, y, -renderDistance));
        indices.Add(vertices.Count);
        vertices.Add(new Vector3(x, y, renderDistance));
    }
}
