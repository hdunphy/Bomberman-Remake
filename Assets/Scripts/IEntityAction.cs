using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAction
{
    bool IsDead();
    bool IsTurnOver();
    void StartTurnAction();
}
