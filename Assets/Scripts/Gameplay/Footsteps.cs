using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kidnapped
{
    public class Footsteps : MonoBehaviour
    {
        [System.Serializable]
        class ClipData
        {
            [SerializeField]
            public Texture[] textures;
            [SerializeField]
            public AudioClip[] clips;
        }

        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        List<ClipData> clipDataList;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
            
        }

        public void PlayFootstep()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, out hit, 1f))
            {
                TerrainDetector terrainDetector = hit.collider.GetComponent<TerrainDetector>();
                Texture texture = null;
                if (terrainDetector != null) // Walking on the terrain
                {
                    texture = terrainDetector.GetTexture(hit.point);
                }
                else // Walking on another object
                {
                    MultiMaterialChecker mmc = hit.collider.GetComponent<MultiMaterialChecker>();
                    if (mmc)
                    {
                        Material material = mmc.GetMaterialByTriangleIndex(hit.triangleIndex);
                        if(material)
                        {
                            texture = material.mainTexture;
                        }
                    }
                }

                //if(texture)
                //    Debug.Log($"Step on texture:{texture.name}");

                ClipData clipData = null;
                if (texture) 
                    clipData = clipDataList.Find(c=>c.textures.Contains(texture));

                if(clipData == null)
                    clipData = clipDataList[0];

                audioSource.clip = clipData.clips[Random.Range(0, clipData.clips.Length)];
                audioSource.Play();
            }
        }


    }

}
