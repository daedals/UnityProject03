using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private HealthHandler _healthHandler = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private Image _fill = null;
    [SerializeField] private Gradient _gradient = null;
    
    private void OnEnable()
    {
        _healthHandler.EventHealthChanged += RpcHandleHealthChanged;
        _healthHandler.EventPlayerDeath += RpcHandlePlayerDeath;

        UpdateGradient();
    }

    private void OnDisable()
    {
        _healthHandler.EventHealthChanged -= RpcHandleHealthChanged;
        _healthHandler.EventPlayerDeath -= RpcHandlePlayerDeath;
    }

    [ClientRpc]
    private void RpcHandleHealthChanged(float currentHealth, float maximumHealth)
    {
        _slider.value = currentHealth / maximumHealth;
        UpdateGradient();
    }

    [ClientRpc]
    private void RpcHandlePlayerDeath()
    {
        // disable it i guesss
    }

    private void UpdateGradient() => _fill.color = _gradient.Evaluate(_slider.normalizedValue);

}
