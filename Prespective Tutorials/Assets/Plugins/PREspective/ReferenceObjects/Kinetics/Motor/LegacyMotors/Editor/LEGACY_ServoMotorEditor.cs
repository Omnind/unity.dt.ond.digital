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
    [CustomEditor(typeof(LEGACY_ServoMotor))]
    public class LEGACY_ServoMotorEditor : PrespectiveEditor
    {
        private LEGACY_ServoMotor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (LEGACY_ServoMotor)target;
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
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Live Data", "Properties", "Control Panel" });

            switch (toolbarTab.intValue)
            {
                case 0:
                    EditorGUI.BeginDisabledGroup(true);
                    component.ShowControlPanel();
                    EditorGUI.EndDisabledGroup();
                    break;

                case 1:
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        if (component.targetWheelJoint == null)
                        {
                            EditorGUILayout.LabelField("No target wheel joint has been set so no properties can be shown.");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Target Wheel Joint", component.targetWheelJoint.ToString());
                            EditorGUILayout.LabelField(new GUIContent("Maximum angular velocity", "Degrees per second"), new GUIContent(component.MaxAngularVelocity.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Angular Acceleration", "Degrees per second"), new GUIContent(component.AngularAcceleration.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Offset", "Offset in degrees"), new GUIContent(component.Offset.ToString()));
                        }
                    }
                    else
                    {
                        component.targetWheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Target Wheel Joint", component.targetWheelJoint, typeof(WheelJoint), true);
                        component.MaxAngularVelocity = EditorGUILayout.DelayedFloatField(new GUIContent("Maximum angular velocity", "Degrees per second"), component.MaxAngularVelocity);
                        component.AngularAcceleration = EditorGUILayout.DelayedFloatField(new GUIContent("Angular Acceleration", "Degrees per second"), component.AngularAcceleration);
                        component.Offset = EditorGUILayout.DelayedFloatField(new GUIContent("Offset", "Offset in degrees"), component.Offset);
                        if (GUILayout.Button("Set Current Position As Zero"))
                        {
                            component.MatchWheelJointStartingPosition();
                        }
                    }
                    break;

                case 2:
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