/**
 * Copyright (c) 2023-present Design X Development. All rights reserved.
 * 'DXD' can not be copied and/or distributed without the express permission of Design X Development.
 */

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(ObjectAnimator))]
public class ObjectAnimatorEditor : Editor
{
    SerializedProperty ActionEvent;

    private GUIStyle ToggleButtonStyleNormal = null;
    private GUIStyle ToggleButtonStyleToggled = null;
    private GUIStyle ToggleButtonStyleToggledStyleV2 = null;

    SerializedProperty isAnimated;

    SerializedProperty IsRotating,
        FloatingVector,
        RotationSpeed,
        RotationAngle,
        SpeedSelect,
        RandomRotationSpeedOne,
        RandomRotationSpeedTwo; // Rotation

    SerializedProperty ShakePosition, ShakeRotation, ShakeSpeed, ShakeDuration, ShakeCurve; // Shake 
    SerializedProperty PingPongSpeed, TravelDistance; //PingPong

    void OnEnable()
    {
        ActionEvent = serializedObject.FindProperty("ActionEvent");

        isAnimated = serializedObject.FindProperty("isAnimated");

        IsRotating = serializedObject.FindProperty("isRotating");
        FloatingVector = serializedObject.FindProperty("floatingVector");
        RotationSpeed = serializedObject.FindProperty("rotationSpeed");
        RotationAngle = serializedObject.FindProperty("rotationAngle");
        SpeedSelect = serializedObject.FindProperty("speedSelect");
        RandomRotationSpeedOne = serializedObject.FindProperty("randomRotationSpeedOne");
        RandomRotationSpeedTwo = serializedObject.FindProperty("randomRotationSpeedTwo");

        #region Shake

        ShakePosition = serializedObject.FindProperty("shakePosition");
        ShakeRotation = serializedObject.FindProperty("shakeRotation");
        ShakeSpeed = serializedObject.FindProperty("shakeSpeed");
        ShakeDuration = serializedObject.FindProperty("shakeDuration");
        ShakeCurve = serializedObject.FindProperty("shakeCurve");

        #endregion

        PingPongSpeed = serializedObject.FindProperty("pingPongSpeed");
        TravelDistance = serializedObject.FindProperty("travelDistance");
    }

    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();
        serializedObject.Update();

        var myScript = target as ObjectAnimator;

        if (ToggleButtonStyleNormal == null)
        {
            ToggleButtonStyleNormal = "Button";
            ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);

            ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
            // ToggleButtonStyleToggled.normal.background = Texture2D.grayTexture;
            ToggleButtonStyleToggled.fontStyle = FontStyle.Bold;
            ToggleButtonStyleToggled.fontSize = 13;
            ToggleButtonStyleToggled.normal.textColor = Color.yellow;

            ToggleButtonStyleToggledStyleV2 = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleToggledStyleV2.normal.background = ToggleButtonStyleToggledStyleV2.active.background;
            ToggleButtonStyleToggledStyleV2.fontStyle = FontStyle.Bold;
            ToggleButtonStyleToggledStyleV2.fontSize = 14;

            ToggleButtonStyleToggledStyleV2.normal.textColor = new Color(255f, 161f, 67f, 255f) / 255f;
        }

        var styleCenter = new GUIStyle(GUI.skin.label)
            {alignment = TextAnchor.MiddleCenter, fontSize = 14, fontStyle = FontStyle.Bold};
        styleCenter.normal.textColor = Color.yellow;

        // BUTTON
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Animated",
            myScript.isAnimated ? ToggleButtonStyleToggledStyleV2 : ToggleButtonStyleNormal))
            myScript.InitAnimated(myScript.isAnimated ? false : true);
        GUILayout.EndHorizontal();

        using (var groupA = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.isAnimated)))
        {
            if (groupA.visible == true)
            {
                #region Rotate

                // BUTTON
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Rotate",
                    myScript.isRotating ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
                    myScript.InitRotation(myScript.isRotating ? false : true);
                GUILayout.EndHorizontal();

                using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.isRotating)))
                {
                    if (group.visible == true)
                    {
                        EditorGUI.indentLevel++;

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(RotationAngle);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(SpeedSelect);
                        GUILayout.EndHorizontal();

                        switch (SpeedSelect.enumValueIndex)
                        {
                            case 0:
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.PropertyField(RandomRotationSpeedOne);
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                EditorGUILayout.PropertyField(RandomRotationSpeedTwo);
                                GUILayout.EndHorizontal();
                                break;

                            case 1:
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.PropertyField(RotationSpeed);
                                GUILayout.EndHorizontal();
                                break;
                        }

                        EditorGUILayout.LabelField("----", styleCenter, GUILayout.ExpandWidth(true));
                        EditorGUI.indentLevel--;
                    }
                }

                #endregion

                #region Shake

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Shake",
                    myScript.isShake ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
                    myScript.InitShake(myScript.isShake ? false : true);
                GUILayout.EndHorizontal();

                using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.isShake)))
                {
                    if (group.visible == true)
                    {
                        EditorGUI.indentLevel++;

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(ShakePosition);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(ShakeRotation);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(ShakeSpeed);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(ShakeCurve);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(ShakeDuration);
                        GUILayout.EndHorizontal();

                        EditorGUILayout.LabelField("**", styleCenter, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("Shake Now"))
                            myScript.ShakeNow();

                        EditorGUILayout.LabelField("----", styleCenter, GUILayout.ExpandWidth(true));
                        EditorGUI.indentLevel--;
                    }
                }

                #endregion

                #region Ping Pong - Position

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Ping Pong - Position",
                    myScript.isPingPongPosition ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
                    myScript.InitPingPongPosition(myScript.isPingPongPosition ? false : true);
                GUILayout.EndHorizontal();

                using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.isPingPongPosition)))
                {
                    if (group.visible == true)
                    {
                        EditorGUI.indentLevel++;

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(PingPongSpeed);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Set Start Pos"))
                            myScript.SetPingPongStartPos();
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Set End Pos"))
                            myScript.SetPingPongEndPos();
                        GUILayout.EndHorizontal();

                        EditorGUILayout.LabelField("----", styleCenter, GUILayout.ExpandWidth(true));
                        EditorGUI.indentLevel--;
                    }
                }

                #endregion

            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(myScript);
    }
}
#endif