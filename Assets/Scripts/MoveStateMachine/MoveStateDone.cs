using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateDone : MoveStateBase
{
    public MoveStateDone(MoveState moveState, IEntityAction entityAction) : base(moveState, entityAction) { }

    public override MoveState UpdateState()
    {
        EntityAction.EndOfTurnAction();
        return MoveState.Waiting;
    }
}
