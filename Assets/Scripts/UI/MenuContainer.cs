using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class MenuContainer : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> menuList;

        [SerializeField]
        int defaultMenu = -1;

        // Start is called before the first frame update
        void Start()
        {
            if(defaultMenu >= 0)
                ShowMenu(defaultMenu);
            else
                HideMenuAll();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HideMenuAll()
        {
            foreach (var menu in menuList) 
                menu.SetActive(false);
        }

        public void ShowMenu(int index)
        {
            HideMenuAll();
            menuList[index].SetActive(true);
        }
    }

}
