using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(EntityGridMove))]
public class Player : MonoBehaviour
{
    [SerializeField] private EntityGridMove EntityGridMove;
    [SerializeField] private Bomb BombPrefab;

    private Bomb placedBomb = null;

    private void Start()
    {
        EntityMoveManager.Instance.AddEntity(EntityGridMove);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnTriggerPlayerDies();
    }

    public void OnMove(InputValue _inputValue)
    {
        EntityGridMove.SetInputVector(_inputValue.Get<Vector2>());
    }

    public void OnFire(InputValue _inputValue)
    {
        Debug.Log("FIRE");
        if (_inputValue.isPressed && placedBomb == null && EntityGridMove.isAtMovePoint)
        {
            placedBomb = Instantiate(BombPrefab, transform.position, Quaternion.identity);
            EntityGridMove.UsedAction();
        }
    }
}
