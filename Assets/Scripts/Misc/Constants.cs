using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class Tags
    {
        public static string Player = "Player";
        public static string Cat = "Cat";
        public static string KillingPlane = "Killing Plane";
    }

    public class Layers
    {
        public static string Interaction = "Interaction";
        public static string Player = "Player";
        public static string SightObstacle = "SightObstacle";   
    }

    public class KeyBindings
    {
        public static KeyCode InteractionKey = KeyCode.E;
        public static KeyCode FlashlightKey = KeyCode.F;
        public static KeyCode CrouchKey = KeyCode.LeftControl;
        public static KeyCode SprintKey = KeyCode.LeftShift;
    }

    public class GameplaySettings
    {
        public static float InteractionDistance = 1.5f;
    }

    public class  LocalizationTables
    {
        public static string Subtitles = "subtitles";
        public static string Menu = "menu";
    }
}
