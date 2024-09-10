using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPreviewPackage
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7.0f;
        private CharacterController charCtrl;
        private RaycastHit[] res;
        private Transform camTransf;

        private void Start()
        {
            charCtrl = this.GetComponent<CharacterController>();
            camTransf = Camera.main.transform;
            res = new RaycastHit[1];
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = transform.right * horizontalInput + transform.forward * verticalInput;

            charCtrl.Move(direction * moveSpeed * Time.deltaTime);

            //In your own project, you probably want to change LayerMask to your own layer to not raycast against default layer
            int hitCount = Physics.RaycastNonAlloc(camTransf.position, camTransf.forward, res, 2.0f, 1 >> LayerMask.NameToLayer("Default"));
            if (hitCount > 0)
            {
                DemoUI.Instance.helpPanel.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CarPartController contr = res[0].transform.GetComponent<CarPartController>();
                    if (contr)
                        contr.Interact(); //contr?.Interact()
                }
            }
            else
            {
                DemoUI.Instance.helpPanel.SetActive(false);
            }
        }
    }
}
