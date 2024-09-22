using System.Collections;
using System.Collections.Generic;
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

        public void Show(string text)
        {
            if(!bg.gameObject.activeSelf)
                bg.gameObject.SetActive(true);
            textField.text = text;
            
        }
                


        public void Hide()
        {
            textField.text = "";
            bg.gameObject.SetActive(false);
        }
    }

}
