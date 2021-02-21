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
        hasMove = false;
        isAtMovePoint = true;
        isDead = false;
        movePoint = transform.position;
    }

    private void OnDestroy()
    {
        isDead = true;
        StopAllCoroutines();
    }

    public void EndOfTurnAction()
    {
        hasMove = false;
        StartCoroutine(MoveToPosition());
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

    public void UsedAction()
    {
        hasMove = true;
        movePoint = transform.position;
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);

        if (isAtMovePoint)
        {
            bool _hasMove = false;
            Vector3 _newPosition = movePoint;
            if (Mathf.Abs(inputVector.x) == 1f)
            {
                _newPosition += new Vector3(inputVector.x, 0f);
                _hasMove = true;
            }
            else if (Mathf.Abs(inputVector.y) == 1f)
            {
                _newPosition += new Vector3(0f, inputVector.y);
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

    public void SetInputVector(Vector2 _inputVector)
    {
        inputVector = _inputVector;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = MovePointColor;

        Gizmos.DrawSphere(movePoint, .2f);
    }

    public bool HasAction()
    {
        PreActionCallback?.Invoke();
        return hasMove;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
