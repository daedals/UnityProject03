using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LavaBehavior : NetworkBehaviour
{

    [Header("Settings")]
    [SerializeField] public float _damagePerSecond = 5f;

    private List<HealthHandler> _healthHandlers = new List<HealthHandler>();


    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger." + other);
        HealthHandler healthHandler = other.GetComponentInParent<HealthHandler>();

        if (healthHandler != null)
        {
            _healthHandlers.Add(healthHandler);
        }
        else Debug.Log("Failed to fetch HealthHandler from Entity.");
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player left trigger.");
        HealthHandler healthHandler = other.GetComponentInParent<HealthHandler>();

        if (healthHandler != null)
        {
            _healthHandlers.Remove(healthHandler);
        }
    }

    [Server]
    private void Update()
    {
        // ToList() is a hack to avoid the error that the collection was modified during enumeration, so we just copy it
        foreach(HealthHandler obj in _healthHandlers.ToList())
        {
            if (obj == null)
            {
                // object has been destroyed because player died
                _healthHandlers.Remove(obj);
                continue;
            }

            obj.DealDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}
