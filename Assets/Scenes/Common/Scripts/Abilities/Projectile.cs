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

    public int Ticket { get; private set; }

    public void Initialize(int ticket, float movementSpeed, float lifeTime)
    {
        Ticket = ticket;
        this.movementSpeed = movementSpeed;
        this.lifeTime = lifeTime;

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

    public void Fire(Vector3 targetDirection)
    {
        if (!initialized) return;

        fired = true;
        this.targetDirection = targetDirection;

        StartCoroutine(DestroyAtEndOfLife());
    }

    public event Action<Projectile> LifeTimeEnded;
    private IEnumerator DestroyAtEndOfLife()
    {
        yield return new WaitForSeconds(lifeTime);
        LifeTimeEnded?.Invoke(this);
    }

    public event Action<Projectile, GameObject> TargetHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // TODO: determine if gameObject has the right tag
            // exclude the casting player for some time (TBD)

            TargetHit?.Invoke(this, other.gameObject);
        }
    }
}
