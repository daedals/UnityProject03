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

    public bool Fired { get; private set; }

    private Vector3 _fireDirection;


    private void OnDestroy()
    {
        Debug.Log("Projectile was destroyed");
    }

    [Server]
    public void Fire(Vector3 fireDirection)
    {
        Fired = true;
        _fireDirection = fireDirection;
        StartCoroutine(UpdatePosition());
        Destroy(gameObject, _lifeTime);
    }

    [Server]
    IEnumerator UpdatePosition()
    {
        while (true)
        {
            transform.position += _fireDirection * _speed * Time.deltaTime;

            yield return 0;
        }
    }


    // currently not used, serves as code reference (pls fix)
    [Server]
    IEnumerator DestroySelfAfterSeconds(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        
        NetworkServer.UnSpawn(this.gameObject);
        Debug.Log("Projectile was despawned");
    }
}
