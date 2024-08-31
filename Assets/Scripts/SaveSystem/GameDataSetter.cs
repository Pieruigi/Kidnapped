using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    public class GameDataSetter : MonoBehaviour
    {
        [SerializeField]
        PlayerData playerData = new PlayerData() { Code = "player" };

        [SerializeField]
        CutSceneData fadeIn = new CutSceneData() { Code = "fade_in" };

        [SerializeField]
        CutSceneData fadeOut = new CutSceneData() { Code = "fade_out" };

        [SerializeField]
        List<CutSceneData> cutScenes;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetFadeInOut(List<CutSceneSavable> list)
        {
            // Fade in
            var tmp = list.Find(c=>c.Code == fadeIn.Code);
            if (tmp)
                tmp.SetData(fadeIn);
            tmp = list.Find(c => c.Code == fadeOut.Code);
            if (tmp)
                tmp.SetData(fadeOut);
        }

        void SetCutScenes(List<CutSceneSavable> list)
        {
            // Loop through each cut
        }

        public void SetData()
        {
            Debug.Log("Setting data");

            //PlayerController.Instance.Init() // Init() method missing; also check the savable script
            List<CutSceneSavable> csd = new List<CutSceneSavable>(FindObjectsOfType<CutSceneSavable>());
            // Set fade in and fade out
            SetFadeInOut(csd);
            SetCutScenes(csd);
        }


    }

}
