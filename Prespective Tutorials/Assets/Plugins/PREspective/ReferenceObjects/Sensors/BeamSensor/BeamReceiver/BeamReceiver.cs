using System.Reflection;
using u040.prespective.prepair.inspector;
using UnityEngine;
using u040.prespective.prepair.components.sensors;


namespace u040.prespective.referenceobjects.sensors.beamsensor
{
    public class BeamReceiver : QuantitativeSensor, IBeamTarget, IControlPanel
    {
        [SerializeField] [Obfuscation(Exclude = true)] private int toolbarTab;

        public BeamPathRedirectionPoint resolveHit(Vector3 _hitVector, RaycastHit hit)
        {
            this.Flagged = true;
            return new BeamPathRedirectionPoint(this, hit.point, hit.point, Vector3.zero); //Vector3.zero means absorb ray
        }

        public void lostHit()
        {
            this.Flagged = false;
        }

        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            ShowQuantitativeSensorValuesInspector(false);
#endif
        }
    }
}