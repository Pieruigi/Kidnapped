using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPreviewPackage
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 100.0f;
        [SerializeField] private Transform playerTransfrom;
        private float xRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

            transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
            playerTransfrom.Rotate(Vector3.up * mouseX);
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
