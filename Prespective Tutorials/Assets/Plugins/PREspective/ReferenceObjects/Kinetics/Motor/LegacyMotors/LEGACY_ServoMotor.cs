#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using u040.prespective.prepair.inspector;
using UnityEngine;

namespace u040.prespective.prepair.physics.kinetics.motor
{
    [AddComponentMenu("")]
    public class LEGACY_ServoMotor : LEGACY_MotorComponent, IControlPanel
    {
        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            //Editable values
            PreferredAngularVelocityFactor = EditorGUILayout.Slider("Preferred Angular Velocity Factor ", PreferredAngularVelocityFactor, 0f, 1f);
            TargetDegrees = EditorGUILayout.DelayedFloatField("Rotation Target", TargetDegrees);
            ContinuousRotation = EditorGUILayout.Toggle("Continuous Rotation", ContinuousRotation);
            RotationDirection = (LEGACY_MotorComponent.Direction)EditorGUILayout.EnumPopup("Direction", RotationDirection);
            AutoStart = EditorGUILayout.Toggle(new GUIContent("Start Automatically", "Start rotation as soon as the motor is not at its rotation target"), AutoStart);

            //ReadOnly values
            EditorGUILayout.LabelField(new GUIContent("Current angular velocity", "in degrees per second"), new GUIContent(AngularVelocity.ToString()));
            EditorGUILayout.LabelField(new GUIContent("Current rotation", "in degrees"), new GUIContent(CurrentRotationDegrees.ToString()));
            EditorGUILayout.LabelField("Current rotation state", CurrentRotationState.ToString());
            EditorGUILayout.LabelField("Error", Error.ToString());


            //Buttons
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start"))
            {
                StartRotation();
            }
            if (GUILayout.Button("Stop"))
            {
                StopRotation();
            }
            if (GUILayout.Button("Error"))
            {
                ErrorStop();
            }
            if (GUILayout.Button("Reset Error"))
            {
                ResetError();
            }
            if (GUILayout.Button("Reset Zero"))
            {
                ResetToZero();
            }
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
#endif
        }
    }
}
