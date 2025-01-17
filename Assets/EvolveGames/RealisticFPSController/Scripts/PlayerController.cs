﻿//by EvolveGames
using Kidnapped;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Singleton<PlayerController>, ISavable
    {
        [Header("PlayerController")]
        [SerializeField] public Transform Camera;
        [SerializeField] public ItemChange Items;
        [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
        [Range(0.1f, 5)] public float CroughSpeed = 1.0f;
        [SerializeField, Range(2, 20)] float RuningSpeed = 4.0f;
        [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;
        [SerializeField, Range(1f, 10)] float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
        [Space(20)]
        [Header("Advance")]
        [SerializeField] float RunningFOV = 65.0f;
        [SerializeField] float SpeedToFOV = 4.0f;
        [SerializeField] float CroughHeight = 1.0f;
        [SerializeField] float gravity = 20.0f;
        [SerializeField] float timeToRunning = 2.0f;
        [HideInInspector] public bool canMove = true;
        /*[HideInInspector]*/ public bool CanRunning = true;

        [Space(20)]
        [Header("Climbing")]
        [SerializeField] bool CanClimbing = true;
        [SerializeField, Range(1, 25)] float Speed = 2f;
        bool isClimbing = false;

        [Space(20)]
        [Header("HandsHide")]
        [SerializeField] bool CanHideDistanceWall = true;
        [SerializeField, Range(0.1f, 5)] float HideDistance = 1.5f;
        [SerializeField] int LayerMaskInt = 1;

        [Space(20)]
        [Header("Input")]
        [SerializeField] KeyCode CroughKey = KeyCode.LeftControl;


        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Vector3 moveDirection = Vector3.zero;
        bool isCrough = false;
        public bool IsCrouching {  get { return isCrough; } }
        float InstallCroughHeight;
        float rotationX = 0;
        [HideInInspector] bool isRunning = false;
        public bool IsRunning {  get { return isRunning; } }
        Vector3 InstallCameraMovement;
        float InstallFOV;
        Camera cam;
        [HideInInspector] public bool Moving;
        [HideInInspector] public float vertical;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float Lookvertical;
        [HideInInspector] public float Lookhorizontal;
        float RunningValue;
        float installGravity;
        bool WallDistance;
        [HideInInspector] public float WalkingValue;

        public bool InteractionDisabled { get; set; } = false;
        public bool PlayerInputEnabled { get; set; } = true;


        public bool IsDying { get; set; } = false;

        public bool CanCrouch { get; set; } = true;

        //public bool CanRun { get; set; } = false;

        public bool HasFlashlight { get; set; } = false;

        Animator animator;
        bool invertedMouseY = false;

        

        protected override void Awake()
        {
            base.Awake();
            // Init
            string data = SaveManager.GetCachedValue(code);
            if (!string.IsNullOrEmpty(data))
            {
                Init(data);
            }

            animator = GetComponent<Animator>();
        }

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            if (Items == null && GetComponent<ItemChange>()) Items = GetComponent<ItemChange>();
            cam = GetComponentInChildren<Camera>();
            Utility.SetCursorVisible(false);
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            InstallCroughHeight = characterController.height;
            InstallCameraMovement = Camera.localPosition;
            InstallFOV = cam.fieldOfView;
            RunningValue = RuningSpeed;
            installGravity = gravity;
            WalkingValue = walkingSpeed;
        }

        void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.Tab)) 
            {
                PlayerInputEnabled = !PlayerInputEnabled;
            }

            if (Input.GetKeyDown(KeyCode.M)) 
            {
                transform.parent.position = new Vector3(-37f, 1.7f, 141f);
            }

            //RuningSpeed = 40;
#endif

            if (!PlayerInputEnabled)
            {
                animator.SetFloat("Speed", 0); // Stop footsteps
                return;
            }
            

            RaycastHit CroughCheck;
            RaycastHit ObjectCheck;

            if (!characterController.isGrounded && !isClimbing)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            isRunning = !isCrough ? CanRunning ? Input.GetKey(KeyBindings.SprintKey) : false : false;
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                isRunning = true;
            }
#endif
            vertical = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Vertical") : 0;
            horizontal = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Horizontal") : 0;
            if (isRunning) RunningValue = Mathf.Lerp(RunningValue, RuningSpeed, timeToRunning * Time.deltaTime);
            else RunningValue = WalkingValue;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * vertical) + (right * horizontal);
            moveDirection = Vector3.ClampMagnitude(moveDirection, isRunning ? RunningValue : WalkingValue);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isClimbing)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }
            characterController.Move(moveDirection * Time.deltaTime);
            Moving = horizontal < 0 || vertical < 0 || horizontal > 0 || vertical > 0 ? true : false;

            if (Cursor.lockState == CursorLockMode.Locked && canMove)
            {
                Lookvertical = Input.GetAxis("Mouse Y") * (invertedMouseY ? 1 : -1);
                Lookhorizontal = Input.GetAxis("Mouse X");

                rotationX += Lookvertical * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

                if (isRunning && Moving) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
                else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
            }

            if (Input.GetKey(CroughKey) && CanCrouch)
            {
                isCrough = true;
                float Height = Mathf.Lerp(characterController.height, CroughHeight, 5 * Time.deltaTime);
                characterController.height = Height;
                WalkingValue = Mathf.Lerp(WalkingValue, CroughSpeed, 6 * Time.deltaTime);

            }
            else if (!Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.up), out CroughCheck, 0.8f, 1))
            {
                if (characterController.height != InstallCroughHeight)
                {
                    isCrough = false;
                    float Height = Mathf.Lerp(characterController.height, InstallCroughHeight, 6 * Time.deltaTime);
                    characterController.height = Height;
                    WalkingValue = Mathf.Lerp(WalkingValue, walkingSpeed, 4 * Time.deltaTime);
                }
            }

            if(WallDistance != Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt) && CanHideDistanceWall)
            {
                WallDistance = Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt);
                Items.ani.SetBool("Hide", WallDistance);
                Items.DefiniteHide = WallDistance;
            }

            animator.SetFloat("Speed", characterController.velocity.magnitude);
        }

        private void OnEnable()
        {
            if (SettingsManager.Instance)
            {
                SetLookSpeed(SettingsManager.Instance.MouseSensitivity);
                HandleOnMouseYAxisInvertedChanged(SettingsManager.Instance.MouseInvertedY);
            }
            SettingsManager.OnMouseSensitivityChanged += HandleOnMouseSensitivityChanged;
            SettingsManager.OnMouseYAxisInvertedChanged += HandleOnMouseYAxisInvertedChanged;
        }

        private void OnDisable()
        {
            SettingsManager.OnMouseSensitivityChanged -= HandleOnMouseSensitivityChanged;
            SettingsManager.OnMouseYAxisInvertedChanged -= HandleOnMouseYAxisInvertedChanged;
        }

        private void HandleOnMouseYAxisInvertedChanged(bool inverted)
        {
            invertedMouseY = inverted;
        }

        private void HandleOnMouseSensitivityChanged(float sensitivity)
        {
            SetLookSpeed(sensitivity);
        }

        void SetLookSpeed(float mouseSensitivity)
        {
            lookSpeed = mouseSensitivity * .5f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            { 
                CanRunning = false;
                isClimbing = true;
                WalkingValue /= 2;
                Items.Hide(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                moveDirection = new Vector3(0, Input.GetAxis("Vertical") * Speed * (-Camera.localRotation.x / 1.7f), 0);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                CanRunning = true;
                isClimbing = false;
                WalkingValue *= 2;
                Items.ani.SetBool("Hide", false);
                Items.Hide(false);
            }
        }

        public void PrepareToCutScene()
        {
            GetComponent<CharacterController>().enabled = false;
        }

        public void ResetFromCutScene()
        {
            GetComponent<CharacterController>().enabled = true;
        }

        public void ForcePositionAndRotation(Transform target)
        {
            ForcePositionAndRotation(target.position, target.rotation);
        }

        public void ForcePositionAndRotation(Vector3 position, Quaternion rotation)
        {
            characterController.enabled = false;
            transform.position = position;
            transform.rotation = rotation;
            characterController.enabled = true;
        }

        public void HideHandAll()
        {
            // Get hand roots
            var roots = transform.parent.GetComponentsInChildren<FPSHandRotator>();
            foreach (var root in roots)
            {
                root.gameObject.SetActive(false);
            }
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
            char c = ISavable.Separator;
            return $"{HasFlashlight}{c}{CanRunning}{c}{CanCrouch}{c}{SaveManager.ParseVector3ToString(transform.position)}{c}{SaveManager.ParseQuaternionToString(transform.rotation)}";
        }

        public void Init(string data)
        {
            string[] s = data.Split(ISavable.Separator);
            HasFlashlight = bool.Parse(s[0]);
            CanRunning = bool.Parse(s[1]);
            CanCrouch = bool.Parse(s[2]);
            characterController = GetComponent<CharacterController>();
            characterController.enabled = false;
            if (!"-".Equals(s[3]))
                transform.position = SaveManager.ParseStringToVector3(s[3]);
            if (!"-".Equals(s[4]))
                transform.rotation = SaveManager.ParseStringToQuaternion(s[4]);
            characterController.enabled = true;
        }
        #endregion
    }
}