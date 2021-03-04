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
    [SerializeField] private Gradient _gradient = null;
    [SerializeField] private List<GameObject> _healthBars = new List<GameObject>();
    
    private void OnEnable()
    {
        _healthHandler.EventHealthChanged += RpcHandleHealthChanged;
        _healthHandler.PlayerDeath += RpcHandlePlayerDeath;

        UpdateGradient();
    }

    private void OnDisable()
    {
        _healthHandler.EventHealthChanged -= RpcHandleHealthChanged;
        _healthHandler.PlayerDeath -= RpcHandlePlayerDeath;
    }

    public void Add(GameObject healthbar)
    {
        _healthBars.Add(healthbar);
        UpdateGradient();
    }

    public void Remove(GameObject healthbar)
    {
        _healthBars.Remove(healthbar);
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
    private void RpcHandlePlayerDeath(uint netId)
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
