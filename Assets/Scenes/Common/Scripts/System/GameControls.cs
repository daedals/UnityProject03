// GENERATED AUTOMATICALLY FROM 'Assets/Scenes/SampleMap01/Scripts/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""146e4fda-abdf-462a-9c7c-5ca68aaa307b"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""38f7db86-e652-44ce-a039-07f7f9bb7976"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""2699ea49-2541-4328-aa00-ccd2a3338a27"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LMB"",
                    ""type"": ""Button"",
                    ""id"": ""690c888c-9305-4071-9105-cc0dd27d62e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RMB"",
                    ""type"": ""Button"",
                    ""id"": ""fa095727-1679-4cbb-bd53-8954f980a0cb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""294b9e07-650b-496f-83a4-735e672d1fd3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b4ee0bd5-c483-4a5c-abc8-c6966fff23ad"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c1ad31dd-97db-403e-902d-591233f0ac2d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1608395e-538d-4bea-bac2-d7fa368de3c8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2971577a-67c4-4cf3-afc1-e29cd8ec6d96"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e84e8aab-4be7-4bc8-9ba5-8524276a51a0"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc8cb33f-e23a-4f0c-a474-dd6d170e1118"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""RMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f711f0a1-edbe-4cc0-b20a-13e775eb42db"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""LMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""VCam"",
            ""id"": ""1c17f053-e6e5-45bd-b852-f851b5288a8d"",
            ""actions"": [
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""422912d1-fdf1-4eb9-9436-dd928438d0eb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""12709f29-9107-4bab-b83e-063fcaf1a469"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GlobalEvents"",
            ""id"": ""68ef5be3-eb03-49b7-86c6-b02db18d502d"",
            ""actions"": [
                {
                    ""name"": ""ResetScene"",
                    ""type"": ""Button"",
                    ""id"": ""60207260-3bba-4912-a584-f73f63ab5cf1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3f16b7eb-04fa-4cbb-aa90-153ec58236ac"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Mouse = m_Player.FindAction("Mouse", throwIfNotFound: true);
        m_Player_LMB = m_Player.FindAction("LMB", throwIfNotFound: true);
        m_Player_RMB = m_Player.FindAction("RMB", throwIfNotFound: true);
        // VCam
        m_VCam = asset.FindActionMap("VCam", throwIfNotFound: true);
        m_VCam_Zoom = m_VCam.FindAction("Zoom", throwIfNotFound: true);
        // GlobalEvents
        m_GlobalEvents = asset.FindActionMap("GlobalEvents", throwIfNotFound: true);
        m_GlobalEvents_ResetScene = m_GlobalEvents.FindAction("ResetScene", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Mouse;
    private readonly InputAction m_Player_LMB;
    private readonly InputAction m_Player_RMB;
    public struct PlayerActions
    {
        private @GameControls m_Wrapper;
        public PlayerActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Mouse => m_Wrapper.m_Player_Mouse;
        public InputAction @LMB => m_Wrapper.m_Player_LMB;
        public InputAction @RMB => m_Wrapper.m_Player_RMB;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Mouse.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @LMB.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLMB;
                @LMB.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLMB;
                @LMB.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLMB;
                @RMB.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRMB;
                @RMB.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRMB;
                @RMB.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRMB;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @LMB.started += instance.OnLMB;
                @LMB.performed += instance.OnLMB;
                @LMB.canceled += instance.OnLMB;
                @RMB.started += instance.OnRMB;
                @RMB.performed += instance.OnRMB;
                @RMB.canceled += instance.OnRMB;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // VCam
    private readonly InputActionMap m_VCam;
    private IVCamActions m_VCamActionsCallbackInterface;
    private readonly InputAction m_VCam_Zoom;
    public struct VCamActions
    {
        private @GameControls m_Wrapper;
        public VCamActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Zoom => m_Wrapper.m_VCam_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_VCam; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VCamActions set) { return set.Get(); }
        public void SetCallbacks(IVCamActions instance)
        {
            if (m_Wrapper.m_VCamActionsCallbackInterface != null)
            {
                @Zoom.started -= m_Wrapper.m_VCamActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_VCamActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_VCamActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_VCamActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public VCamActions @VCam => new VCamActions(this);

    // GlobalEvents
    private readonly InputActionMap m_GlobalEvents;
    private IGlobalEventsActions m_GlobalEventsActionsCallbackInterface;
    private readonly InputAction m_GlobalEvents_ResetScene;
    public struct GlobalEventsActions
    {
        private @GameControls m_Wrapper;
        public GlobalEventsActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResetScene => m_Wrapper.m_GlobalEvents_ResetScene;
        public InputActionMap Get() { return m_Wrapper.m_GlobalEvents; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalEventsActions set) { return set.Get(); }
        public void SetCallbacks(IGlobalEventsActions instance)
        {
            if (m_Wrapper.m_GlobalEventsActionsCallbackInterface != null)
            {
                @ResetScene.started -= m_Wrapper.m_GlobalEventsActionsCallbackInterface.OnResetScene;
                @ResetScene.performed -= m_Wrapper.m_GlobalEventsActionsCallbackInterface.OnResetScene;
                @ResetScene.canceled -= m_Wrapper.m_GlobalEventsActionsCallbackInterface.OnResetScene;
            }
            m_Wrapper.m_GlobalEventsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ResetScene.started += instance.OnResetScene;
                @ResetScene.performed += instance.OnResetScene;
                @ResetScene.canceled += instance.OnResetScene;
            }
        }
    }
    public GlobalEventsActions @GlobalEvents => new GlobalEventsActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnLMB(InputAction.CallbackContext context);
        void OnRMB(InputAction.CallbackContext context);
    }
    public interface IVCamActions
    {
        void OnZoom(InputAction.CallbackContext context);
    }
    public interface IGlobalEventsActions
    {
        void OnResetScene(InputAction.CallbackContext context);
    }
}
