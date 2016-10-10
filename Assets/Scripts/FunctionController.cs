using UnityEngine;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

public class FunctionController : MonoBehaviour {
    public string functionString = "f(x, y)=x^2+y^2";
    float drawBound = 10;
    int boundDivisions = 24;
    public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Mesh GetFunctionMesh(Function function, float segmentLength)
    {
        if(function.checkSyntax() == false)
        {
            return GetErrorMesh(player.localScale.x);
        }
        Mesh fMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        if(function.getArgumentsNumber() == 1)
        {

        }
        else if(function.getArgumentsNumber() == 2)
        {
            int vertCount;
            float angleStep;
            float theta;
            float radius;
            float x = 0;
            float z = 0;
            float y = (float)function.calculate(x, z);
            vertices.Add(new Vector3(x, y, z));
            for (int ring = 1; ring * segmentLength <= drawBound; ring++)
            {
                vertCount = Mathf.RoundToInt(2 * Mathf.PI * ring);
                angleStep = 360 / vertCount;
                radius = ring * segmentLength;
                for (int vert = 0; vert < vertCount; vert++)
                {
                    theta = angleStep * vert;
                    x = Mathf.Cos(theta) * radius;
                    z = Mathf.Sin(theta) * radius;
                    y = (float)function.calculate(x, z);
                    vertices.Add(new Vector3(x, y, z));
                }
            }
        }
        return fMesh;
    }

    List<int> GetIndices(int numRings)
    {
        List<int> indices = new List<int>();
        indices.Add(0);
        for(int ring = 1; ring < numRings - 1; ring++)
        {
            /*float outTrisPerVert = (float)ringVertCount / prevRingVertCount;
            float outTris = 0;
            for(int vert = 0; vert < ringVertCount; vert++)
            {
                outTris += outTrisPerVert;

            }*/
            int ringVertCount = Mathf.RoundToInt(2 * Mathf.PI * ring);
            int ringVertOffset = GetVertexOffset(ring);
            int nextRingVertCount = Mathf.RoundToInt(2 * Mathf.PI * (ring + 1));
        }

    }

    int GetVertexOffset(int ringNum)
    {
        if(ringNum == 0)
        {
            return 0;
        }
        int offset = 1;
        for(int ring = 1; ring <= ringNum; ring++)
        {
            offset += Mathf.RoundToInt(2 * Mathf.PI * (ring - 1));
        }
        return offset;
    }

    // Return "X" error mesh.
    Mesh GetErrorMesh(float scale)
    {
        Mesh eMesh = new Mesh();
        Vector3[] vertices = new Vector3[4] {new Vector3(-scale, -scale, 0), new Vector3(scale, scale, 0),
                                             new Vector3(-scale, scale, 0), new Vector3(scale, -scale, 0)};
        int[] indices = new int[4] { 0, 1, 2, 3 };
        eMesh.vertices = vertices;
        eMesh.SetIndices(indices, MeshTopology.Lines, 0);
        return eMesh;
    }
}
