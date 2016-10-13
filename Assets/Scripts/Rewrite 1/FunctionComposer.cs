using UnityEngine;
using org.mariuszgromada.math.mxparser;

public class FunctionComposer : MonoBehaviour {
    public string function = "f(x,y)=0";
    string oldFunct;
    GraphController graphCtrl;

	void Start () {
        graphCtrl = GetComponent<GraphController>();
        StartCoroutine(graphCtrl.InitialUpdateFunction(new Function(function)));
        oldFunct = function;
	}
	
	void Update () {
	    if(oldFunct != function)
        {
            oldFunct = function;
            graphCtrl.UpdateFunction(new Function(function));
        }
	}
}
