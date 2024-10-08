using Kidnapped;
using Kidnapped.SaveSystem;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class ScaryDoor : MonoBehaviour, ISavable
    {
        public UnityAction<ScaryDoor> OnLocked;

        [SerializeField]
        MMF_Player openFx;

        [SerializeField]
        MMF_Player closeFx;

        [SerializeField]
        MMF_Player lockedFx;

        [SerializeField]
        Collider _collider;

        [SerializeField]
        float distance = 1.5f;

        [SerializeField]
        bool closed = false;

        [SerializeField]
        float openAngle = 90;

        bool inside = false;
        DateTime lastInteractionTime;

        private void Awake()
        {
            // Set up fx
            openFx.GetFeedbackOfType<MMF_Rotation>().RemapCurveOne = openAngle;
            closeFx.GetFeedbackOfType<MMF_Rotation>().RemapCurveOne = -openAngle;

            // Get data from cache
            string data = SaveManager.GetCachedValue(code);
            if(string.IsNullOrEmpty(data))
                data = closed.ToString();
            // Init
            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (!inside || !closed) // We can only tigger the locked fx
                return;

            if ((DateTime.Now - lastInteractionTime).TotalSeconds < 1f)
                return;

            // The only interaction is with closed ( locked ) doors, because you can't open door in the school, they open when you an leave the section you are in.
            // This means that the only interaction we have with closed door will trigger the locked fx.
            if (Input.GetKeyDown(KeyBindings.InteractionKey))
            {
                RaycastHit hit;
                Debug.Log("Raycast");
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
                {
                    Debug.Log($"Hit:{hit.collider}");
                    if (hit.collider == _collider)
                    {
                        lockedFx.PlayFeedbacks();
                        OnLocked?.Invoke(this);
                    }
                }
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            inside = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            inside = false;

        }

        public void Open()
        {
            Debug.Log($"Open the door, door name:{gameObject.name}");
            if(!closed) return;
            closed = false;
            openFx.PlayFeedbacks();
            
        }

        public void Close()
        {
            if (closed) return;
            closed = true;
            closeFx.PlayFeedbacks();
        }


        #region save system
        [Header("Save System")]
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return closed.ToString();
        }

        public void Init(string data)
        {
            closed = bool.Parse(data);
            if (!closed)
            {
                _collider.transform.localEulerAngles = Vector3.up * openAngle;
            }
        }
        #endregion
    }

}
