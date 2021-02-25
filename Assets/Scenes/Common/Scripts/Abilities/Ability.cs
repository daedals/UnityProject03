using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Ability : MonoBehaviour
{
    public StateMachine stateMachine;

    [SerializeField]
    public List<BaseBehaviour> behaviours;
    public AbilityTemplate template;
    public GameObject owner;

    /*
    Factory method "CreateAbilityComponent" made constructor obsolete

        public Ability(List<BaseBehaviour> behaviours, AbilityTemplate template)
        {
            this.behaviours = behaviours;
            this.template = template;
        }
    */
    public static Ability CreateAbilityComponent(GameObject where, List<BaseBehaviour> behaviours, AbilityTemplate template)
    {
        Ability ability = where.AddComponent<Ability>();
        ability.behaviours = behaviours;
        ability.template = template;

        return ability;
    }

    public void Initialize(GameObject owner)
    {
        this.owner = owner;
        stateMachine = new StateMachine(owner.GetComponent<PlayerAbilityManager>());

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.Initialize(this);
        }

        SetupStatemachine();
    }

    private void SetupStatemachine()
    {	
		// Setup all relevant states
		var inactive = new Inactive();
		var channeling = new Channeling(this, template.channelDuration, behaviours);
		var casting = new Casting(this, template.castDuration, behaviours);
		var onCooldown = new OnCooldown(this, template.cooldownDuration, behaviours);
		var disabled = new Disabled(this, 1f, behaviours);
		var interrupted = new Interrupted(this, template.interruptionDuration, behaviours);

		// connect states with transitions
		stateMachine.AddTransition(inactive, channeling, ref AbilityTriggerPressed);

		stateMachine.AddTransition(channeling, inactive, ref AbilityTriggerReleased);
		stateMachine.AddTransition(channeling, interrupted, ref AbilityInterrupted);
		stateMachine.AddTransition(channeling, casting, ref StateCompleted);

		stateMachine.AddTransition(casting, interrupted, ref AbilityInterrupted);
		stateMachine.AddTransition(casting, onCooldown, ref StateCompleted);

		stateMachine.AddTransition(onCooldown, inactive, ref StateCompleted);

		stateMachine.AddTransition(interrupted, inactive, ref StateCompleted);
		
		stateMachine.AddTransition(disabled, inactive, ref AbilityEnabled);
		stateMachine.AddAnyTransition(disabled, ref AbilityDisabled);

        // Start statemachine
        stateMachine.Start(inactive);
		
		Debug.Log("Initialized State machine to state " + stateMachine.currentState.GetType().ToString());
    }

	#region Events
	public event SubscribtableAction ChannelCanceled;
	public void InvokeChannelCanceled() => ChannelCanceled?.Invoke();
	
	public event SubscribtableAction CastCanceled;
	public void InvokeCastCanceled() => CastCanceled?.Invoke();
	

	public event SubscribtableAction StateCompleted;
	public void InvokeStateCompleted() => StateCompleted?.Invoke();

	public event SubscribtableAction AbilityDisabled;
	public void DisableAbility(float duration)
	{
		// set duration of disable
		AbilityDisabled?.Invoke();
	}
	public event SubscribtableAction AbilityEnabled;
	public void EnableAbility() => AbilityEnabled?.Invoke();

    public event SubscribtableAction AbilityTriggerPressed;
    public event SubscribtableAction AbilityTriggerReleased;

    public void SetTrigger(bool trigger)
    {
        if (trigger)
        {
			Debug.Log("Invoking AbilityTriggerPressed.");
            AbilityTriggerPressed?.Invoke();
        }
        else
        {
            AbilityTriggerReleased?.Invoke();
        }
    }

	public event SubscribtableAction AbilityInterrupted;
	public void InterruptAbility() => AbilityInterrupted?.Invoke();


    public event Action<GameObject, GameObject> AbilityHitTarget;
    public void SignalTargetHit(GameObject obj, GameObject other) => AbilityHitTarget?.Invoke(obj, other);

	#endregion
}
