using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Projectile : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 5f;

    private bool _fired;
    private Vector3 _fireDirection;
    private GameObject _spawn;

    [Server]
    public void SetReferences(GameObject spawn, PlayerActionProcessor processor)
    {
        _spawn = spawn;
    }


    [ServerCallback]
    private void OnEnable()
    {
        _fired = false;
    }

    [Server]
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

    [Server]
    public void Fire()
    {
        _fired = true;
        _fireDirection = _spawn.transform.forward;
        // StartCoroutine(DestroySelfAfterSeconds(_lifeTime));
        Destroy(gameObject, _lifeTime);
    }

    [Server]
    private void OnDestroy()
    {
        Debug.Log("Projectile was destroyed");
    }

    [Server]
    IEnumerator DestroySelfAfterSeconds(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        
        NetworkServer.UnSpawn(this.gameObject);
        Debug.Log("Projectile was despawned");
    }
}
