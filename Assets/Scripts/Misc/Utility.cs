using Aura2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public static class Utility
    {
        // Funzione generica per copiare un componente da un oggetto all'altro
        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            // Crea una nuova istanza del componente su destination
            T copy = destination.AddComponent<T>();

            // Copia i campi pubblici e privati del componente originale
            System.Type type = typeof(T);
            System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }

            return copy;
        }

        public static void SwitchLightOn(Light light, bool value)
        {
            light.enabled = value;
            
            LightFlicker lf = light.GetComponent<LightFlicker>();
            if(lf)
                lf.enabled = value;
            AuraLight al = light.GetComponent<AuraLight>();
            if(al)
                al.enabled = value;
        }

        public static void SetCursorVisible(bool value)
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
        }

    }
    
}
