using UnityEngine;
using org.mariuszgromada.math.mxparser;

// Class for building graph meshes.
public static class MeshBuilder {

    // Update the referenced mesh with geometry for the given function.
    public static void UpdateFunctionMesh(ref Mesh mesh, Function function, float bound, int vertsPerAxis, MeshTopology meshType)
    {
        if (function.checkSyntax())
        {
            Vector3[] verts = GetFunctionVertices(function, bound, vertsPerAxis);
            if(verts == null)
            {
                Debug.Log("Verts null in UpdateFunctionMesh()");
            }
            mesh.vertices = verts;
            mesh.SetIndices(GetFunctionIndices(bound, vertsPerAxis, function.getArgumentsNumber(), meshType), meshType, 0);
            mesh.RecalculateBounds();
            if(meshType == MeshTopology.Triangles || meshType == MeshTopology.Quads)
            {
                mesh.RecalculateNormals();
            }
        }
        else
        {
            BuildErrorMesh(ref mesh);
        }
    }

    // Get an array of vertex positions for a function mesh.
    static Vector3[] GetFunctionVertices(Function function, float bound, int vertsPerAxis)
    {
        if(function.getArgumentsNumber() == 2)
        {
            Vector3[] verts = GetSamplingPositions(bound, vertsPerAxis, function.getArgumentsNumber());
            for(int i = 0; i < verts.Length; i++)
            {
                verts[i].y = (float)function.calculate(verts[i].x, verts[i].z);
            }
            if(verts == null)
            {
                Debug.Log("null verts in GetFunctionVertices().");
            }
            return verts;
        }
        else
        {
            Debug.Log("Unsupported dimensions.");
            return null;
        }

    }
    
    // Get an array of positions on the x-z plane to sample function values from.
    static Vector3[] GetSamplingPositions(float bound, int vertsPerAxis, int dimensions)
    {
        if(dimensions == 2)
        {
            Vector3[] samplePositions = new Vector3[vertsPerAxis * vertsPerAxis];
            float segmentLength = 2 * bound / (vertsPerAxis - 1);
            float z = -bound;
            int samplePosIndex = 0;
            for (int i = 0; i < vertsPerAxis; i++)
            {
                float x = -bound;
                for (int j = 0; j < vertsPerAxis; j++)
                {
                    samplePositions[samplePosIndex] = new Vector3(x, 0, z);
                    x += segmentLength;
                    samplePosIndex++;
                }
                z += segmentLength;
            }
            return samplePositions;
        }
        else
        {
            Debug.Log("Unsupported dimensions.");
            return null;
        }
        
    }

    // Build an "X" mesh to indicate an error.
    static void BuildErrorMesh(ref Mesh mesh)
    {
        float scale = 1;
        Vector3[] vertices = new Vector3[4] {new Vector3(-scale, -scale, 0), new Vector3(scale, scale, 0),
                                             new Vector3(-scale, scale, 0), new Vector3(scale, -scale, 0)};
        int[] indices = new int[4] { 0, 1, 2, 3 };
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
    }

    // Get an array of vertex indices. This array defines the primitives of the mesh.
    static int[] GetFunctionIndices(float bound, int vertsPerAxis, int dimensions, MeshTopology meshType)
    {
        if(dimensions == 2)
        {
            if(meshType == MeshTopology.Points)
            {
                int[] indices = new int[vertsPerAxis * vertsPerAxis];
                for(int i = 0; i < indices.Length; i++)
                {
                    indices[i] = i;
                }
                return indices;
            }
            else
            {
                Debug.Log("Unsupported mesh type.");
                return null;
            }
        }
        else
        {
            Debug.Log("Unsupported dimensions.");
            return null;
        }
    }
}
