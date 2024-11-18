using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class DemoManager : Singleton<DemoManager>
    {
        [SerializeField]
        SimpleActivator block;

        public void ActivateBlock()
        {
            block.Init(true.ToString());
        }
    }

}
