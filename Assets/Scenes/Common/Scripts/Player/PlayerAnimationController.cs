using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator = null;
    [SerializeField] private CharacterController _controller = null;
    [SerializeField] private EntityDataContainer _playerData = null;

    private Vector2 _movementDirectionAnimation = Vector2.up;

    private void Update() => Animate();

    private void Animate()
    {
        
        float angle = transform.rotation.eulerAngles.y - Vector3.SignedAngle(Vector3.forward, _controller.velocity, Vector3.up);
        float magnitude = _controller.velocity.magnitude / _playerData.movementSpeed;

        _movementDirectionAnimation = Vector2.Lerp(
            _movementDirectionAnimation, 
            new Vector2(magnitude * Mathf.Cos(angle * Mathf.PI / 180f), magnitude * Mathf.Sin(angle * Mathf.PI / 180f)),
            .2f);

        _animator.SetFloat("xDirection", _movementDirectionAnimation.x);
        _animator.SetFloat("yDirection", _movementDirectionAnimation.y);
    }
}
