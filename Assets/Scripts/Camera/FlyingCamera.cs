using EvolveGames;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped
{
    public class FlyingCamera : MonoBehaviour
    {
        [SerializeField]
        GameObject flyingCamera;

        [SerializeField]
        GameObject _light;

        bool activated = false;
        bool paused = false;

        float yaw = 0;
        float pitch = 0;
        private float moveSpeed = 5f;
        float mouseSensitivity = 50;
        bool running = false;

        private void Awake()
        {
#if !UNITY_EDITOR
            Destroy(gameObject);
#endif
        }

        // Start is called before the first frame update
        void Start()
        {
            flyingCamera.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha0)) 
            {
                activated = !activated;

                if (activated)
                {
                    PlayerController.Instance.transform.parent.gameObject.SetActive(false);
                    flyingCamera.SetActive(true);
                    paused = false;
                }
                else
                {
                    PlayerController.Instance.transform.parent.gameObject.SetActive(true);
                    flyingCamera.SetActive(false);
                }
            }

            if (activated)
            {

                if (Input.GetKeyDown(KeyCode.P))
                {
                    paused = !paused;
                }

                if (paused)
                    return;

                Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                yaw += mouseSensitivity * mouseInput.x * Time.deltaTime;
                pitch -= mouseSensitivity * mouseInput.y * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -90, 90);
                transform.eulerAngles = new Vector3(pitch, yaw, 0);

                running = false;

                if (Input.GetKey(KeyCode.LeftShift))
                    running = true;
                


                if (Input.GetKey(KeyCode.W))
                    transform.position += transform.forward * (running ? 3*moveSpeed:moveSpeed) * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                    transform.position -= transform.forward * (running ? 3 * moveSpeed : moveSpeed) * Time.deltaTime;
                if (Input.GetKey(KeyCode.A))
                    transform.position -= transform.right * (running ? 3 * moveSpeed : moveSpeed) * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    transform.position += transform.right * (running ? 3 * moveSpeed : moveSpeed) * Time.deltaTime;
                if (Input.GetKey(KeyCode.Q))
                    transform.position += Vector3.up * (running ? 3 * moveSpeed : moveSpeed) * Time.deltaTime;
                if (Input.GetKey(KeyCode.E))
                    transform.position -= Vector3.up * (running ? 3 * moveSpeed : moveSpeed) * Time.deltaTime;

                

            }
           
        }
    }

}
