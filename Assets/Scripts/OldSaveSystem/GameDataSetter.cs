using EvolveGames;
using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.OldSaveSystem
{
    public class GameDataSetter : MonoBehaviour
    {
        [System.Serializable]
        class Data
        {
            [SerializeField]
            public GameObject savable;

            //[SerializeField]
            //string code;

            [SerializeField]
            public string value;
        }

        [SerializeField]
        List<Data> dataList;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SetData()
        {
            Debug.Log("Setting data");

            foreach (Data data in dataList)
            {
                ISavable iSav = data.savable.GetComponent<ISavable>();
                iSav.Init(data.value);
            }

            SaveManager.Instance.SaveGame();
        }


    }

}
