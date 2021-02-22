using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGridMove : MonoBehaviour, IEntityAction
{
    [SerializeField] private float moveSpeed = 5f;

    public bool isAtMovePoint { get; private set; }

    private bool isDead;
    private Vector2 movePoint;


    private IEntityController Controller;
    private Dictionary<MoveState, MoveStateBase> ActionState;
    private MoveState CurrentMove;

    private void Start()
    {
        //isMoving = false;
        isAtMovePoint = true;
        isDead = false;
        movePoint = transform.position;
        Controller = GetComponent<IEntityController>();

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
        StopAllCoroutines();
    }

    public void SetController(IEntityController controller)
    {
        Controller = controller;
    }

    private IEnumerator MoveToPosition()
    {
        while (Vector3.Distance(transform.position, movePoint) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isAtMovePoint = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public bool ReadInput()
    {
        bool hasInput = false;
        Vector2? move = Controller.ReadInput();
        if (move.HasValue)
        {
            movePoint = move.Value;
            hasInput = true;
        }

        return hasInput;
    }

    public void StartMove()
    {
        isAtMovePoint = false;
        StartCoroutine(MoveToPosition());
    }

    public bool IsActionComplete()
    {
        return isAtMovePoint;
    }

    public void EndOfTurnAction()
    {
        Controller.EndOfTurnAction();
    }

    public bool UpdateState()
    {
        CurrentMove = ActionState[CurrentMove].UpdateState();

        return (CurrentMove.Equals(MoveState.Done));
    }
}
