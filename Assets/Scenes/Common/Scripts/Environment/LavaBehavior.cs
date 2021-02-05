using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider _collider = null;

    [Header("Settings")]
    [SerializeField] public float _damagePerSecond = 5f;

    private List<HealthHandler> _healthHandlers = new List<HealthHandler>();


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

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player left trigger.");
        HealthHandler healthHandler = other.GetComponentInParent<HealthHandler>();

        if (healthHandler != null)
        {
            _healthHandlers.Remove(healthHandler);
        }
    }

    private void Update()
    {
        foreach(HealthHandler obj in _healthHandlers)
        {
            obj.CmdDealDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}
