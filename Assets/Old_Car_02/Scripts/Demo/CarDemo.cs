using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPreviewPackage
{
    [RequireComponent(typeof(CarManager))]
    public class CarDemo : MonoBehaviour
    {
        //[Header("Open/Close")]

        [Range(0.0f, 100.0f)]
        public float frontLeftDoor; 

        [Range(0.0f, 100.0f)]
        public float frontRightDoor;

        [Range(0.0f, 100.0f)]
        public float hood;

        [Range(0.0f, 100.0f)]
        public float trunk;

       //[Header("Body")]
        public bool hoodState;
        public bool trunkState;
        public bool suspensionState;
        public bool Body_01_Element_01;
        public bool Body_02;

       //[Header("Fornt left door")]
        public bool frontLeftDoorState;
        public bool frontLeftDoorGlassState;
        public bool frontLeftDoorHandleState;

        //[Header("Front right door")]
        public bool frontRightDoorState;
        public bool frontRightDoorGlassState;
        public bool frontRightDoorHandleState;
                
        //[Header("Front bumper")]
        public bool bumperFront;        

        //[Header("Back bumper")]
        public bool bumperBack;        

        //[Header("LightBackLeft")]
        public bool lightBackLeft;
        
        //[Header("LightBacktRight")]
        public bool lightBackRight;
        
        //[Header("LightFrontLeft")]
        public bool lightFront_01;
        
        //[Header("LightFrontRight")]
        public bool lightFront_02;
        
        //[Header("Details")]
        public bool plasticLogo;        
        public bool frontcell;
        public bool Mirror_01;
        public bool Tank;
        public bool WipersLeft;
        public bool WipersRight;        

        //[Header("Seats")]        
        public bool seatFrontRight1;
        
        //[Header("Interior")]
        public bool steeringWheel;
        public bool steeringWheel_Element;
        public bool panel;

        //[Header("WheelsBackLeft")]
        public bool wheelBackLeft;
        public bool wheelBackLeftBolt;
        public bool wheelBackLeftDisk;

        //[Header("WheelsBackRight")]
        public bool wheelBackRight;
        public bool wheelBackRightBolt;        
        public bool wheelBackRightDisk;

        //[Header("WheelsFrontLeft")]
        public bool wheelFrontLeft;
        public bool wheelFrontLeftBolt;        
        public bool wheelFrontLeftDisk;

        //[Header("WheelsFrontRight")]
        public bool wheelFrontRight;
        public bool wheelFrontRightBolt;        
        public bool wheelFrontRightDisk;

        //[Header("Glass")]
        public bool windowBack;
        public bool windshield;

        //[Header("Engine")]
        public bool engine;

        private CarManager carManager;

        private void Start()
        {
            carManager = this.GetComponent<CarManager>();
        }

        public void UpdateData()
        {
            if (carManager && carManager.IsInitilized)
            {
                //Open/Close
                carManager.SetCarPartRotation(frontLeftDoor, CarParts.doorFrontLeft);
                carManager.SetCarPartRotation(frontRightDoor, CarParts.doorFrontRight);                
                carManager.SetCarPartRotation(hood, CarParts.hood);
                carManager.SetCarPartRotation(trunk, CarParts.trunk);

                //Bumper front
                carManager.SetCarPartState(bumperFront, CarParts.bumperFrontMain);                

                //Body
                carManager.SetCarPartState(trunkState, CarParts.trunk);
                carManager.SetCarPartState(hoodState, CarParts.hood);
                carManager.SetCarPartState(suspensionState, CarParts.suspension);
                carManager.SetCarPartState(Body_01_Element_01, CarParts.Body_01_Element_01);
                carManager.SetCarPartState(Body_02, CarParts.Body_02);

                //Bumper back
                carManager.SetCarPartState(bumperBack, CarParts.bumperBackMain);
                                
                //Door front left states
                carManager.SetCarPartState(frontLeftDoorState, CarParts.doorFrontLeft);
                carManager.SetCarPartState(frontLeftDoorGlassState, CarParts.doorFrontLeftGlass);
                carManager.SetCarPartState(frontLeftDoorHandleState, CarParts.doorFrontLeftHanlde);

                //Door front right states
                carManager.SetCarPartState(frontRightDoorState, CarParts.doorFrontRight);
                carManager.SetCarPartState(frontRightDoorGlassState, CarParts.doorFrontRightGlass);
                carManager.SetCarPartState(frontRightDoorHandleState, CarParts.doorFrontRightHanlde);

                //light back left
                carManager.SetCarPartState(lightBackLeft, CarParts.lightBackLeft);
                
                //LightBacktRight
                carManager.SetCarPartState(lightBackRight, CarParts.lightBackRight);
                
                //LightFrontLeft
                carManager.SetCarPartState(lightFront_01, CarParts.lightFront_01);
                
                //LightFrontRight
                carManager.SetCarPartState(lightFront_02, CarParts.lightFront_02);
                
                //Details
                carManager.SetCarPartState(plasticLogo, CarParts.plusticLogo);                
                carManager.SetCarPartState(frontcell, CarParts.frontcell);
                carManager.SetCarPartState(Mirror_01, CarParts.Mirror_01);
                carManager.SetCarPartState(Tank, CarParts.Tank);
                carManager.SetCarPartState(WipersLeft, CarParts.WipersLeft);
                carManager.SetCarPartState(WipersRight, CarParts.WipersRight);

                //Seats                
                carManager.SetCarPartState(seatFrontRight1, CarParts.seatFrontRight1);
                
                //Interior
                carManager.SetCarPartState(steeringWheel, CarParts.steeringWheel);
                carManager.SetCarPartState(steeringWheel_Element, CarParts.steeringWheel_Element); 
                carManager.SetCarPartState(panel, CarParts.panel);

                //WheelsBackLeft
                carManager.SetCarPartState(wheelBackLeft, CarParts.wheelBackLeft);
                carManager.SetCarPartState(wheelBackLeftBolt, CarParts.wheelBackLeftBolts);                
                carManager.SetCarPartState(wheelBackLeftDisk, CarParts.wheelBackLeftDisk);

                //WheelsBackRight
                carManager.SetCarPartState(wheelBackRight, CarParts.wheelBackRight);
                carManager.SetCarPartState(wheelBackRightBolt, CarParts.wheelBackRightBolts);                
                carManager.SetCarPartState(wheelBackRightDisk, CarParts.wheelBackRightDisk);

                //WheelsFrontLeft
                carManager.SetCarPartState(wheelFrontLeft, CarParts.wheelFrontLeft);
                carManager.SetCarPartState(wheelFrontLeftBolt, CarParts.wheelFrontLeftBolts);                
                carManager.SetCarPartState(wheelFrontLeftDisk, CarParts.wheelFrontLeftDisk);

                //WheelsFrontRight
                carManager.SetCarPartState(wheelFrontRight, CarParts.wheelFrontRight);
                carManager.SetCarPartState(wheelFrontRightBolt, CarParts.wheelFrontRightBolts);                
                carManager.SetCarPartState(wheelFrontRightDisk, CarParts.wheelFrontRightDisk);

                //Glass
                carManager.SetCarPartState(windowBack, CarParts.windowBack);
                carManager.SetCarPartState(windshield, CarParts.windshield);

                //Engine
                carManager.SetCarPartState(engine, CarParts.engine);
            }
            else
            {
                if (!Application.isPlaying)
                {
                    Start();
                    carManager.Awake();
                    UpdateData();
                }
            }
        }
    }
}
