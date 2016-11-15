using UnityEngine;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System;

// Class for building graph meshes.
public static class MeshBuilder {

    // Update the referenced mesh with geometry for the given function.
    public static void UpdateFunctionMesh(ref Mesh mesh, Function function, float bound, int vertsPerAxis, MeshTopology meshType)
    {
        if (vertsPerAxis > 2 && bound > 0 && function.checkSyntax())
        {
            Vector3[] verts = GetFunctionVertices(function, bound, vertsPerAxis, meshType);
            if(verts == null)
            {
                Debug.Log("Verts null in UpdateFunctionMesh()");
            }
            mesh.triangles = null;
            mesh.normals = null;
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
            BuildErrorMesh(ref mesh, bound / 2);
        }
    }

    // Get an array of vertex positions for a function mesh.
    static Vector3[] GetFunctionVertices(Function function, float bound, int vertsPerAxis, MeshTopology meshType)
    {
        if(function.getArgumentsNumber() == 2)
        {
            Vector3[] verts = GetSamplingPositions(bound, vertsPerAxis, function.getArgumentsNumber());
            for(int i = 0; i < verts.Length; i++)
            {
                verts[i].y = (float)function.calculate(verts[i].x, verts[i].z);
            }
            if (verts == null)
            {
                Debug.Log("null verts in GetFunctionVertices().");
            }
            // Double vertices for 2 sided face normals.
            if (meshType == MeshTopology.Triangles || meshType == MeshTopology.Quads)
            {
                Vector3[] doubledVerts = new Vector3[verts.Length * 2];
                verts.CopyTo(doubledVerts, 0);
                verts.CopyTo(doubledVerts, verts.Length);
                return doubledVerts;
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
    static void BuildErrorMesh(ref Mesh mesh, float scale)
    {
        mesh.triangles = null;
        mesh.normals = null;
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
        List<int> indices = new List<int>();
        if (dimensions == 2)
        {
            // Point indices.
            if(meshType == MeshTopology.Points)
            {
                int vertCount = vertsPerAxis * vertsPerAxis;
                for(int i = 0; i < vertCount; i++)
                {
                    indices.Add(i);
                }
                return indices.ToArray();
            }
            // Wireframe indices.
            else if(meshType == MeshTopology.Lines)
            {
                for(int i = 0; i < vertsPerAxis; i++)
                {
                    for(int j = 0; j < vertsPerAxis; j++)
                    {
                        int vertIndex = vertsPerAxis * i + j;
                        if (j < vertsPerAxis - 1)
                        {
                            indices.Add(vertIndex);
                            indices.Add(vertIndex + 1);
                        }
                        if(i < vertsPerAxis - 1)
                        {
                            indices.Add(vertIndex);
                            indices.Add(vertIndex + vertsPerAxis);
                        }
                    }
                }
                return indices.ToArray();
            }
            // Quad indices.
            else if (meshType == MeshTopology.Quads)
            {
                int uniqueVertCount = vertsPerAxis * vertsPerAxis;
                for(int i = 0; i < vertsPerAxis - 1; i++)
                {
                    for(int j = 0; j < vertsPerAxis - 1; j++)
                    {
                        // Front face
                        int vertIndex = vertsPerAxis * i + j;
                        indices.Add(vertIndex);
                        indices.Add(vertIndex + vertsPerAxis);
                        indices.Add(vertIndex + vertsPerAxis + 1);
                        indices.Add(vertIndex + 1);
                        // Back face.
                        vertIndex += uniqueVertCount;
                        indices.Add(vertIndex);
                        indices.Add(vertIndex + 1);
                        indices.Add(vertIndex + 1 + vertsPerAxis);
                        indices.Add(vertIndex + vertsPerAxis);
                    }
                }
                return indices.ToArray();
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
