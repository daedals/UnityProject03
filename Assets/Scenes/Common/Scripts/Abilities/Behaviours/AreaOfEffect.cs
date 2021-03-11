using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaOfEffect : BaseBehaviour
{
    public override BaseBehaviourData Data { get => data; set => data = (AreaOfEffectData)value; }
    [SerializeField] private AreaOfEffectData data;

	public override void Initialize()
	{
		GetComponent<Ability>().AbilityHitTarget += OnAbilityTargetHit;
	}

	private void OnAbilityTargetHit(GameObject obj, GameObject other)
	{
        if (!(other.gameObject.tag == data.affectedTag)) return;

        List<GameObject> affectedObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag(data.affectedTag));

        Vector3 center = obj.transform.position;

        affectedObjs = affectedObjs.FindAll(x => Vector3.ProjectOnPlane(x.transform.position - center, Vector3.up).magnitude <= data.range);

        Debug.Log($"{name} identified {affectedObjs.Count} target{(affectedObjs.Count != 1 ? "s" : "")} in range {data.range} with tag \"{data.affectedTag}\"");

        GetComponent<Ability>().SignalTargetsIdentified(obj, affectedObjs);
	}
}
