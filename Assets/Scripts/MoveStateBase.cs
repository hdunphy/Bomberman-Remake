using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState { Waiting, HasMove, InProcess, Done }

public abstract class MoveStateBase
{
    protected MoveState CurrentState;
    protected IEntityAction EntityAction;

    public MoveStateBase(MoveState _currentState, IEntityAction _entityAction)
    {
        CurrentState = _currentState;
        EntityAction = _entityAction;
    }

    public abstract MoveState UpdateState();
}
