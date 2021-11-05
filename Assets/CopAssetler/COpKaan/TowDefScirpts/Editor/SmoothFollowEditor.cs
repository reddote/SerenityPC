using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SmoothFollow))]
public class SmoothFollowEditor : Editor
{
    SmoothFollow smoothFollow;

    void OnEnable()
    {
        smoothFollow = (SmoothFollow)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        smoothFollow.protectFromObstacles = EditorGUILayout.Toggle("Protect From Obstacles", smoothFollow.protectFromObstacles);

        if (smoothFollow.protectFromObstacles)
        {
            smoothFollow.protectionRange = EditorGUILayout.Slider(new GUIContent("Protection Range"), smoothFollow.protectionRange, 0.1f, 10f);
            smoothFollow.castRadius = EditorGUILayout.Slider(new GUIContent("Cast Radius"), smoothFollow.castRadius, 0.1f, 2f);
        }
    }
}