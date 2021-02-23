using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityController
{
    Vector2? ReadInput();
    void EndOfTurnAction();
}
