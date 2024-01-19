﻿#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.prepair.physics.kinetics.motor;
using u040.prespective.utility.editor;
using System.Reflection;

namespace u040.prespective.referenceobjects.kinetics.motor.steppermotor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(DrivenStepperMotorLogic))]
    public class DrivenStepperMotorLogicEditor : PrespectiveEditor
    {
        private DrivenStepperMotorLogic component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;
        private SerializedProperty signalNamingRuleOverrides;
        private SerializedProperty implicitNamingRule;

        private void OnEnable()
        {
            component = (DrivenStepperMotorLogic)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
            signalNamingRuleOverrides = soTarget.FindProperty("signalNamingRuleOverrides");
            implicitNamingRule = soTarget.FindProperty("implicitNamingRule");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            if (component.StepperMotor == null)
            {
                EditorGUILayout.HelpBox("No DrivenStepperMotor Component has been set. This component will not function properly untill all required components have been assigned. You can do this in the Properties tab.", MessageType.Warning);
            }

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Properties", "I/O", "Settings", "Debugging" });

            switch (toolbarTab.intValue)
            {
                case 0:
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        if (component.StepperMotor == null)
                        {
                            EditorGUILayout.LabelField("No DrivenStepperMotor has been set so no properties can be shown.");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("DrivenStepperMotor", component.StepperMotor.ToString());
                        }
                    }
                    else
                    {
                        component.StepperMotor = (DrivenStepperMotor)EditorGUILayout.ObjectField("DrivenStepperMotor", component.StepperMotor, typeof(DrivenStepperMotor), true);
                    }
                    break;

                case 1:
                    //Header
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Property", "Input", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    //Inputs
                    EditorGUILayout.LabelField("Velocity", component.iVelocity.ToString());
                    EditorGUILayout.LabelField("Position (deg)", component.iPositionDeg.ToString());
                    EditorGUILayout.LabelField("Position (step)", component.iPositionStep.ToString());
                    EditorGUILayout.LabelField("Error", component.iError.ToString());

                    EditorGUILayout.Space();

                    //Outputs
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Preferred Velocity");
                    EditorGUILayout.LabelField(component.oPreferredVelocity.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target Step");
                    EditorGUILayout.LabelField(component.oTargetStep.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Continuous");
                    EditorGUILayout.LabelField(component.oContinuous.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Direction");
                    EditorGUILayout.LabelField(component.oDirection ? DrivenMotor.Direction.CW.ToString() : DrivenMotor.Direction.CCW.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Is Active");
                    EditorGUILayout.LabelField(component.oIsActive.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Reset Error");
                    EditorGUILayout.LabelField(component.oResetError.ToString());
                    EditorGUILayout.EndHorizontal();
                    break;

                case 2:
                    EditorGUILayout.PropertyField(signalNamingRuleOverrides, true);
                    EditorGUILayout.PropertyField(implicitNamingRule, true);

                    break;

                case 3:
                    component.VERBOSE = EditorGUILayout.Toggle("VERBOSE", component.VERBOSE);
                    component.UXShowSignalsForDebugging = EditorGUILayout.Toggle("UX Show Signals For Debugging", component.UXShowSignalsForDebugging);
                    component.UXTextOffset = EditorGUILayout.Vector2Field("UX Text Offset", component.UXTextOffset);
                    component.UXTextColor = EditorGUILayout.ColorField("UX Text Color", component.UXTextColor);
                    component.UXTextSize = EditorGUILayout.IntField("UX Text Size", component.UXTextSize);
                    component.UXTextLineSpacing = EditorGUILayout.IntField("UX Text Line Spacing", component.UXTextLineSpacing);
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }

    }
}
#endif