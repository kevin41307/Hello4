using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Cube cube = (Cube) target;
        if(GUILayout.Button("generate color"))
        {
            Debug.Log("generate color");
            cube.GenerateColor();
        }
    }

    

}
