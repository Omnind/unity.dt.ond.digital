#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using System;
using u040.prespective.prepair.inspector;
using UnityEngine;


namespace u040.prespective.prepair.physics.kinetics.motor
{
    [AddComponentMenu("")]
    public class LEGACY_StepperMotor : LEGACY_MotorComponent, IControlPanel
    {

        /// <summary>
        /// @CLASS : StepperMotor
        /// 
        /// @ABOUT : Represents a generic stepper motor
        /// 
        /// @AUTHOR: Mathijs
        /// 
        /// @VERSION: 21/08/2019 - V1.00 - Implemented alpha
        /// </summary>


        [Range(24f, 1000f)] public int MotorStepCount = 200; //Number of steps per full rotation
        [SerializeField] public bool enableHalfStepping = false;
        

        public int CurrentRotationSteps
        {
            get { return (int)Math.Round(CurrentRotationDegrees / (360f / MotorStepCount)); }
            set { CurrentRotationDegrees = (float)value * (360f / MotorStepCount); }
        }

        public int TargetSteps
        {
            get { return (int)Math.Round(TargetDegrees / (360f / (float)(enableHalfStepping ? MotorStepCount * 2 : MotorStepCount))); }
            set { TargetDegrees = (float)value * (360f / (float)(enableHalfStepping ? MotorStepCount * 2 : MotorStepCount)); }
        }

        public override void StopRotation()
        {
            base.StopRotation();
            //FIXME: Stopping doesn't snap to steps yet
        }


        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            //Editable values
            PreferredAngularVelocityFactor = EditorGUILayout.Slider("Preferred Angular Velocity Factor ", PreferredAngularVelocityFactor, 0f, 1f);
            TargetSteps = EditorGUILayout.DelayedIntField(new GUIContent("Rotation Target", "in steps"), TargetSteps);
            ContinuousRotation = EditorGUILayout.Toggle("Continuous Rotation", ContinuousRotation);
            RotationDirection = (LEGACY_MotorComponent.Direction)EditorGUILayout.EnumPopup("Direction", RotationDirection);
            AutoStart = EditorGUILayout.Toggle(new GUIContent("Start Automatically", "Start rotation as soon as the motor is not at its rotation target"), AutoStart);

            //ReadOnly values
            EditorGUILayout.LabelField(new GUIContent("Current angular velocity", "in degrees per second"), new GUIContent(AngularVelocity.ToString()));
            EditorGUILayout.LabelField(new GUIContent("Current rotation", "in degrees"), new GUIContent(CurrentRotationDegrees.ToString()));
            EditorGUILayout.LabelField("Current step", CurrentRotationSteps.ToString());
            EditorGUILayout.LabelField("Current rotation state", CurrentRotationState.ToString());
            EditorGUILayout.LabelField("Error", Error.ToString());

            //Buttons
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start"))
            {
                StartRotation();
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Stop"))
            {
                StopRotation();
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Error"))
            {
                ErrorStop();
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Reset Error"))
            {
                ResetError();
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Reset Zero"))
            {
                ResetToZero();
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
#endif
        }
    }
}