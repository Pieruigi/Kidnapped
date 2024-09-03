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

        //void SetFadeInOut(List<CutSceneSavable> list)
        //{
        //    // Fade in
        //    var tmp = list.Find(c=>c.Code == fadeIn.Code);
        //    if (tmp)
        //        tmp.SetData(fadeIn);
        //    tmp = list.Find(c => c.Code == fadeOut.Code);
        //    if (tmp)
        //        tmp.SetData(fadeOut);
        //}

        //void SetCutScenes(List<CutSceneSavable> list)
        //{
        //    // Loop through each cut
        //    foreach(var c in cutScenes)
        //    {
        //        var savable = list.Find(s=>s.Code == c.Code);
        //        savable.SetData(c);
        //    }
        //}

        public void SetData()
        {
            Debug.Log("Setting data");

            foreach (Data data in dataList)
            {
                ISavable iSav = data.savable.GetComponent<ISavable>();
                iSav.Init(data.value);
            }
           
        }


    }

}
