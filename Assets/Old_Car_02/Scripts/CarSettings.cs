using UnityEngine;
using System.Collections;

namespace CarPreviewPackage
{
    public enum CarParts
    {
        doorFrontLeft,
        doorFrontRight,
        hood,
        trunk,
        bumperFrontMain,
        suspension,
        bumperBackMain,
        doorFrontLeftHanlde,
        doorFrontRightHanlde,
        doorFrontLeftGlass,
        doorFrontRightGlass,
        frontcell,
        panel,
        steeringWheel,
        steeringWheel_Element,
        engine,
        lightBackLeft,        
        lightBackRight,
        lightFront_01,
        lightFront_02,
        plusticLogo,
        seatFrontRight1,
        wheelBackLeft,
        wheelBackLeftBolts,        
        wheelBackLeftDisk,
        wheelBackRight,
        wheelBackRightBolts,        
        wheelBackRightDisk,
        wheelFrontRight,
        wheelFrontRightBolts,        
        wheelFrontRightDisk,
        wheelFrontLeft,
        wheelFrontLeftBolts,        
        wheelFrontLeftDisk,
        windowBack,
        windshield,
        Body_01_Element_01,
        Mirror_01,
        Tank,
        Body_02,
        WipersLeft,
        WipersRight
    }

    /// <summary>
    /// Parts that can be openned and closed
    /// </summary>
    [System.Serializable]
    public class DynamicPart
    {
        public enum RotationPlane
        {
            x,
            y,
            z
        }

        public CarParts carPartType;
        [SerializeField] private Transform[] carPartTransforms;
        [Range(-360, 360)]
        [SerializeField] private float minRotationAngle;
        [Range(-360, 360)]
        [SerializeField] private float maxRotationAngle;
        [SerializeField] private RotationPlane rotationPlane;
        [HideInInspector] public float currentAngleStep;
        [HideInInspector] public Coroutine currentAnim;
        [HideInInspector] public bool isOpenned;

        /// <summary>
        /// Set Open/Close 
        /// </summary>
        /// <param name="rotation">0-100, where 0 is completely closed and a 100 - fully open</param>
        public void SetRotation(float rotation)
        {
            for (int i = 0; i < carPartTransforms.Length; i++)
            {
                float angle = Mathf.Lerp(minRotationAngle, maxRotationAngle, rotation / 100.0f);
                carPartTransforms[i].localEulerAngles = GetRotationVector(angle, i);
                currentAngleStep = rotation;
            }
        }

        private Vector3 GetRotationVector(float newAngle, int index)
        {
            Vector3 angle = carPartTransforms[index].localEulerAngles;
            switch (rotationPlane)
            {
                case RotationPlane.x:
                    angle.x = newAngle;
                    break;
                case RotationPlane.y:
                    angle.y = newAngle;
                    break;
                case RotationPlane.z:
                    angle.z = newAngle;
                    break;
            }

            return angle;
        }
    }

    /// <summary>
    /// Parts, that can be toggled on and off
    /// </summary>
    [System.Serializable]
    public class StaticPart
    {
        public CarParts carPartType;
        public GameObject carPartGO;
        [HideInInspector] public bool currentState;

        public void SetState(bool state)
        {
            currentState = state;
            carPartGO.SetActive(currentState);
        }

        public void ToggleState()
        {
            currentState = !currentState;
            carPartGO.SetActive(currentState);
        }
    }
   
    public class CarSettings : MonoBehaviour
    {
        [SerializeField] private DynamicPart[] dynamicParts;
        [SerializeField] private StaticPart[] staticParts;
    
        public DynamicPart[] DynamicParts
        {
            get { return dynamicParts; }
        }

        public StaticPart[] StaticParts
        {
            get { return staticParts; }
        }
    }
}