using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGridMove : MonoBehaviour, IEntityAction
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

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


        animator.SetBool("IsIdle", true);
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
        Vector2 dir = movePoint - new Vector2(transform.position.x, transform.position.y);
        animator.SetBool("IsIdle", false);
        //animator.SetFloat("MoveHorizontal", dir.x);
        //animator.SetFloat("MoveVertical", dir.y);

        while (Vector3.Distance(transform.position, movePoint) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isAtMovePoint = true;
        animator.SetBool("IsIdle", true);
        //animator.SetFloat("MoveHorizontal", 0f);
        //animator.SetFloat("MoveVertical", 0f);
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
