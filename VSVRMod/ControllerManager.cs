using UnityEngine;

namespace VSVRMod
{
    public class ControllerManager
    {
        public Vector2 prevLeftAxisValue = Vector2.zero;
        public Vector2 prevRightAxisValue = Vector2.zero;

        public bool prevLeftAxisClickValue = false;
        public bool prevRightAxisClickValue = false;

        public bool prevLeftPrimaryValue = false;
        public bool prevRightPrimaryValue = false;

        public bool prevLeftSecondaryValue = false;
        public bool prevRightSecondaryValue = false;

        public bool prevLeftTriggerValue = false;
        public bool prevRightTriggerValue = false;

        public bool prevLeftGripValue = false;
        public bool prevRightGripValue = false;
    }
}
