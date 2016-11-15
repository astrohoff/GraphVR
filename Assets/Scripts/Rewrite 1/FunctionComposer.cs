using UnityEngine;
using org.mariuszgromada.math.mxparser;

public class FunctionComposer : MonoBehaviour {
    public string function = "f(x,y)=0";
    string oldFunct;
    public GraphController graphCtrl;

	void Start () {
        graphCtrl.function = new Function(function);
        oldFunct = function;
	}
	
	void Update () {
	    if(oldFunct != function)
        {
            graphCtrl.function = new Function(function);
            oldFunct = function;
        }
	}
}
