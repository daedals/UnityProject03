using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Projectile : NetworkBehaviour
{
    private bool initialized = false;
    private bool fired = false;
    private Vector3 targetDirection;
    private float movementSpeed;
    private float lifeTime;

    private uint ownerNetId;

    public bool ownerProtection = true;

    private void OnEnable()
    {
        if(initialized && fired) 
        {
            StartCoroutine(LifeTimeCoroutine());
        }
    }

    public void Initialize(float movementSpeed, float lifeTime, uint ownerNetId)
    {
        this.movementSpeed = movementSpeed;
        this.lifeTime = lifeTime;
        this.ownerNetId = ownerNetId;

        initialized = true;
    }

    private void Update()
    {
        if (fired)
        {
            // move in target direction
            transform.position = transform.position + targetDirection * movementSpeed * Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        fired = false;
        ownerProtection = true;
    }

    public void Fire(Vector3 targetDirection)
    {
        if (!initialized) return;

        fired = true;
        this.targetDirection = targetDirection.normalized;
    }

    public event Action<Projectile> LifeTimeEnded;
    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        LifeTimeEnded?.Invoke(this);
    }

    public event Action<Projectile, GameObject> TargetHit;
	private void OnTriggerEnter(Collider other)
    {
        if (ownerProtection &&
            (other.gameObject.tag == "Player") && 
            (other.GetComponent<NetworkIdentity>() != null) && 
            (other.GetComponent<NetworkIdentity>().netId == ownerNetId))
        {
            ownerProtection = false;
            Debug.Log("Removed owner protection.");
            return;
        }

        TargetHit?.Invoke(this, other.gameObject);
    }
    
}
