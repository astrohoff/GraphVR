using UnityEngine;
using org.mariuszgromada.math.mxparser;

public class GraphingTester : MonoBehaviour {
    public bool refresh = false;
    public string function = "f(x) = x";
    public int segments = 1;
    public float boundDistance;
    public Color color;
    public bool twoSided = true;

    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRend;
    Material unlitMat;
    Material litMat;

	void Start () {
        mesh = new Mesh();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshRend = gameObject.AddComponent<MeshRenderer>();
        unlitMat = new Material(Shader.Find("Unlit/Color"));
        litMat = new Material(Shader.Find("Standard"));
        unlitMat.color = color;
        litMat.color = color;
        Function f = new Function(function);
        if(f.getArgumentsNumber() == 1) {
            meshRend.material = unlitMat;
        } else {
            meshRend.material = litMat;
        }
        BuildMesh();
    }
	
	void Update () {
        if (refresh) {
            BuildMesh();
            refresh = false;
        }
	}
    void BuildMesh() {
        Function f = new Function(function);
        if (f.checkSyntax()) {
            if(segments < 1) {
                segments = 1;
                Debug.Log("Segments must be >= 1");
            }
            if(f.getArgumentsNumber() == 1) {
                Vector3[] vertices = new Vector3[segments + 1];
                int[] indices = new int[vertices.Length];
                for (int i = 0; i < vertices.Length; i++) {
                    float x = -boundDistance + ((float)i / segments) * 2 * boundDistance;
                    float y = (float)f.calculate(x);
                    vertices[i] = new Vector3(x, y, 0);
                    indices[i] = i;
                }
                mesh.vertices = vertices;
                mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
                mesh.RecalculateBounds();
            }else if(f.getArgumentsNumber() == 2) {
                Vector3[] vertices = new Vector3[Mathf.RoundToInt(Mathf.Pow((segments + 1), 2))];
                for(int i = 0; i < segments + 1; i++) {
                    float y = -boundDistance + ((float)i / segments) * 2 * boundDistance;
                    for(int j = 0; j < segments + 1; j++) {
                        float x = -boundDistance + ((float)j / segments) * 2 * boundDistance;
                        float z = (float)f.calculate(x, y);
                        vertices[GetVertexIndex(j, i)] = new Vector3(x, y, z);                    }
                }
                int twoSidedMultiplier = 1;
                if (twoSided)
                    twoSidedMultiplier = 2;
                int[] indices = new int[Mathf.RoundToInt(Mathf.Pow(segments, 2)) * 2 * 3 * twoSidedMultiplier];
                int flipIndex = GetSegmentIndex(0, segments);
                for (int i = 0; i < segments; i++) {
                    for (int j = 0; j < segments; j++) {
                        int baseIndex = GetSegmentIndex(j, i);
                        indices[baseIndex] = GetVertexIndex(j, i);
                        indices[baseIndex + 1] = GetVertexIndex(j + 1, i + 1);
                        indices[baseIndex + 2] = GetVertexIndex(j, i + 1);
                        indices[baseIndex + 3] = indices[baseIndex];
                        indices[baseIndex + 4] = GetVertexIndex(j + 1, i);
                        indices[baseIndex + 5] = indices[baseIndex + 1];
                        if (twoSided) {
                            indices[baseIndex + flipIndex] = indices[baseIndex];
                            indices[baseIndex + 1 + flipIndex] = indices[baseIndex + 2];
                            indices[baseIndex + 2 + flipIndex] = indices[baseIndex + 1];
                            indices[baseIndex + 3 + flipIndex] = indices[baseIndex + 3];
                            indices[baseIndex + 4 + flipIndex] = indices[baseIndex + 5];
                            indices[baseIndex + 5 + flipIndex] = indices[baseIndex + 4];
                        }
                    }
                }
                mesh.vertices = vertices;
                mesh.triangles = indices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
            }
        } else {
            Debug.Log(f.getErrorMessage());
        }
    }
    int GetVertexIndex(int vertX, int vertY) {
        return (segments + 1) * vertY + vertX;
    }
    int GetSegmentIndex(int segmentX, int segmentY) {
        return segments * 6 * segmentY + 6 * segmentX;
    }
    int GetFactorial(int i) {
        int factorial = 0;
        for(int j = 0; j < i; j++) {
            factorial += Mathf.RoundToInt(Mathf.Pow(i, j));
        }
        return factorial;
    }
}
