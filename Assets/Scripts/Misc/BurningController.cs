using AndreyGraphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class BurningController : MonoBehaviour
    {
       
        [SerializeField]
        float startBurningDuration = 5;

        // Negative means infinite
        [SerializeField]
        float burningDuration = 10;

        
        [SerializeField]
        float stopBurningDuration = 0;

        [SerializeField]
        List<Renderer> renderers;

        [SerializeField]
        Mesh shapeMesh;

        [SerializeField]
        ParticleSystem fireParticleSystem;

        [SerializeField]
        float[] fireEmissionRateArray;

        [SerializeField]
        List<Light> lights;

        [SerializeField]
        List<AudioSource> audioSources;

        [SerializeField]
        bool burnOnEnable = false;
       
        bool looping = false;
        float elapsed = 0;

        ParticleSystem[] fireParticles;
        
        float[] maxVolumes;
        float stopAudioTime = 3f;

        float[] maxIntensities;
        float stopLightTime = 3;

        void Awake()
        {
            

            // Reset shaders
            ResetBurningShaders();

            // Init particle systems
            InitFireParticles();

            // Init lights
            InitLights();

            // Init audio
            InitAudio();


        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.B))
            {
               
                StartBurning();
            }
#endif                        

            if (looping)
            {
                elapsed += Time.deltaTime;

                // Start burning or keep burning
                if((elapsed < startBurningDuration || startBurningDuration == 0) && (elapsed < startBurningDuration + burningDuration || burningDuration < 0))
                {
                    if(elapsed < startBurningDuration || startBurningDuration == 0) // Start burning
                    {
                        float value = elapsed / startBurningDuration;
                        SetStartBurning(startBurningDuration > 0 ? value : 1);
                        PlayFireParticleSystem(startBurningDuration > 0 ? value : 1);
                        PlayAudio(startBurningDuration > 0 ? value : 1);
                        PlayLights(startBurningDuration > 0 ? value : 1);
                    }
                    else // Keep burning
                    {
                        PlayFireParticleSystem(burningDuration > 0 ? 1 - elapsed / (startBurningDuration + burningDuration) : 1);
                    }    
                }
                else // Sto pburning
                {
                    if(burningDuration >= 0 && elapsed > startBurningDuration + burningDuration)
                    {
                        SetStopBurning((elapsed - startBurningDuration - burningDuration) / stopBurningDuration);
                        StopFireParticleSystem();
                        StopAudio(1f - (elapsed - startBurningDuration - burningDuration) / stopAudioTime);
                        StopLights(1f - (elapsed - startBurningDuration - burningDuration) / stopLightTime);
                    }
                        
                }

                if (burningDuration >= 0 && elapsed > stopBurningDuration + startBurningDuration + burningDuration)
                    looping = false;
            }

        }

        private void OnEnable()
        {
            if (burnOnEnable)
                StartBurning();
        }

        void ResetBurningShaders()
        {
            foreach (var renderer in renderers)
            {
                renderer.material.SetFloat("StartBurning", 0);
                renderer.material.SetFloat("StopBurning", 0);
            }
        }

        void InitAudio()
        {
            if (audioSources != null)
            {
                maxVolumes = new float[audioSources.Count];
                for(int i=0; i<audioSources.Count; i++)
                {
                    maxVolumes[i] = audioSources[i].volume;
                    audioSources[i].volume = 0;
                }
                
                
            }
        }

        void PlayAudio(float normalizedVolume)
        {
            if (audioSources == null)
                return;
            normalizedVolume = Mathf.Clamp01(normalizedVolume);
            for (int i=0; i<audioSources.Count;i++)
            {
                if (!audioSources[i].isPlaying)
                    audioSources[i].Play();
                audioSources[i].volume = normalizedVolume * maxVolumes[i];
            }
                
        }

        void StopAudio(float normalizedVolume)
        {
            
            if (audioSources != null)
            {
                normalizedVolume = Mathf.Clamp01(normalizedVolume);
                for (int i = 0; i < audioSources.Count; i++)
                {
                    if (audioSources[i].isPlaying)
                    {
                        audioSources[i].volume = maxVolumes[i] * normalizedVolume;
                        if (normalizedVolume == 0)
                            audioSources[i].Stop();
                    }
                    
                }

            }
        }



        void InitLights()
        {
            if (lights != null)
            {
                maxIntensities = new float[lights.Count];
                for (int i = 0; i < lights.Count; i++)
                {
                    maxIntensities[i] = lights[i].intensity;
                    lights[i].intensity = 0;
                    lights[i].enabled = false;
                    LightFlicker flicker = lights[i].GetComponent<LightFlicker>();
                    UpdateLightFlicker(flicker, 0);
                    flicker.enabled = false;
                }
            }
        }

        void PlayLights(float normalizedValue)
        {
            
            if(lights == null) return;
            Debug.Log($"Play lights:{normalizedValue}");
            normalizedValue = Mathf.Clamp01(normalizedValue);
            for (int i=0; i < lights.Count; i++)
            {
                LightFlicker flicker = lights[i].GetComponent<LightFlicker>();
                if (!lights[i].enabled)
                {
                    lights[i].enabled = true;
                    flicker.enabled = true;
                }

                lights[i].intensity = maxIntensities[i] * normalizedValue;
                UpdateLightFlicker(flicker, lights[i].intensity);
            }
        }

        void StopLights(float normalizedValue)
        {
            if (lights == null) return;
            normalizedValue = Mathf.Clamp01(normalizedValue);
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].enabled)
                {
                    LightFlicker flicker = lights[i].GetComponent<LightFlicker>();
                    lights[i].intensity = maxIntensities[i] * normalizedValue;
                    UpdateLightFlicker(flicker, lights[i].intensity);
                    if (normalizedValue == 0)
                    {
                        Debug.Log($"StopLight:{lights[i].gameObject.name}");
                        flicker.enabled = false;
                        lights[i].enabled = false;
                    }
                         
                }
                    
                    
            }
        }

        void UpdateLightFlicker(LightFlicker flicker, float intensity)
        {
            flicker.minIntensity = intensity * .8f;
            flicker.maxIntensity = intensity * 1.2f;
        }

        void InitFireParticles()
        {
            if (fireParticleSystem != null)
            {
                if (shapeMesh == null)
                    shapeMesh = renderers[0].GetComponent<MeshFilter>().sharedMesh;

                fireParticles = new ParticleSystem[fireParticleSystem.transform.childCount];
                for (int i = 0; i < fireParticleSystem.transform.childCount; i++)
                {
                    fireParticles[i] = fireParticleSystem.transform.GetChild(i).GetComponent<ParticleSystem>();
                    var shape = fireParticles[i].shape;
                    Debug.Log($"Setting shapemesh:{shapeMesh}");
                    shape.mesh = shapeMesh;
                    
                }
            }
        }

        void PlayFireParticleSystem(float value)
        {
            if (fireParticleSystem != null && !fireParticleSystem.isPlaying)
            {
                Debug.Log("Playing particle");
                fireParticleSystem.Play(true);
            }

            ChangeParticleEmission(value);
        }

        void StopFireParticleSystem()
        {
            if (fireParticleSystem != null && fireParticleSystem.isPlaying)
            {
                fireParticleSystem.Stop(true);
            }
        }

        void ChangeParticleEmission(float value)
        {
            value = Mathf.Clamp01(value);
            for (int i = 0; i < fireParticles.Length - 1; i++)
            {
                ParticleSystem fire = fireParticles[i];
                var em = fire.emission;
                em.rateOverTime = Mathf.FloorToInt(value * fireEmissionRateArray[i]);
            }
        }

       

        void SetStartBurning(float value)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.SetFloat("StartBurning", value);
            }
    
        }

        void SetStopBurning(float value)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.SetFloat("StopBurning", value);
            }
        }


        public void StartBurning()
        {
            ResetBurningShaders();
            SetStartBurning(0);
            looping = true;
            elapsed = 0;
        }
    }

}
