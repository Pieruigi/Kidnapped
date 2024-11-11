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
        Button okButton;

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

        void HideAllButtons()
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            okButton.gameObject.SetActive(false);
        }

        public void ShowActionPopUp(string key, UnityAction onYesButtonClicked, UnityAction onNoButtonClicked)
        {
            if (IsVisible())
                return;

            RemoveAllListeners();

            HideAllButtons();

            yesButton.gameObject.SetActive(true);
            yesButton.onClick.AddListener(() => { onYesButtonClicked?.Invoke(); Hide(); });
            noButton.gameObject.SetActive(true);
            noButton.onClick.AddListener(() => { onNoButtonClicked?.Invoke(); Hide(); });

            locText.SetEntry(key);

            Show();
        }

        public void ShowInfoPopUp(string key)
        {
            if (IsVisible())
                return;

            RemoveAllListeners();

            HideAllButtons();

            okButton.gameObject.SetActive(true);
            okButton.onClick.AddListener(() => { Hide(); });

            locText.SetEntry(key);

            Show();
        }
    }

}
