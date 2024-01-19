using System;
using System.Collections.Generic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using u040.prespective.prepair;
using u040.prespective.prepair.physics.kinetics.motor;
using UnityEngine;

namespace u040.prespective.prelogic.logiccontrollers.kinetics
{
    [AddComponentMenu("")]
    public class LEGACY_StepperMotorLogicController : PreLogicComponent
    {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
        [HideInInspector] public int ToolbarTab;
        [HideInInspector] public string CurrentTab;
#endif

        public LEGACY_StepperMotor StepperMotor;
        public bool DEBUG;

        //Inputs
        public float iAngularVelocity = 0f;
        public int iRotationSteps = 0;
        public float iRotationDegrees = 0f;
        public bool iError = false;

        //Input / output
        public float iPreferredAngularVelocityFactor = 0f;
        public float oPreferredAngularVelocityFactor = 0f;

        public int iTargetSteps = 0;
        public int oTargetSteps = 0;

        public float iTargetDegrees = 0f;
        public float oTargetDegrees = 0f;

        public bool iContinuousRotation = false;
        public bool oContinuousRotation = false;

        public bool iRotationDirection = false;
        public bool oRotationDirection = false;

        public bool iAutoStart = false;
        public bool oAutoStart = false;

        //Outputs
        public bool oStart = false;
        public bool oStop = false;
        public bool oResetError = false;
        public bool oResetZero = false;


        private void Reset()
        {
            this.signalNamingRuleOverrides[0].plcAddressPathFormat = "{{INST_NAME}}.{{IO_NAME}}";
            this.implicitNamingRule.instanceNameRule = "GVLs." + this.GetType().Name + "[{{INDEX_IN_PARENT}}]";
        }


        #region <<PLC Signals>>
        #region <<Signal Definitions>>
        /// <summary>
        /// Declare the IO signals
        /// </summary>
        public override List<SignalDefinition> SignalDefinitions
        {
            get
            {
                return new List<SignalDefinition>() {
                    ////Input Only
                    new SignalDefinition("iAngularVelocity", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, "", "Current angular velocity in degrees/s", null, null, 0f),
                    new SignalDefinition("iRotationSteps", PLCSignalDirection.INPUT, SupportedSignalType.INT32, "", "Current rotation in steps", null, null, 0),
                    new SignalDefinition("iRotationDegrees", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, "", "Current rotation in degrees", null, null, 0f), 
                    new SignalDefinition("iError", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", "Whether currently in error stop", null, null, false),

                    //Input / Outputs
                    new SignalDefinition("iPreferredAngularVelocityFactor", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, "", "Preferred angular velocity factor, percentage of maxAngularVelocity", null, null, 1f),
                    new SignalDefinition("oPreferredAngularVelocityFactor", PLCSignalDirection.OUTPUT, SupportedSignalType.REAL32, "", "Preferred angular velocity factor, percentage of maxAngularVelocity", onSignalChanged, null, 1f),

                    new SignalDefinition("iTargetSteps", PLCSignalDirection.INPUT, SupportedSignalType.INT32, "", "Rotate number of steps", null, null, 0),
                    new SignalDefinition("oTargetSteps", PLCSignalDirection.OUTPUT, SupportedSignalType.INT32, "", "Rotate number of steps", onSignalChanged, null, 0),

                    new SignalDefinition("iTargetDegrees", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, "", "Rotate number of degrees", null, null, 0f),
                    new SignalDefinition("oTargetDegrees", PLCSignalDirection.OUTPUT, SupportedSignalType.REAL32, "", "Rotate number of degrees", onSignalChanged, null, 0f),

                    new SignalDefinition("iContinuousRotation", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", "Toggle whether should rotate continuously", null, null, false),
                    new SignalDefinition("oContinuousRotation", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Toggle whether should rotate continuously", onSignalChanged, null, false),

                    new SignalDefinition("iRotationDirection", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", "Current rotation direction. TRUE = CW, FALSE = CCW.", null, null, true),
                    new SignalDefinition("oRotationDirection", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Current rotation direction. TRUE = CW, FALSE = CCW.", onSignalChanged, null, true),

                    new SignalDefinition("iAutoStart", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", "Toggle whether should start automatically", null, null, false),
                    new SignalDefinition("oAutoStart", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Toggle whether should start automatically", onSignalChanged, null, false),

                    ////Outputs only
                    new SignalDefinition("oStart", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Toggle whether should start if not at rotation point", onSignalChanged, null, false),
                    new SignalDefinition("oStop", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Toggle whether should stop immediately", onSignalChanged, null, false),
                    new SignalDefinition("oResetError", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Reset error", onSignalChanged, null, false),
                    new SignalDefinition("oResetZero", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Reset to zero position", onSignalChanged, null, false),
                };
            }
        }
        #endregion
        #region <<PLC Outputs>>
        /// <summary>
        /// General callback for the IOs
        /// </summary>
        /// <param name="_signal">the signal that has changed</param>
        /// <param name="_newValue">the new value</param>
        /// <param name="_newValueReceived">the time of the value change</param>
        /// <param name="_oldValue">the old value</param>
        /// <param name="_oldValueReceived">the time of the old value change</param>
        void onSignalChanged(SignalInstance _signal, object _newValue, DateTime _newValueReceived, object _oldValue, DateTime _oldValueReceived)
        {
            switch (_signal.definition.defaultSignalName)
            {
                case "oPreferredAngularVelocityFactor":
                    oPreferredAngularVelocityFactor = (float)_newValue;
                    StepperMotor.PreferredAngularVelocityFactor = oPreferredAngularVelocityFactor;
                    break;

                case "oTargetSteps":
                    oTargetSteps = (int)_newValue; //Log PLC output to editor
                    StepperMotor.TargetSteps = oTargetSteps;
                    break;

                case "oTargetDegrees":
                    oTargetDegrees = (float)_newValue;
                    StepperMotor.SetRotationTarget(oTargetDegrees, StepperMotor.RotateAbsolute);
                    break;

                case "oContinuousRotation":
                    oContinuousRotation = (bool)_newValue;
                    StepperMotor.ContinuousRotation = oContinuousRotation;
                    break;

                case "oRotationDirection":
                    oRotationDirection = (bool)_newValue;
                    StepperMotor.RotationDirection = oRotationDirection ? LEGACY_StepperMotor.Direction.CW : LEGACY_StepperMotor.Direction.CCW;
                    break;

                case "oAutoStart":
                    oAutoStart = (bool)_newValue;
                    StepperMotor.AutoStart = oAutoStart;
                    break;

                case "oStart":
                    oStart = (bool)_newValue;
                    if (oStart) { StepperMotor.StartRotation(); }
                    break;

                case "oStop":
                    oStop = (bool)_newValue;
                    if (oStop) { StepperMotor.StopRotation(); }
                    break;

                case "oResetError":
                    oResetError = (bool)_newValue;
                    if (oResetError) { StepperMotor.ResetError(); }
                    break;

                case "oResetZero":
                    oResetZero = (bool)_newValue;
                    if (oResetZero) { StepperMotor.ResetToZero(); }
                    break;

                default:
                    Debug.LogWarning("Unrecognized PLC output registered");
                    break;
            }
        }
        #endregion
        #endregion

        #region <<Update>>
        /// <summary>
        /// update the simulation component
        /// </summary>
        /// <param name="_simFrame">the current frame since start</param>
        /// <param name="_deltaTime">the time since last frame</param>
        /// <param name="_totalSimRunTime">total run time of the simulation</param>
        /// <param name="_simStart">the time the simulation started</param>
        protected override void onSimulatorUpdated(
            int _simFrame,
            float _deltaTime,
            float _totalSimRunTime,
            DateTime _simStart)
        {
            if (StepperMotor != null)
            {
                readComponent();
            }
            else
            {
                Debug.LogWarning("No StepperMotor has been assigned.");
            }
        }


        void readComponent()
        {
            if (StepperMotor.AngularVelocity != iAngularVelocity)
            {
                iAngularVelocity = StepperMotor.AngularVelocity;
                WriteValue("iAngularVelocity", iAngularVelocity);
            }

            if (StepperMotor.CurrentRotationSteps != iRotationSteps || StepperMotor.CurrentRotationDegrees != iRotationDegrees)
            {
                iRotationSteps = StepperMotor.CurrentRotationSteps;
                iRotationDegrees = StepperMotor.CurrentRotationDegrees;
                WriteValue("iRotationSteps", iRotationSteps);
                WriteValue("iRotationDegrees", iRotationDegrees);
            }

            if (StepperMotor.Error != iError)
            {
                iError = StepperMotor.Error;
                WriteValue("iError", iError);
            }

            if (StepperMotor.PreferredAngularVelocityFactor != iPreferredAngularVelocityFactor)
            {
                iPreferredAngularVelocityFactor = StepperMotor.PreferredAngularVelocityFactor;
                WriteValue("iPreferredAngularVelocityFactor", iPreferredAngularVelocityFactor);
            }

            if (StepperMotor.TargetSteps != iTargetSteps)
            {
                iTargetSteps = StepperMotor.TargetSteps;
                WriteValue("iTargetSteps", iTargetSteps);
            }

            if (StepperMotor.TargetDegrees != iTargetDegrees)
            {
                iTargetDegrees = StepperMotor.TargetDegrees;
                WriteValue("iTargetDegrees", iTargetDegrees);
            }

            if (StepperMotor.ContinuousRotation != iContinuousRotation)
            {
                iContinuousRotation = StepperMotor.ContinuousRotation;
                WriteValue("iContinuousRotation", iContinuousRotation);
            }

            if ((StepperMotor.RotationDirection == LEGACY_StepperMotor.Direction.CW && iRotationDirection == false) || (StepperMotor.RotationDirection == LEGACY_StepperMotor.Direction.CCW && iRotationDirection == true))
            {
                iRotationDirection = StepperMotor.RotationDirection == LEGACY_StepperMotor.Direction.CW ? true : false;
                WriteValue("iRotationDirection", iRotationDirection);
            }

            if (StepperMotor.AutoStart != iAutoStart)
            {
                iAutoStart = StepperMotor.AutoStart;
                WriteValue("iAutoStart", iAutoStart);
            }
        }
        #endregion
    }
}