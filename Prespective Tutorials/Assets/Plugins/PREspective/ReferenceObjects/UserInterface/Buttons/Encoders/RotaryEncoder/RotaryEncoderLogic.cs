﻿using System;
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using UnityEngine;

namespace u040.prespective.referenceobjects.userinterface.buttons.encoders
{
    public class RotaryEncoderLogic : PreLogicComponent
    {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
        [SerializeField] [Obfuscation(Exclude = true)] private int toolbarTab;
#endif

        public RotaryEncoder RotaryEncoder;

        public float iValue = 0f;

        private void Reset()
        {
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
                    //Inputs only
                    new SignalDefinition("iValue", PLCSignalDirection.INPUT, SupportedSignalType.INT32, "", "Value", null, null, 0f),
                };
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
        protected override void onSimulatorUpdated(int _simFrame, float _deltaTime, float _totalSimRunTime, DateTime _simStart)
        {
            if (RotaryEncoder.OutputSignal != iValue)
            {
                iValue = RotaryEncoder.OutputSignal;
                WriteValue("iValue", iValue);
            }
        }
        #endregion
    }
}