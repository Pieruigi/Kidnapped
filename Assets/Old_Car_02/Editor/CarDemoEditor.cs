using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CarPreviewPackage
{
    [CustomEditor(typeof(CarDemo))]
    public class CarDemoEditor : Editor
    {
        CarDemo car;

        SerializedProperty frontLeftDoor, frontRightDoor, hood, trunk, hoodState, trunkState, suspensionState,
            frontLeftDoorState, frontLeftDoorGlassState, frontLeftDoorHandleState, frontRightDoorState, frontRightDoorGlassState, frontRightDoorHandleState,
            bumperFront, bumperBack, frontcell, lightBackLeft, lightBackRight, panel, seatFrontRight1, steeringWheel, steeringWheel_Element, wheelBackLeft,
            wheelBackLeftBolt, wheelBackLeftDisk, wheelBackRight, wheelBackRightBolt, wheelBackRightDisk, wheelFrontLeft, Logo,
            wheelFrontLeftBolt, wheelFrontLeftDisk, wheelFrontRight, wheelFrontRightBolt, wheelFrontRightDisk, windowBack, windshield, engine, lightFront_01, lightFront_02,
            body_01_Element_01, body_02, mirror_01, tank, wipersLeft, wipersRight;

        bool openCloseHeader, bodyHeader, SuspensionHeader, frontLeftDoorHeader, frontRightDoorHeader, bumperFrontHeader, bumperBackHeader,
            lightBackLeftHeader, lightBackRightHeader, detailsHeader, seatsHeader, interiorHeader, wheelBackLeftHeader,
            wheelBackRightHeader, wheelFrontLeftHeader, wheelFrontRightHeader, glassHeader, engineHeader, frontcellHeader, hoodHeader;

        GUIStyle headerStyle;

        SerializedProperty[] bodyChildrenProps, frontLeftDoorChildrenProps, frontRightDoorChildrenProps, bumperFrontChildrenProps,
            bumperBackChildrenProps, detailsProps, seatsProps, bodyChildrenProps_2,
            interiorProps, wheelBackLeftChildrenProps, wheelBackRightChildrenProps, wheelFrontLeftChildrenProps, wheelFrontRightChildrenProps, glassProps, engineProps, LightFrontChildrenProps, HoodProps;

        private void OnEnable()
        {
            car = target as CarDemo;

            frontLeftDoor = serializedObject.FindProperty(nameof(CarDemo.frontLeftDoor));
            frontRightDoor = serializedObject.FindProperty(nameof(CarDemo.frontRightDoor));
            hood = serializedObject.FindProperty(nameof(CarDemo.hood));
            trunk = serializedObject.FindProperty(nameof(CarDemo.trunk));
            hoodState = serializedObject.FindProperty(nameof(CarDemo.hoodState));
            trunkState = serializedObject.FindProperty(nameof(CarDemo.trunkState));
            suspensionState = serializedObject.FindProperty(nameof(CarDemo.suspensionState));
            frontLeftDoorState = serializedObject.FindProperty(nameof(CarDemo.frontLeftDoorState));
            frontLeftDoorGlassState = serializedObject.FindProperty(nameof(CarDemo.frontLeftDoorGlassState));
            frontLeftDoorHandleState = serializedObject.FindProperty(nameof(CarDemo.frontLeftDoorHandleState));
            frontRightDoorState = serializedObject.FindProperty(nameof(CarDemo.frontRightDoorState));
            frontRightDoorGlassState = serializedObject.FindProperty(nameof(CarDemo.frontRightDoorGlassState));
            frontRightDoorHandleState = serializedObject.FindProperty(nameof(CarDemo.frontRightDoorHandleState));
            bumperFront = serializedObject.FindProperty(nameof(CarDemo.bumperFront));
            bumperBack = serializedObject.FindProperty(nameof(CarDemo.bumperBack));
            frontcell = serializedObject.FindProperty(nameof(CarDemo.frontcell));
            lightBackLeft = serializedObject.FindProperty(nameof(CarDemo.lightBackLeft));
            lightBackRight = serializedObject.FindProperty(nameof(CarDemo.lightBackRight));
            panel = serializedObject.FindProperty(nameof(CarDemo.panel));
            seatFrontRight1 = serializedObject.FindProperty(nameof(CarDemo.seatFrontRight1));
            steeringWheel = serializedObject.FindProperty(nameof(CarDemo.steeringWheel));
            wheelBackLeft = serializedObject.FindProperty(nameof(CarDemo.wheelBackLeft));
            wheelBackLeftBolt = serializedObject.FindProperty(nameof(CarDemo.wheelBackLeftBolt));
            wheelBackLeftDisk = serializedObject.FindProperty(nameof(CarDemo.wheelBackLeftDisk));
            wheelBackRight = serializedObject.FindProperty(nameof(CarDemo.wheelBackRight));
            wheelBackRightBolt = serializedObject.FindProperty(nameof(CarDemo.wheelBackRightBolt));
            wheelBackRightDisk = serializedObject.FindProperty(nameof(CarDemo.wheelBackRightDisk));
            wheelFrontLeft = serializedObject.FindProperty(nameof(CarDemo.wheelFrontLeft));
            wheelFrontLeftBolt = serializedObject.FindProperty(nameof(CarDemo.wheelFrontLeftBolt));
            wheelFrontLeftDisk = serializedObject.FindProperty(nameof(CarDemo.wheelFrontLeftDisk));
            wheelFrontRight = serializedObject.FindProperty(nameof(CarDemo.wheelFrontRight));
            wheelFrontRightBolt = serializedObject.FindProperty(nameof(CarDemo.wheelFrontRightBolt));
            wheelFrontRightDisk = serializedObject.FindProperty(nameof(CarDemo.wheelFrontRightDisk));
            windowBack = serializedObject.FindProperty(nameof(CarDemo.windowBack));
            windshield = serializedObject.FindProperty(nameof(CarDemo.windshield));
            engine = serializedObject.FindProperty(nameof(CarDemo.engine));            
            lightFront_01 = serializedObject.FindProperty(nameof(CarDemo.lightFront_01));
            lightFront_02 = serializedObject.FindProperty(nameof(CarDemo.lightFront_02));
            body_01_Element_01 = serializedObject.FindProperty(nameof(CarDemo.Body_01_Element_01));
            body_02 = serializedObject.FindProperty(nameof(CarDemo.Body_02));
            mirror_01 = serializedObject.FindProperty(nameof(CarDemo.Mirror_01));
            tank = serializedObject.FindProperty(nameof(CarDemo.Tank));
            wipersLeft = serializedObject.FindProperty(nameof(CarDemo.WipersLeft));
            wipersRight = serializedObject.FindProperty(nameof(CarDemo.WipersRight));
            steeringWheel_Element = serializedObject.FindProperty(nameof(CarDemo.steeringWheel_Element));
            Logo = serializedObject.FindProperty(nameof(CarDemo.plasticLogo));


            bodyChildrenProps = new SerializedProperty[1] { suspensionState };
            frontLeftDoorChildrenProps = new SerializedProperty[2] { frontLeftDoorGlassState, frontLeftDoorHandleState };
            frontRightDoorChildrenProps = new SerializedProperty[2] { frontRightDoorGlassState, frontRightDoorHandleState };
            detailsProps = new SerializedProperty[4] { mirror_01, tank, wipersLeft, wipersRight };
            seatsProps = new SerializedProperty[1] { seatFrontRight1 };
            interiorProps = new SerializedProperty[3] { steeringWheel, steeringWheel_Element, panel };
            wheelBackLeftChildrenProps = new SerializedProperty[2] { wheelBackLeftBolt, wheelBackLeftDisk };
            wheelBackRightChildrenProps = new SerializedProperty[2] { wheelBackRightBolt, wheelBackRightDisk };
            wheelFrontRightChildrenProps = new SerializedProperty[2] { wheelFrontRightBolt, wheelFrontRightDisk };
            wheelFrontLeftChildrenProps = new SerializedProperty[2] { wheelFrontLeftBolt, wheelFrontLeftDisk };
            glassProps = new SerializedProperty[2] { windowBack, windshield };
            engineProps = new SerializedProperty[1] { engine };
            bumperFrontChildrenProps = new SerializedProperty[1] { bumperFront };
            bumperBackChildrenProps = new SerializedProperty[1] { bumperBack };
            bodyChildrenProps_2 = new SerializedProperty[3] { trunkState, lightBackLeft, lightBackRight };
            LightFrontChildrenProps = new SerializedProperty[2] { lightFront_01, lightFront_02 };
            HoodProps = new SerializedProperty[1] { Logo };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            DrawEditor();

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                car.UpdateData();
            }  
        }

        private void DrawEditor()
        {
            headerStyle = new GUIStyle(EditorStyles.foldoutHeader);
            headerStyle.margin = new RectOffset(30, 10, 10, 10);

            openCloseHeader = EditorGUILayout.BeginFoldoutHeaderGroup(openCloseHeader, "Open/Close", headerStyle);

            EditorGUI.indentLevel++;

            if (openCloseHeader)
            {
                EditorGUILayout.PropertyField(frontLeftDoor);
                EditorGUILayout.PropertyField(frontRightDoor);
                EditorGUILayout.PropertyField(hood);
                EditorGUILayout.PropertyField(trunk);
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndFoldoutHeaderGroup();
                        
            CreateFoldoutGroup("Hood", ref hoodHeader, ref hoodState, HoodProps);
            CreateFoldoutGroup("Body", ref bodyHeader, ref body_02, bodyChildrenProps_2);
            CreateFoldoutGroup("Suspension", ref SuspensionHeader, bodyChildrenProps);
            CreateFoldoutGroup("Front left door", ref frontLeftDoorHeader, ref frontLeftDoorState, frontLeftDoorChildrenProps);
            CreateFoldoutGroup("Front right door", ref frontRightDoorHeader, ref frontRightDoorState, frontRightDoorChildrenProps);
            CreateFoldoutGroup("Bumper front", ref bumperFrontHeader, bumperFrontChildrenProps);
            CreateFoldoutGroup("Bumper back", ref bumperBackHeader, bumperBackChildrenProps);
            CreateFoldoutGroup("Frontcell", ref frontcellHeader, ref frontcell, LightFrontChildrenProps);
            CreateFoldoutGroup("Details", ref detailsHeader, detailsProps);
            CreateFoldoutGroup("Seats", ref seatsHeader, seatsProps);
            CreateFoldoutGroup("Interior", ref interiorHeader, interiorProps);
            CreateFoldoutGroup("Wheel back left", ref wheelBackLeftHeader, ref wheelBackLeft, wheelBackLeftChildrenProps);
            CreateFoldoutGroup("Wheel back right", ref wheelBackRightHeader, ref wheelBackRight, wheelBackRightChildrenProps);
            CreateFoldoutGroup("Wheel front right", ref wheelFrontRightHeader, ref wheelFrontRight, wheelFrontRightChildrenProps);
            CreateFoldoutGroup("Wheel front left", ref wheelFrontLeftHeader, ref wheelFrontLeft, wheelFrontLeftChildrenProps);
            CreateFoldoutGroup("Glass", ref glassHeader, glassProps);
            CreateFoldoutGroup("Engine", ref engineHeader, engineProps);
           


        }

        private void CreateFoldoutGroup(string groupName, ref bool header, ref SerializedProperty mainToggle, SerializedProperty[] childrenToggles)
        {
            header = EditorGUILayout.BeginFoldoutHeaderGroup(header, groupName, headerStyle);

            EditorGUI.indentLevel++;

            if (header)
            {
                if (mainToggle != null)
                {
                    mainToggle.boolValue = EditorGUILayout.BeginToggleGroup(mainToggle.name, mainToggle.boolValue);

                    for (int i = 0; i < childrenToggles.Length; i++)
                    {
                        childrenToggles[i].boolValue = EditorGUILayout.Toggle(childrenToggles[i].name, mainToggle.boolValue ? childrenToggles[i].boolValue : false);
                    }

                    EditorGUILayout.EndToggleGroup();
                }
                else
                {
                    for (int i = 0; i < childrenToggles.Length; i++)
                    {
                        EditorGUILayout.PropertyField(childrenToggles[i]);
                    }
                }
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void CreateFoldoutGroup(string groupName, ref bool header, SerializedProperty[] childrenToggles)
        {
            header = EditorGUILayout.BeginFoldoutHeaderGroup(header, groupName, headerStyle);

            EditorGUI.indentLevel++;

            if (header)
            {
                for (int i = 0; i < childrenToggles.Length; i++)
                {
                    EditorGUILayout.PropertyField(childrenToggles[i]);
                }              
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
