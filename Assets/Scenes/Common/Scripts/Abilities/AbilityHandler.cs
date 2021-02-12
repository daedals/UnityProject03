using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityHandler : MonoBehaviour
{
    private Coroutine channel = null;
    private float channelTime;

    private Coroutine cast = null;
    private float castTime;

    public AbilityHandler(float channelTime, float castTime)
    {
        this.channelTime = channelTime;
        this.castTime = castTime;

        AbilityTriggerPressed += OnAbilityTriggerPressed;
        AbilityTriggerReleased += OnAbilityTriggerReleased;
    }

    private event Action AbilityTriggerPressed;
    private event Action AbilityTriggerReleased;

    #region Public Functions

    public void SetTrigger(bool trigger)
    {
        if (trigger)
        {
            AbilityTriggerPressed?.Invoke();
        }
        else
        {
            AbilityTriggerReleased?.Invoke();
        }
    }

    public void CancelChannel()
    {
        if (channel != null)
        {
            StopCoroutine(channel);
            channel = null;
            AbilityChannelCancel();
        }
    }
    public void CancelCast()
    {
        if (cast != null)
        {
            StopCoroutine(cast);
            cast = null;
            AbilityCastCancel();
        }
    }

    public void CancelAbility()
    {
        CancelChannel();
        CancelCast();
    }

    public event Action ChannelStarted;
    public event Action ChannelCanceled;
    public event Action ChannelCompleted;

    public event Action CastStarted;
    public event Action CastCanceled;
    public event Action CastCompleted;

    #endregion

    #region Private Functions

    public void OnAbilityTriggerPressed()
    {
        if (channelTime > 0f)
        {
            AbilityChannelStart();
        }
        else
        {
            AbilityCastStart();
        }
    }

    private void AbilityChannelStart()
    {
        ChannelStarted?.Invoke();

        channel = StartCoroutine(AbilityChannel());
    }

    private IEnumerator AbilityChannel()
    {
        yield return new WaitForSeconds(channelTime);
        channel = null;
        AbilityChannelComplete();
    }

    public void OnAbilityTriggerReleased()
    {
        if (channel != null)
        {
            CancelChannel();
        }
        else
        {
            if (castTime > 0f)
            {
                AbilityCastStart();
            }
            else
            {
                AbilityCastComplete();
            }
        }
    }

    private void AbilityChannelCancel()
    {
        ChannelCanceled?.Invoke();
    }

    private void AbilityChannelComplete()
    {
        ChannelCompleted?.Invoke();
    }

    private void AbilityCastStart()
    {
        CastStarted?.Invoke();

        cast = StartCoroutine(AbilityCast());
    }

    private IEnumerator AbilityCast()
    {
        yield return new WaitForSeconds(castTime);
        cast = null;
        AbilityCastComplete();
    }
    
    // currently not triggerable
    private void AbilityCastCancel()
    {
        CastCanceled?.Invoke();
    }

    private void AbilityCastComplete()
    {
        CastCompleted?.Invoke();
    }

    #endregion
}
