using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Projectile : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private SphereCollider _collider = null;

    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 5f;

    private PlayerIdentity _ownerID;

    public bool Fired { get; private set; }

    private Vector3 _fireDirection;


    private void OnDestroy()
    {
        Debug.Log("Projectile was destroyed");
    }

    [Server]
    public void SetOwner(PlayerIdentity id)
    {
        _ownerID = id;
    }

    [Server]
    public void Fire(Vector3 fireDirection)
    {
        Fired = true;
        _fireDirection = fireDirection;

        _collider.enabled = true;

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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile collided with " + other + " (" + other.gameObject.layer + ")");

        if (other.gameObject.tag == TagNames.Player)
        {
            Debug.Log("Player was hit.");

            PlayerIdentity otherID = other.GetComponent<PlayerIdentity>();

            Debug.Log("Hitbox belongs to projectile owner? " + (otherID.Value == _ownerID.Value));
        }
    }
}
