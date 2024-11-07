using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class SelectorHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        float alphaOnEnter = 0.8f;
        float alphaOnExit = 0.24f;

        Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, alphaOnEnter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, alphaOnExit);
        }
    }

}

