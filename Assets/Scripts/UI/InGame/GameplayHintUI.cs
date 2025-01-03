using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Kidnapped.UI
{
    public class GameplayHintUI : Singleton<GameplayHintUI>
    {
        [SerializeField]
        TMP_Text textField;

        

        // Start is called before the first frame update
        void Start()
        {
            textField.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        }

        string GetText(int id)
        {
            string ret = "";

            switch (id)
            {
                case 0: // Crouch
                    ret = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Menu, "hint_crouch");
                    ret = string.Format(ret, KeyBindings.CrouchKey.ToString());
                    break;
                case 1: // Stealth and sprint
                    ret = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Menu, "hint_hunt");
                    ret = string.Format(ret, KeyBindings.CrouchKey.ToString(), KeyBindings.SprintKey.ToString());
                    break;
            }

            return ret;
        }

        public async void ShowHint(int id)
        {
            if (textField.gameObject.activeSelf)
                return;

            // Init text
            textField.text = GetText(id);

#if !TRAILER
            // Show
            textField.gameObject.SetActive(true);
#endif

            await Task.Delay(5000);

            // Hide
            textField.gameObject.SetActive(false);
        }

        public void ForceHideHint()
        {
            textField.gameObject.SetActive(false);
        }
    }

}
