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
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.T))
            {
                ShowHint(0);
            }
#endif
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
            }

            return ret;
        }

        public async void ShowHint(int id)
        {
            if (textField.gameObject.activeSelf)
                return;

            // Init text
            textField.text = GetText(id);

            // Show
            textField.gameObject.SetActive(true);

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
