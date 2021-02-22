﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, IEntityAction
{
    [SerializeField] private int ticksLeft = 3;
    [SerializeField] private float radius = 1f;

    private Dictionary<MoveState, MoveStateBase> ActionState;
    private MoveState CurrentMove;
    private bool isDead = false;
    private bool isTurnOver = false;

    private void Start()
    {
        EntityMoveManager.Instance.AddEntity(this);
        ActionState = new Dictionary<MoveState, MoveStateBase>() {
            { MoveState.Waiting, MoveStateFactory.CreateMoveState(MoveState.Waiting, this)},
            { MoveState.HasMove, MoveStateFactory.CreateMoveState(MoveState.HasMove, this)},
            { MoveState.InProcess, MoveStateFactory.CreateMoveState(MoveState.InProcess, this)},
            { MoveState.Done, MoveStateFactory.CreateMoveState(MoveState.Done, this)},
        };
        CurrentMove = MoveState.Waiting;
    }

    private void OnDestroy()
    {
        isDead = true;
    }

    private void ExplodeBomb()
    {
        Vector2 start = transform.position - Vector3.one * radius;
        Vector2 end = transform.position + Vector3.one * radius;
        
        Collider2D[] colliders = Physics2D.OverlapAreaAll(start, end);
        foreach (Collider2D _collider in colliders)
        {
            if (_collider.TryGetComponent(out IDeathBehavior obj))
            {
                obj.Die();
            }
        }

        EventManager.Instance.OnTriggerExplodeBomb(transform.position, radius);
        Debug.Log("Explode");
        //isDead = true;
        isTurnOver = true;
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawCube(transform.position, radius * Vector3.one);
    }

    //public bool IsTurnOver()
    //{
    //    return isTurnOver;
    //}

    public bool IsDead()
    {
        return isDead;
    }

    public bool UpdateState()
    {
        CurrentMove = ActionState[CurrentMove].UpdateState();
        Debug.Log($"CurrenMove in Bomb: {CurrentMove}");

        return (CurrentMove.Equals(MoveState.Done));
    }

    public bool ReadInput()
    {
        //Has no input
        return true;
    }

    public void StartMove()
    {
        //TODO: This is getting run multiple times per turn
        isTurnOver = false;
        if (--ticksLeft <= 0)
        {
            ExplodeBomb();
        }
        isTurnOver = true;
    }

    public bool IsActionComplete()
    {
        return isTurnOver;
    }

    public void EndOfTurnAction()
    {
        //Nothing
    }
}
