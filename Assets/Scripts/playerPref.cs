using System;
using System.Collections.Generic;
using UnityEngine;

public class playerPrefss : MonoBehaviour
{
    #region - Player -

    public enum PlayerStance
    {
        Stand,
        Crouch,
        Prone
    }
    [Serializable]
    public class PlayerSettings
    {
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInverted;
        public bool ViewYInverted;

        public bool RunningHold;
        public float MovementSmoothing;

        public float RunningSpeedF;
        public float RunningSpeedS;

        public float WalkingSpeedF;
        public float WalkingSpeedS;
        public float WalkingSpeedB;

        public float JumpingHeight;
        public float JumpingFalloff;

        public float FOVSensitivity = 5f;
    }
    [Serializable]
    public class CharacterStance
    {
        public float CameraHeight;
        public CapsuleCollider StanceCollider;
    }
    
    #endregion
}
