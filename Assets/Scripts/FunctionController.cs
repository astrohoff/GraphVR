using UnityEngine;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

public class FunctionController : MonoBehaviour {
    public string functionString = "f(x, y)=x^2+y^2";
    public bool refresh;
    public float bound = 10;
    public int segments = 24;
    Mesh mesh;

	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        BuildMesh(new Function(functionString));
	}
	
	// Update is called once per frame
	void Update () {
        if (refresh)
        {
            BuildMesh(new Function(functionString));
            refresh = false;
        }
	}

    void BuildMesh(Function funct)
    {
        if (funct.checkSyntax())
        {
            mesh.vertices = GetVertices(funct);
            mesh.SetIndices(GetIndices(funct), MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
        else
        {
            BuildErrorMesh(1);
        }
    }
    Vector3[] GetVertices(Function funct)
    {
        Vector3[] verts;
        int vertsPerAxis = segments + 1;
        float segmentLength = 2 * bound / segments;
        if (funct.getArgumentsNumber() == 2)
        {
            verts = new Vector3[vertsPerAxis * vertsPerAxis];
            for(int i = 0; i < vertsPerAxis; i++)
            {
                float z = -bound + segmentLength * i;
                for(int j = 0; j < vertsPerAxis; j++)
                {
                    float x = -bound + segmentLength * j;
                    float y = (float)funct.calculate(x, z);
                    verts[vertsPerAxis * i + j] = new Vector3(x, y, z);
                }
            }
        }
        else
        {
            verts = null;
        }
        return verts;
    }

    int[] GetIndices(Function funct)
    {
        int[] indices;
        if(funct.getArgumentsNumber() == 2)
        {
            indices = new int[4 * segments * segments];
            for(int i = 0; i < segments; i++)
            {
                for(int j = 0; j < segments; j++)
                {
                    int quadStartIndex = 4 * (i * segments + j);
                    indices[quadStartIndex] = i * segments + j;
                    indices[quadStartIndex + 1] = i * segments + j + 1;
                    indices[quadStartIndex + 2] = (i + 1) * segments + j + 1;
                    indices[quadStartIndex + 3] = (i + 1) * segments + j;
                }
            }
        }
        else
        {
            indices = null;
        }
        return indices;
    }

    // Return "X" error mesh.
    void BuildErrorMesh(float scale)
    {
        Vector3[] vertices = new Vector3[4] {new Vector3(-scale, -scale, 0), new Vector3(scale, scale, 0),
                                             new Vector3(-scale, scale, 0), new Vector3(scale, -scale, 0)};
        int[] indices = new int[4] { 0, 1, 2, 3 };
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
    }
}
