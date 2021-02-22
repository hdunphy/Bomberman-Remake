using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAction
{
    bool IsDead();
    bool ReadInput();
    void StartMove();
    bool IsActionComplete();
    void EndOfTurnAction();
    bool UpdateState();
}
