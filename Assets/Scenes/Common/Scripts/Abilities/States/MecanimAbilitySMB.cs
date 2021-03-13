using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MecanimAbilitySMB : SceneLinkedSMB<MecanimAbility>
{
    private List<BaseBehaviour> behaviours;
    [SerializeField] private BaseBehaviour.ExecutionMask executionMask;
    public BaseBehaviour.ExecutionMask ExecutionMask { get => executionMask; }

    private float duration;
    private float remainingDuration;

	public override void OnStart(Animator animator)
	{
		base.OnStart(animator);

        duration = m_MonoBehaviour.Data.StateDurations.Get(executionMask);
        behaviours = m_MonoBehaviour.behaviours.SelectRelevant(ExecutionMask);
	}

	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{
		base.OnSLStateEnter(animator, stateInfo, layerIndex, controller);
        
        Debug.Log("SMB OnStateEnter of " + ExecutionMask.ToString());
        
        remainingDuration = duration;

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnEnter(new BaseAbilityState.AbilityStateContext(null, false, duration, 0));
        }
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex, controller);
        
        Debug.Log("SMB OnStateExit of " + ExecutionMask.ToString());

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnExit(new BaseAbilityState.AbilityStateContext(null, remainingDuration == 0 ? true : false, duration, duration - remainingDuration));
        }
	}

	public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{
		base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);

        remainingDuration = Mathf.Max(remainingDuration - Time.deltaTime, 0);

        if (remainingDuration == 0 ) animator.SetTrigger("StateCompleted");

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.Tick(new BaseAbilityState.AbilityStateContext(null, false, duration, duration - remainingDuration));
        }
	}
}
