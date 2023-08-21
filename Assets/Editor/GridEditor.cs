#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Grid), true)]
public class GridEditor : Editor
{
    SerializedProperty meshRendererProp;
    SerializedProperty gridWorldSizeProp;

    private void OnEnable()
    {
        meshRendererProp = serializedObject.FindProperty("meshRenderer");
        gridWorldSizeProp = serializedObject.FindProperty("gridWorldSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (meshRendererProp.objectReferenceValue == null)
        {
            EditorGUILayout.PropertyField(gridWorldSizeProp);
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}

#endif