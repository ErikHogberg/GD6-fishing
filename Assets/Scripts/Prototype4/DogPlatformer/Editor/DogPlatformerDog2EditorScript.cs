using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

 [CustomEditor(typeof(DogPlatformerDog2Script))]
public class DogPlatformerDog2EditorScript : Editor
{

    // public override void OnInspectorGUI()
    // {
    //     serializedObject.Update();
        
            
    //     serializedObject.ApplyModifiedProperties();
    // }

    public void OnSceneGUI()
    {
        var t = (target as DogPlatformerDog2Script);

        EditorGUI.BeginChangeCheck();
        Vector3 pos = Handles.PositionHandle(t.transform.TransformPoint(t.RaycastPos), Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move point");
            t.RaycastPos = t.transform.InverseTransformPoint(pos);
        }
    }
}
