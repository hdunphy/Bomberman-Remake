using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateInProcess : MoveStateBase
{
    public MoveStateInProcess(MoveState moveState, IEntityAction entityAction) : base(moveState, entityAction) { }

    public override MoveState UpdateState()
    {
        MoveState nextState = CurrentState;

        if (EntityAction.IsActionComplete())
        {
            nextState = MoveState.Done;
        }

        return nextState;
    }
}
