using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanimAbility : MonoBehaviour
{
    public AbilityTemplate Data { get; set; }
    public List<BaseBehaviour> behaviours;

    [SerializeField] private Animator animator = null;

    private void OnEnable()
    {
        // SceneLinkedSMB<MecanimAbility>.Initialise(animator, this);
    }

    public void Initialize()
    {
        Transform root = transform.parent;

        if (root.GetComponent<PlayerAbilityManager>() == null) throw new System.Exception("Abilities root object has no AbilityManager");

        behaviours = new List<BaseBehaviour>(transform.GetComponents<BaseBehaviour>()).FindAll(behaviour => behaviour.enabled == true);

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.Initialize();
        }
        
        SceneLinkedSMB<MecanimAbility>.Initialise(animator, this);
    }
    
	#region Events
	public event System.Action ChannelCanceled;
	public void InvokeChannelCanceled() => ChannelCanceled?.Invoke();
	
	public event System.Action CastCanceled;
	public void InvokeCastCanceled() => CastCanceled?.Invoke();
	

	public event System.Action StateCompleted;
	public void InvokeStateCompleted() => StateCompleted?.Invoke();

	public event System.Action AbilityDisabled;
	public void DisableAbility(float duration)
	{
		// set duration of disable
		AbilityDisabled?.Invoke();
	}
	public event System.Action AbilityEnabled;
	public void EnableAbility() => AbilityEnabled?.Invoke();

    public event System.Action AbilityTriggerPressed;
    public event System.Action AbilityTriggerReleased;

    public void SetTrigger(bool trigger)
    {
        if (trigger)
        {
			Debug.Log("Invoking AbilityTriggerPressed.");
            animator.SetBool("TriggerPressed", true);
            AbilityTriggerPressed?.Invoke();
        }
        else
        {
            animator.SetBool("TriggerPressed", false);
            AbilityTriggerReleased?.Invoke();
        }
    }

	public event System.Action AbilityInterrupted;
	public void InterruptAbility() => AbilityInterrupted?.Invoke();


    public event System.Action<GameObject, GameObject> AbilityHitTarget;
    public void SignalTargetHit(GameObject obj, GameObject other) => AbilityHitTarget?.Invoke(obj, other);
    
    
    public event System.Action<GameObject, List<GameObject>> AbilityTargetsIdentified;
    public void SignalTargetsIdentified(GameObject obj, List<GameObject> targets) => AbilityTargetsIdentified?.Invoke(obj, targets);

	#endregion
}
