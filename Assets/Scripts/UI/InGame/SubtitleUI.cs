using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class SubtitleUI : Singleton<SubtitleUI>
    {
        [SerializeField]
        Image bg;

        [SerializeField]
        TMP_Text textField;

        protected override void Awake()
        {
            base.Awake();
            bg.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public async void Show(string text, bool autoHide = false)
        {
            if(!bg.gameObject.activeSelf)
                bg.gameObject.SetActive(true);
            textField.text = text;
            
            if(autoHide)
            {
                // Check how many words and calculate half a second for each word
                int count = text.Split(' ').Length;
                await Task.Delay(500 * count);
                Hide();
            }
        }
                


        public void Hide()
        {
            textField.text = "";
            bg.gameObject.SetActive(false);
        }
    }

}
