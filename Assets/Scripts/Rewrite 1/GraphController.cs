using UnityEngine;
using org.mariuszgromada.math.mxparser;
using System.Collections;

public class GraphController : MonoBehaviour {
    public float bound = 2;
    public int vertsPerAxis = 5;
    public MeshTopology meshType = MeshTopology.Points;
    Mesh functMesh;
    public Function function;
    float oldBound;
    int oldVertsPerAxis;
    MeshTopology oldMeshType;
    Function oldFunction;

    void Start () {
        functMesh = new Mesh();
        functMesh.vertices = new Vector3[] { Vector3.zero };
        GetComponent<MeshFilter>().mesh = functMesh;
        oldBound = bound;
        oldVertsPerAxis = vertsPerAxis;
        oldMeshType = meshType;
	}
	
    void Update()
    {
        if(oldBound != bound || oldVertsPerAxis != vertsPerAxis || oldMeshType != meshType || oldFunction != function)
        {
            oldBound = bound;
            oldVertsPerAxis = vertsPerAxis;
            oldMeshType = meshType;
            oldFunction = function;
            MeshBuilder.UpdateFunctionMesh(ref functMesh, function, bound, vertsPerAxis, meshType);
        }
    }
    
    public IEnumerator InitialUpdateFunction(Function newFunct)
    {
        while(functMesh == null)
        {
            Debug.Log("Mesh still null");
            yield return null;
        }
        //UpdateFunction(newFunct);
    }
}
