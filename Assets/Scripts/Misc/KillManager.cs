using EvolveGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Kidnapped
{
    public class KillManager : Singleton<KillManager>
    {
        public enum Killer { Lilith, Puck }

        

        [System.Serializable]
        private class KillingInfo
        {

            [SerializeField]
            public float cameraFov;

            [SerializeField]
            public Vector3 cameraEulers;

            [SerializeField]
            public Vector3 characterPosition;

            [SerializeField]
            public Vector3 characterEulers;

            [SerializeField]
            public GameObject killerLightPrefab;

            [SerializeField]
            public int killerAnimation;

            [SerializeField]
            public int killerSoundIndex;

            [SerializeField]
            public float killerSoundTime;

            [SerializeField]
            public float exitDelay;

        }

        [SerializeField]
        List<KillingInfo> lilithKillingInfos;

        [SerializeField]
        List<KillingInfo> puckKillingInfos;

        [SerializeField]
        GameObject lilithPrefab;

        [SerializeField]
        GameObject puckPrefab;

        KillingInfo info;
        Killer killer;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        KillingInfo GetRandomKillingInfo(Killer killer)
        {
            List<KillingInfo> tmp = null;

            switch(killer)
            {
                case Killer.Lilith:
                    tmp = lilithKillingInfos;
                    break;
                case Killer.Puck:
                    tmp = puckKillingInfos;
                    break;
            }

            return tmp[UnityEngine.Random.Range(0, tmp.Count)];
        }

        GameObject GetKillerPrefab(Killer killer)
        {
            GameObject prefab = null;
            switch (killer)
            {
                case Killer.Lilith:
                    prefab = lilithPrefab;
                    break;
                case Killer.Puck:
                    prefab = puckPrefab;
                    break;
            }

            return prefab;
        }

        float offDuration = .5f;
        public void Kill(Killer killer)
        {
            if (PlayerController.Instance.IsDying) 
                return; // Player is already dying

            // Set player dying
            PlayerController.Instance.IsDying = true;
            // Disable player interaction
            PlayerController.Instance.InteractionDisabled = true;

            this.killer = killer;

            // Get random info
            info = GetRandomKillingInfo(killer);
            
            // Flicker 
            
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerToDarkeness(HandleOnLightOff, null, offDuration);

            
        }

        async void HandleOnLightOff(float duration)
        {
            

            // Stop the player from moving and receiving input
            PlayerController.Instance.PlayerInputEnabled = false;

            // Hide player hands
            PlayerController.Instance.HideHandAll();

            // Move the player on the killing plane
            PlayerController.Instance.ForcePositionAndRotation(transform.position, transform.rotation);
            
            // Stop audio 
            GameSceneAudioManager.Instance.StopAmbience();

            await Task.Delay(TimeSpan.FromSeconds(offDuration));

            HandleOnFlickerComplete();
        }

        async void HandleOnFlickerComplete()
        {
            // Instantiate the killer object
            var killerObject = Instantiate(GetKillerPrefab(killer));

            // Set material
            killerObject.GetComponent<EvilMaterialSetter>().SetEvil();

            // Move the object under the camera
            killerObject.transform.parent = Camera.main.transform;

            // Set local position and rotation
            killerObject.transform.localPosition = info.characterPosition;
            killerObject.transform.localEulerAngles = info.characterEulers;

            // Set animation
            Animator anim = killerObject.GetComponentInChildren<Animator>();
            anim.SetInteger("Type", info.killerAnimation);
            anim.SetTrigger("Kill");

            // Play sound
            GameSceneAudioManager.Instance.PlayKiller(info.killerSoundIndex, time:info.killerSoundTime);

            // Set camera
            Camera.main.fieldOfView = info.cameraFov;
            Camera.main.transform.GetChild(0).GetComponent<Camera>().fieldOfView = info.cameraFov;
            Camera.main.transform.localEulerAngles = info.cameraEulers;

            // Spawn the killer light
            var killerLight = Instantiate(info.killerLightPrefab, Camera.main.transform);
            killerLight.transform.localPosition = Vector3.zero;
            killerLight.transform.localRotation = Quaternion.identity;
            //killerLight.SetActive(false);


            await Task.Delay(TimeSpan.FromSeconds(info.exitDelay));
            // Reload
            GameManager.Instance.FadeOutAndReloadAfterDeath();
        }
    }

}
