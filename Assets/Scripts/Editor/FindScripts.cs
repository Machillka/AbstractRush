using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof())]
public class ScriptUsageFinder : Editor
{
    private List<GameObject> subscribers = new List<GameObject>();

    private void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Editor.
    }

    private void GetSubscribers()
    {
        subscribers.Clear();
    }
}
