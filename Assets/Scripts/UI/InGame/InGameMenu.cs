using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class InGameMenu : MonoBehaviour
    {
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

        public void QuitGame()
        {
            PopUpManager.Instance.ShowActionPopUp("quit_msg", () => { Time.timeScale = 1f; GameManager.Instance.ReturnToMainScene(); }, () => { });
        }
    }

}
