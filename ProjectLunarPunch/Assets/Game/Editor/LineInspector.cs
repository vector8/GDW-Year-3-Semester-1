using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Line))] // use this on line types 
public class LineInspector : Editor
{

    // allows lines to be drawn in the Editor window
    private void OnSceneGUI()
    {
        Line line = target as Line; // draw target

        // convert points into world space
        Transform handleTransform = line.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity; // set and check rotation mode
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        Handles.color = Color.white; //set line color
        Handles.DrawLine(p0, p1); // draw the line using the points

        // allows movements of each point in the editor
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p0 = handleTransform.InverseTransformPoint(p0);
        }
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }

        
        

    }
}
