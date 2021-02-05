using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 5f;

    private bool _fired;
    private Vector3 _fireDirection;
    private GameObject _spawn;

    public void SetReferences(GameObject spawn, PlayerActionProcessor processor)
    {
        processor.OnProjectileFired += OnFired;
        _spawn = spawn;
    }


    private void OnEnable()
    {
        _fired = false;
    }

    private void OnDestroy()
    {
        Debug.Log("Projectiles lifetime is over.");
    }

    private void OnFired()
    {
        _fired = true;
        _fireDirection = _spawn.transform.forward;
        Destroy(this, _lifeTime);
    }

    private void Update()
    {
        if (!_fired)
        {
            transform.position = _spawn.transform.position;
            transform.rotation = _spawn.transform.rotation;
        }
        else
        {
            transform.position += _fireDirection * _speed * Time.deltaTime;
        }
    }
}
