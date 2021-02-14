using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBehaviour : System.ICloneable
{
    [System.Flags]
	public enum ExecutionMask
	{
        NONE = 0,
		INACTIVE = 1 << 1,
		CHANNELING = 1 << 2,
		CASTING = 1 << 3,
		ONCOOLDOWN = 1 << 4,
		DISABLED = 1 << 5,
		INTERRUPTED = 1 << 6,
        ALL = 1 << 7 - 1
	}

    public ExecutionMask executionMask = ExecutionMask.NONE;

    public abstract void Tick();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract object Clone();
}
