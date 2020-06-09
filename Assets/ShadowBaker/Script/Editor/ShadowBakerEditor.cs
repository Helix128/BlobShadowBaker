using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShadowBakerEditor : EditorWindow
{
    public LayerMask ObjectLayer;
    public Transform Target;
    public int textureSize = 1024;
    public float OrthographicSize = 1;
    public float ShadowBlur;
    public bool includeChildren;


    [MenuItem("Tools/Shadow Baker")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ShadowBakerEditor window = (ShadowBakerEditor)EditorWindow.GetWindow(typeof(ShadowBakerEditor));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Target Object");
        Target = EditorGUILayout.ObjectField("Target", Target, typeof(Transform), true) as Transform;
      //  EditorGUILayout.LabelField("Object Layer");
        //ObjectLayer = EditorGUILayout.LayerField(ObjectLayer);
        EditorGUILayout.LabelField("Texture Size");
        textureSize = EditorGUILayout.IntField(textureSize);
        EditorGUILayout.LabelField("Orthographic Scale");
        OrthographicSize = EditorGUILayout.FloatField(OrthographicSize);
        EditorGUILayout.LabelField("Shadow Blur");
        ShadowBlur = EditorGUILayout.FloatField(ShadowBlur);
        EditorGUILayout.LabelField("Bake Children");
        includeChildren = EditorGUILayout.Toggle(includeChildren);
        if (GUILayout.Button("Bake"))
        {
            ShadowBaker.BakeShadow(ObjectLayer, Target, textureSize, OrthographicSize,ShadowBlur,includeChildren);
        }
    }
}
