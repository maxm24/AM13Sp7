// Copyright (C) 2014-2023 Gleechi AB. All rights reserved.

using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#else
using UnityEngine.XR;
#endif

namespace VirtualGrasp.Scripts
{
    /** 
     * VG_AnimationDriver provides a generic animation driver to drive finger and object animations 
     * to achieve in-hand manipulation of articulated objects. 
     */
    [LIBVIRTUALGRASP_UNITY_SCRIPT]
    [HelpURL("https://docs.virtualgrasp.com/unity_component_vganimationdriver." + VG_Version.__VG_VERSION__ + ".html")]
    public class VG_AnimationDriver : MonoBehaviour
    {
        [SerializeField, Tooltip("Which hand is the driver")]
        private VG_HandSide m_handSide;
#if ENABLE_INPUT_SYSTEM
        [SerializeField, Tooltip("Which action drives this animation")]
        private InputActionReference m_actionReference;
#else
        [SerializeField]
        private VG_VrButton button = VG_VrButton.TRIGGER;
        private InputDevice handInputDevice;
#endif
        [SerializeField, Tooltip("Input value range")]
        private Vector2 m_inputRange = new Vector2(0f, 1f);
        [SerializeField, Tooltip("Optional, if this is unassigned this transform will be used")]
        private Transform m_interactableObject;

        [Tooltip("Event driving animation from input")]
        public UnityEvent<float> OnDriven = new UnityEvent<float>();

        [Tooltip("Generic animation driver events")]
        public UnityEvent OnEnabled = new UnityEvent();

        [Tooltip("Generic animation driver events")]
        public UnityEvent OnDisabled = new UnityEvent();

        void Awake()
        {
            if (this.m_interactableObject == null)
                this.m_interactableObject = transform;
        }

        void OnEnable()
        {
            OnDriven.Invoke(0.0f);
            OnEnabled.Invoke();
        }

        void OnDisable()
        {
            OnDisabled.Invoke();
        }

        void Start()
        {
            VG_Controller.OnObjectGrasped.AddListener(OnObjectInteractionChanged);
            VG_Controller.OnObjectReleased.AddListener(OnObjectInteractionChanged);
#if ENABLE_INPUT_SYSTEM
            if(this.m_actionReference != null)
                this.m_actionReference.action.Enable();
#endif
            enabled = false;
        }

        private void Update()
        {
            float inputValue = 0;
#if ENABLE_INPUT_SYSTEM
            if(this.m_actionReference != null)
            {
                inputValue = this.m_actionReference.action.ReadValue<float>();
            }
#else
            if (m_handSide == VG_HandSide.LEFT)
                this.handInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            else
                this.handInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            if (!this.handInputDevice.TryGetFeatureValue(this.button == VG_VrButton.TRIGGER ?
                CommonUsages.trigger : CommonUsages.grip, out inputValue))
            {
                Debug.LogError($"Could not read button {this.button} on device {this.handInputDevice}");
                return;
            }
#endif
            var normalizedInputValue = Mathf.InverseLerp(m_inputRange.x, m_inputRange.y, inputValue);
            OnDriven.Invoke(normalizedInputValue);
        }

        private void OnObjectInteractionChanged(VG_HandStatus status)
        {
            if (status.m_selectedObject != m_interactableObject) return;
            if (status.m_side != m_handSide) return;
            this.enabled = status.IsHolding();
        }
    }
}

