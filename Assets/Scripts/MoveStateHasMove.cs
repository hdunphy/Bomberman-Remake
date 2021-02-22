using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateHasMove : MoveStateBase
{
    public MoveStateHasMove(MoveState moveState, IEntityAction entityAction) : base(moveState, entityAction) { }
    public override MoveState UpdateState()
    {
        EntityAction.StartMove();
        return MoveState.InProcess;
    }
}
