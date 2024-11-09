using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class PopUpManager : Singleton<PopUpManager>
    {

        [SerializeField]
        GameObject panel;

        [SerializeField]
        Button yesButton;

        [SerializeField]
        Button noButton;

        [SerializeField]
        TMP_Text textField;

        LocalizeStringEvent locText;

        protected override void Awake()
        {
            base.Awake();
            locText = textField.GetComponent<LocalizeStringEvent>();
            locText.SetTable("menu");
        }

        // Start is called before the first frame update
        void Start()
        {
            Hide();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void Show()
        {
            panel.SetActive(true);
        }

        void Hide()
        {
            panel.SetActive(false);
        }

        bool IsVisible()
        {
            return panel.activeSelf;
        }

        void RemoveAllListeners()
        {
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
        }

        public void ShowPopUp(string key, UnityAction onYesButtonClicked, UnityAction onNoButtonClicked)
        {
            if (IsVisible())
                return;

            RemoveAllListeners();

            yesButton.onClick.AddListener(() => { onYesButtonClicked?.Invoke(); Hide(); });
            noButton.onClick.AddListener(() => { onNoButtonClicked?.Invoke(); Hide(); });

            locText.SetEntry(key);

            Show();
        }
    }

}
