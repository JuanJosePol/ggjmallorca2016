using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Jammer))]
public class JammerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Jammer jammer = (Jammer)target;

        if (GUILayout.Button("Go To Bathroom"))
        {
            jammer.GoToBathroom();
        }
    }
}
