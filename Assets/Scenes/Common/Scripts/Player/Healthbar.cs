using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Healthbar : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private HealthHandler _healthHandler = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private Image _fill = null;
    [SerializeField] private Gradient _gradient = null;
    [SerializeField] private List<GameObject> _healthBars = new List<GameObject>();
    
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
        foreach (GameObject healthbar in _healthBars)
        {
            healthbar.GetComponent<Slider>().value = currentHealth / maximumHealth;
        }
        
        UpdateGradient();
    }

    [ClientRpc]
    private void RpcHandlePlayerDeath()
    {
        // disable it i guesss
    }

    private void UpdateGradient()
    {
        foreach (GameObject healthbar in _healthBars)
        {
            healthbar.transform.Find("Fill").GetComponent<Image>().color = _gradient.Evaluate(healthbar.GetComponent<Slider>().normalizedValue);
        }
    }
}
