using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateFactory
{
    public static MoveStateBase CreateMoveState(MoveState state, IEntityAction entityAction)
    {
        MoveStateBase moveState;
        switch (state)
        {
            case MoveState.Waiting:
                moveState = new MoveStateWaiting(state, entityAction);
                break;
            case MoveState.HasMove:
                moveState = new MoveStateHasMove(state, entityAction);
                break;
            case MoveState.InProcess:
                moveState = new MoveStateInProcess(state, entityAction);
                break;
            case MoveState.Done:
                moveState = new MoveStateDone(state, entityAction);
                break;
            default:
                Debug.LogWarning("Hit default state");
                moveState = new MoveStateWaiting(state, entityAction);
                break;
        }

        return moveState;
    }
}
