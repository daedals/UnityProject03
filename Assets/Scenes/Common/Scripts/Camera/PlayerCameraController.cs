using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Mirror;

public class PlayerCameraController : NetworkBehaviour, GameControls.IVCamActions
{
    [SerializeField] private CinemachineVirtualCamera _vcam;
    [SerializeField] private Transform _playerTransform;

    private CinemachineFramingTransposer _transposer;
    private Quaternion _fixedRotation;

    private GameControls _controls;
    private GameControls Controls
    {
        get
        {
            if (_controls != null) return _controls;
            return _controls = new GameControls();
        }
    }

	public override void OnStartAuthority()
	{
        _transposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        _vcam.gameObject.SetActive(true);

        enabled = true;
        Controls.VCam.SetCallbacks(this);
	}
    

	[ClientCallback] private void OnEnable()
    {
        _fixedRotation =  Quaternion.Inverse(_playerTransform.rotation) * _vcam.transform.rotation;
        Controls.Enable();
    }
	[ClientCallback] private void OnDisable() => Controls.Disable();


    void LateUpdate()
    {
        _vcam.transform.rotation = _fixedRotation;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        
    }
}
