#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.prepair.kinematics;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.prepair.physics.kinetics.motor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(LEGACY_StepperMotor))]
    public class LEGACY_StepperMotorEditor : PrespectiveEditor
    {
        private LEGACY_StepperMotor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (LEGACY_StepperMotor)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            if (component.targetWheelJoint == null)
            {
                EditorGUILayout.HelpBox("No Wheel Joint has been set. This component will not function properly untill all required components have been assigned. You can do this in the Properties tab.", MessageType.Warning);
            }

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Live Data", "Properties", "Debugging"});

            switch (toolbarTab.intValue)
            {
                case 0: //Live Data
                    EditorGUI.BeginDisabledGroup(true);
                    component.ShowControlPanel();
                    EditorGUI.EndDisabledGroup();
                    //EditorGUILayout.LabelField(new GUIContent("Preferred Angular Velocity Factor"), new GUIContent((component.PreferredAngularVelocityFactor * 100f) + "% (" + (component.PreferredAngularVelocityFactor * component.MaxAngularVelocity) + ")"));
                    //EditorGUILayout.LabelField(new GUIContent("Rotation Target", "in steps"), new GUIContent(component.TargetSteps.ToString()));
                    //EditorGUILayout.LabelField(new GUIContent("Continuous Rotation"), new GUIContent(component.ContinuousRotation.ToString()));
                    //EditorGUILayout.LabelField(new GUIContent("Direction"), new GUIContent(component.RotationDirection.ToString()));
                    //EditorGUILayout.LabelField(new GUIContent("Start Automatically", "Start rotation as soon as the motor is not at its rotation target"), new GUIContent(component.AutoStart.ToString()));
                    //EditorGUILayout.LabelField(new GUIContent(), new GUIContent());
  
                    ////component.RotationDirection = (MotorComponent.Direction)EditorGUILayout.EnumPopup("Direction", component.RotationDirection);

                    ////ReadOnly values
                    //EditorGUILayout.LabelField(new GUIContent("Current angular velocity", "in degrees per second"), new GUIContent(component.AngularVelocity.ToString()));
                    //EditorGUILayout.LabelField(new GUIContent("Current rotation", "in degrees"), new GUIContent(component.CurrentRotationDegrees.ToString()));
                    //EditorGUILayout.LabelField("Current step", component.CurrentRotationSteps.ToString());
                    //EditorGUILayout.LabelField("Current rotation state", component.CurrentRotationState.ToString());
                    //EditorGUILayout.LabelField("Error", component.Error.ToString());
                    break;

                case 1: //Properties
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        if (component.targetWheelJoint == null)
                        {
                            EditorGUILayout.LabelField("No target wheel joint has been set so no properties can be shown.");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Target Wheel Joint", component.targetWheelJoint.ToString());
                            EditorGUILayout.LabelField(new GUIContent("Motor Step Count", "Total number of steps in a full rotation"), new GUIContent(component.MotorStepCount.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Enable Half-stepping", "Double the number of steps"), new GUIContent(component.enableHalfStepping.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Maximum angular velocity", "Degrees per second"), new GUIContent(component.MaxAngularVelocity.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Angular Acceleration", "Degrees per second"), new GUIContent(component.AngularAcceleration.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Offset", "Offset in degrees"), new GUIContent(component.Offset.ToString()));
                        }
                    }
                    else
                    {
                        component.targetWheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Target Wheel Joint", component.targetWheelJoint, typeof(WheelJoint), true);
                        component.MotorStepCount = EditorGUILayout.DelayedIntField(new GUIContent("Motor Step Count", "Total number of steps in a full rotation"), component.MotorStepCount);
                        component.enableHalfStepping = EditorGUILayout.Toggle(new GUIContent("Enable Half-stepping", "Double the number of steps"), component.enableHalfStepping);
                        component.MaxAngularVelocity = EditorGUILayout.DelayedFloatField(new GUIContent("Maximum angular velocity", "Degrees per second"), component.MaxAngularVelocity);
                        component.AngularAcceleration = EditorGUILayout.DelayedFloatField(new GUIContent("Angular Acceleration", "Degrees per second"), component.AngularAcceleration);
                        component.Offset = EditorGUILayout.DelayedFloatField(new GUIContent("Offset", "Offset in degrees"), component.Offset);
                        if (GUILayout.Button("Set Current Position As Zero"))
                        {
                            component.MatchWheelJointStartingPosition();
                        }
                    }
                    break;

                case 2: //Debugging
                    component.DEBUG = EditorGUILayout.Toggle("Show Debug Logs", component.DEBUG);

                    ControlPanelInterface.ShowGenerationButtonForComponent(component);
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