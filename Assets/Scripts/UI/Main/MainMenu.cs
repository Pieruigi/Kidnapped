using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        Button continueButton;

        [SerializeField]
        Button newGameButton;

        [SerializeField]
        Button exitButton;

        private void Awake()
        {
            continueButton.onClick.AddListener(LoadSavedGame);
            newGameButton.onClick.AddListener(StartNewGame);
            exitButton.onClick.AddListener(QuitGame);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            if (!SaveManager.Instance)
                return;

            if (SaveManager.Instance.SaveGameExists())
                continueButton.interactable = true;
            else
                continueButton.interactable = false;


        }

        void LoadSavedGame()
        {
            GameManager.Instance.LoadSavedGame();
        }

        void StartNewGame()
        {
            PopUpManager.Instance.ShowPopUp("new_game_msg", () => { GameManager.Instance.StartNewGame(); }, () => { });
        }

        void QuitGame()
        {
            PopUpManager.Instance.ShowPopUp("quit_msg", () => { Application.Quit(); }, () => { });
        }
    }

}
