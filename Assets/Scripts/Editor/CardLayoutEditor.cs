using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardLayout))]
public class CardLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardLayout layout = (CardLayout)target;

        if (GUILayout.Button("Apply Fan Layout"))
        {
            layout.ApplyFanLayout();
        }
    }
}
