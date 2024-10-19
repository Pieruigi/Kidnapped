using EvolveGames;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class BloodyFloor : MonoBehaviour
    {
        public UnityAction OnHeightReached;

        [SerializeField]
        float maxHeight = 1.9f;

        [SerializeField]
        float riseSpeed = .015f;

        [SerializeField]
        Vector2 scrollSpeed;

        
        bool stopRising = false;
        Material material;
        float scrollSpeedRandomizeTime = 2;
        float scrollSpeedRandomizeElapsed = 0;
        Vector2 targetScrollSpeed;
        Vector2 currentScrollSpeed;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
            targetScrollSpeed = GetRandomScrollSpeed();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Rise();
            Scroll();
        }

        void Rise()
        {
            if (stopRising)
                return;

            Vector3 newPosition = transform.position + Vector3.up * riseSpeed * Time.deltaTime;

            transform.position = newPosition;

            if(newPosition.y > maxHeight)
            {
                stopRising = true;
                OnHeightReached?.Invoke();
            }
        }

        Vector2 GetRandomScrollSpeed()
        {
            return new Vector2(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f));
        }

        void Scroll()
        {

            //scrollSpeedRandomizeElapsed += Time.deltaTime;
            //if (scrollSpeedRandomizeElapsed > scrollSpeedRandomizeTime)
            //{
            //    scrollSpeedRandomizeElapsed = 0;
            //    // Change the scroll speed 
            //    targetScrollSpeed = GetRandomScrollSpeed();
            //}

            //scrollSpeed = Vector2.SmoothDamp(scrollSpeed, targetScrollSpeed, ref currentScrollSpeed, Time.deltaTime);
            //scrollSpeed = new Vector2(0.001f, 0.001f);
            Vector2 offset = material.GetTextureOffset("_MainTex");
            Vector2 newOffset = offset + scrollSpeed * Time.deltaTime;
            material.SetTextureOffset("_MainTex", newOffset);
        }
    }

}
