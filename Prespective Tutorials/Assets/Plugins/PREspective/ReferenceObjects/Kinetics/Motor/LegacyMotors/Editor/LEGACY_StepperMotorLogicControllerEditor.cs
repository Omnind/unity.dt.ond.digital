#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.prepair.physics.kinetics.motor;
using u040.prespective.utility.editor;
using System.Reflection;

namespace u040.prespective.prelogic.logiccontrollers.kinetics.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(LEGACY_StepperMotorLogicController))]
    public class LEGACY_StepperMotorLogicControllerEditor : PrespectiveEditor
    {
        private LEGACY_StepperMotorLogicController component;
        private SerializedObject soTarget;
        private SerializedProperty signalNamingRuleOverrides;
        private SerializedProperty implicitNamingRule;


        private void OnEnable()
        {
            component = (LEGACY_StepperMotorLogicController)target;
            soTarget = new SerializedObject(target);
            signalNamingRuleOverrides = soTarget.FindProperty("signalNamingRuleOverrides");
            implicitNamingRule = soTarget.FindProperty("implicitNamingRule");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            if (component.StepperMotor == null)
            {
                EditorGUILayout.HelpBox("No Stepper Motor Component has been set. This component will not function properly untill all required components have been assigned. You can do this in the Properties tab.", MessageType.Warning);
            }

            EditorGUI.BeginChangeCheck();
            component.ToolbarTab = GUILayout.Toolbar(component.ToolbarTab, new string[] { "Properties", "PLC outputs", "PLC settings", "Debugging" });

            switch (component.ToolbarTab)
            {
                case 0:
                    component.CurrentTab = "Properties";
                    break;

                case 1:
                    component.CurrentTab = "PLC outputs";
                    break;

                case 2:
                    component.CurrentTab = "PLC settings";
                    break;

                case 3:
                    component.CurrentTab = "Debugging";
                    break;

            }
            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }

            EditorGUI.BeginChangeCheck();
            switch (component.CurrentTab)
            {
                case "Properties":
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        if (component.StepperMotor == null)
                        {
                            EditorGUILayout.LabelField("No stepper motor has been set so no properties can be shown.");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Stepper Motor", component.StepperMotor.ToString());
                        }
                    }
                    else
                    {
                        component.StepperMotor = (LEGACY_StepperMotor)EditorGUILayout.ObjectField("Stepper Motor", component.StepperMotor, typeof(LEGACY_StepperMotor), true);
                    }
                    break;

                case "PLC outputs":
                    //Header
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Property", "Input", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    //Inputs
                    EditorGUILayout.LabelField("AngularVelocity", component.iAngularVelocity.ToString());
                    EditorGUILayout.LabelField("RotationSteps", component.iRotationSteps.ToString());
                    EditorGUILayout.LabelField("RotationDegrees", component.iRotationDegrees.ToString());
                    EditorGUILayout.LabelField("Error", component.iError.ToString());

                    EditorGUILayout.Space();

                    //Input / output
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("PreferredAngularVelocityFactor", component.iPreferredAngularVelocityFactor.ToString());
                    EditorGUILayout.LabelField(component.oPreferredAngularVelocityFactor.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("TargetSteps", component.iTargetSteps.ToString());
                    EditorGUILayout.LabelField(component.oTargetSteps.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("TargetDegrees", component.iTargetDegrees.ToString());
                    EditorGUILayout.LabelField(component.oTargetDegrees.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("ContinuousRotation", component.iContinuousRotation.ToString());
                    EditorGUILayout.LabelField(component.oContinuousRotation.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("RotationDirection", component.iRotationDirection.ToString());
                    EditorGUILayout.LabelField(component.oRotationDirection.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("AutoStart", component.iAutoStart.ToString());
                    EditorGUILayout.LabelField(component.oAutoStart.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    //Outputs
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Start");
                    EditorGUILayout.LabelField(component.oStart.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Stop");
                    EditorGUILayout.LabelField(component.oStop.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("ResetError");
                    EditorGUILayout.LabelField(component.oResetError.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("ResetZero");
                    EditorGUILayout.LabelField(component.oResetZero.ToString());
                    EditorGUILayout.EndHorizontal();
                    break;

                case "PLC settings":
                    EditorGUILayout.PropertyField(signalNamingRuleOverrides, true);
                    EditorGUILayout.PropertyField(implicitNamingRule, true);

                    break;

                case "Debugging":
                    component.DEBUG = EditorGUILayout.Toggle("Show Debug Logs", component.DEBUG);
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