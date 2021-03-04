using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HUDHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject HUD = null;
    [SerializeField] private Healthbar healthbar = null;

	public override void OnStartAuthority()
	{
        enabled = true;
        HUD.SetActive(true);
        healthbar.Add(HUD.transform.Find("Canvas").transform.Find("Healthbar").gameObject);
	}

    private void OnDisable()
    {
        healthbar.Remove(HUD.transform.Find("Canvas").transform.Find("Healthbar").gameObject);
        HUD.SetActive(false);
    }
}
