using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    public class CutSceneSavable : Savable
    {
        [SerializeField]
        CutSceneController controller;

        public override void SetData(object data)
        {
            CutSceneData dd = (CutSceneData)data;
            controller.Init(dd.PlayOnEnter);
        }

        public override object GetData()
        {
            return new CutSceneData() { Code = Code, PlayOnEnter = controller.PlayOnEnter };

        }
    }

}
