using Kidnapped;
using Kidnapped.OldSaveSystem;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Kidnapped
{
    public class BurningBlock : MonoBehaviour, ISavable
    {
        [SerializeField]
        DoorController doorController;

        [SerializeField]
        bool triggerEnabled = false;

        [SerializeField]
        List<GameObject> burningBlocks;

        [SerializeField]
        List<ParticleSystem> dissolveBlocks;

        [SerializeField]
        ParticleSystem dissolvePS;

        [SerializeField]
        Collider blockCollider;

        Collider trigger;
        //bool dissolve = false;
        //float dissolveTime = .5f;
        //float dissolveElapsed = 0;
        

        private void Awake()
        {
            Debug.Log($"Awake:{gameObject}");
            trigger = GetComponent<Collider>();
        }

        private void Start()
        {
            if (!triggerEnabled)
                trigger.enabled = false;
            else
                trigger.enabled = true;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(Dissolve());
            }
#endif

            //if (!dissolve)
            //    return;

            //dissolveElapsed += Time.deltaTime;

            //if(dissolveElapsed > dissolveTime)
            //{
            //    dissolve = false;
            //    blockCollider.enabled = false;
            //}
        }

        private void OnEnable()
        {
            //DoorController.OnDoorOpenFailed += HandleOnOpenFailed;
           
        }

        private void OnDisable()
        {
            //DoorController.OnDoorOpenFailed -= HandleOnOpenFailed;
         
        }

       

        private void HandleOnOpenFailed(DoorController doorController)
        {
            if (this.doorController != doorController)
                return;

            trigger.enabled = true;
            doorController.GetComponentInParent<BurningController>().StartBurning();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            trigger.enabled = false;
            //dissolve = true;
            StartCoroutine(Dissolve());

            Debug.Log("Player is inside");
        }

        IEnumerator Dissolve()
        {
            dissolvePS.Play();
            
            foreach (var b in dissolveBlocks)
                b.Play();

            yield return new WaitForSeconds(.75f);

            foreach (GameObject b in burningBlocks)
                b.SetActive(false);

            
        }

        void DisableAll()
        {
            foreach (GameObject b in burningBlocks)
                b.SetActive(false);
            trigger.enabled = false;
            blockCollider.enabled = false;
        }

        #region save system
        [Header("SaveSystem")]
        [SerializeField]
        string code;

        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return triggerEnabled.ToString();
        }

        public void Init(string data)
        {
            triggerEnabled = bool.Parse(data);
            if(triggerEnabled && !trigger.enabled)
            {
                trigger.enabled = true;
            }
            else if(!triggerEnabled && trigger.enabled)
            {
                trigger.enabled = false;
                doorController.GetComponentInParent<BurningController>().StartBurning();
                DisableAll();
            }
        }



        #endregion
    }

}
