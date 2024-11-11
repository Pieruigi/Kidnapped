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

        //[SerializeField]
        Button newGameButton;

        //[SerializeField]
        Button optionsButton;

        //[SerializeField]
        Button exitButton;

        void Awake()
        {
            //continueButton.onClick.AddListener(LoadSavedGame);
            //newGameButton.onClick.AddListener(StartNewGame);
            //exitButton.onClick.AddListener(QuitGame);
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

        public void LoadSavedGame()
        {
            GameManager.Instance.LoadSavedGame();
        }

        public void StartNewGame()
        {
            PopUpManager.Instance.ShowActionPopUp("new_game_msg", () => { GameManager.Instance.StartNewGame(); }, () => { });
        }

        

        public void QuitGame()
        {
            PopUpManager.Instance.ShowActionPopUp("quit_msg", () => { Application.Quit(); }, () => { });
        }
    }

}
