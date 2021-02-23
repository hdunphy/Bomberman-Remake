using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateWaiting : MoveStateBase
{
    public MoveStateWaiting(MoveState moveState, IEntityAction entityAction) : base(moveState, entityAction) { }

    public override MoveState UpdateState()
    {
        if (EntityAction.ReadInput())
            return MoveState.HasMove;
        else
            return CurrentState;
    }
}
