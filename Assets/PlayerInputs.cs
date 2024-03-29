//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""3d82e759-bb82-4cb8-a9bd-00f9a3e3c552"",
            ""actions"": [
                {
                    ""name"": ""SingleShot"",
                    ""type"": ""Button"",
                    ""id"": ""991de540-9ace-4535-9f43-fc5f9ab564c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChargeShot"",
                    ""type"": ""Button"",
                    ""id"": ""1ab4eb3d-a42f-4925-adf1-f26c84723b17"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6bc1eeef-16e6-4c06-9ea3-8406e78886c8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SingleShot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56011140-265e-4678-8133-aacf7627a10f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChargeShot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Base Controls"",
            ""bindingGroup"": ""Base Controls"",
            ""devices"": []
        }
    ]
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_SingleShot = m_Mouse.FindAction("SingleShot", throwIfNotFound: true);
        m_Mouse_ChargeShot = m_Mouse.FindAction("ChargeShot", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Mouse
    private readonly InputActionMap m_Mouse;
    private List<IMouseActions> m_MouseActionsCallbackInterfaces = new List<IMouseActions>();
    private readonly InputAction m_Mouse_SingleShot;
    private readonly InputAction m_Mouse_ChargeShot;
    public struct MouseActions
    {
        private @PlayerInputs m_Wrapper;
        public MouseActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @SingleShot => m_Wrapper.m_Mouse_SingleShot;
        public InputAction @ChargeShot => m_Wrapper.m_Mouse_ChargeShot;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void AddCallbacks(IMouseActions instance)
        {
            if (instance == null || m_Wrapper.m_MouseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MouseActionsCallbackInterfaces.Add(instance);
            @SingleShot.started += instance.OnSingleShot;
            @SingleShot.performed += instance.OnSingleShot;
            @SingleShot.canceled += instance.OnSingleShot;
            @ChargeShot.started += instance.OnChargeShot;
            @ChargeShot.performed += instance.OnChargeShot;
            @ChargeShot.canceled += instance.OnChargeShot;
        }

        private void UnregisterCallbacks(IMouseActions instance)
        {
            @SingleShot.started -= instance.OnSingleShot;
            @SingleShot.performed -= instance.OnSingleShot;
            @SingleShot.canceled -= instance.OnSingleShot;
            @ChargeShot.started -= instance.OnChargeShot;
            @ChargeShot.performed -= instance.OnChargeShot;
            @ChargeShot.canceled -= instance.OnChargeShot;
        }

        public void RemoveCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMouseActions instance)
        {
            foreach (var item in m_Wrapper.m_MouseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MouseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    private int m_BaseControlsSchemeIndex = -1;
    public InputControlScheme BaseControlsScheme
    {
        get
        {
            if (m_BaseControlsSchemeIndex == -1) m_BaseControlsSchemeIndex = asset.FindControlSchemeIndex("Base Controls");
            return asset.controlSchemes[m_BaseControlsSchemeIndex];
        }
    }
    public interface IMouseActions
    {
        void OnSingleShot(InputAction.CallbackContext context);
        void OnChargeShot(InputAction.CallbackContext context);
    }
}
