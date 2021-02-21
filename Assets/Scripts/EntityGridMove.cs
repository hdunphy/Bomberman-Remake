using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGridMove : MonoBehaviour, IEntityAction
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask collisionLayer;
    public Color MovePointColor = Color.red;
    public Action PreActionCallback;

    public bool isAtMovePoint { get; private set; }

    private bool isDead;
    private bool hasMove;
    private Vector2 inputVector;
    private Vector2 movePoint;

    private void Start()
    {
        //isMoving = false;
        isAtMovePoint = true;
        isDead = false;
        movePoint = transform.position;
    }

    private void OnDestroy()
    {
        isDead = true;
        StopAllCoroutines();
    }

    public void StartTurnAction()
    {
        //isMoving = true;
        hasMove = false;
        isAtMovePoint = false;
        StartCoroutine(MoveToPosition());
    }

    private IEnumerator MoveToPosition()
    {
        if (!hasMove)
            yield return null;
        while (Vector3.Distance(transform.position, movePoint) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isAtMovePoint = true;
        hasMove = false;
    }

    public void UsedAction()
    {
        isAtMovePoint = true;
        movePoint = transform.position;
    }

    private void Update()
    {
        SetMove(inputVector);
    }

    public void SetInputVector(Vector2 _inputVector)
    {
        inputVector = _inputVector;
    }

    public void SetMove(Vector2 moveVector)
    {
        if (isAtMovePoint)
        {
            bool _hasMove = false;
            Vector3 _newPosition = movePoint;
            if (Mathf.Abs(moveVector.x) == 1f)
            {
                _newPosition += new Vector3(moveVector.x, 0f);
                _hasMove = true;
            }
            else if (Mathf.Abs(moveVector.y) == 1f)
            {
                _newPosition += new Vector3(0f, moveVector.y);
                _hasMove = true;
            }

            if (Physics2D.OverlapCircle(_newPosition, 0.2f, collisionLayer))
            { //if the new position collides with a collider. Can't move so reset _hasMove to false
                _hasMove = false;
            }
            else
            { //else the move is valid set the movePoint and keep _hasMove to true
                movePoint = _newPosition;
            }

            //if _hasMove is true then is not at MovePoint
            //if _hasMove is false then has no new move and value should still be same
            isAtMovePoint = !_hasMove && isAtMovePoint; //simplified of:  = !_hasMove ? false : isAtMovepoint
            hasMove = _hasMove;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = MovePointColor;

        Gizmos.DrawSphere(movePoint, .2f);
    }

    public bool IsTurnOver()
    {
        PreActionCallback?.Invoke();
        return isAtMovePoint;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
