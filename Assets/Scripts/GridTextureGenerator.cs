using UnityEngine;
using System.Collections;

public class GridTextureGenerator : MonoBehaviour {
    public int resolution = 16;
    public int lineWidth = 1;
    public int divisions = 2;
    public int repetitions = 3;
    public Color gridColor = Color.white;
    Material mat;


	void Start () {
        mat = GetComponent<MeshRenderer>().material;
	}
	
	void Regenerate() {
        Texture2D tex = new Texture2D(resolution, resolution);
        Color transparent = Color.white;
        transparent.a = 0;
        
    }
}
