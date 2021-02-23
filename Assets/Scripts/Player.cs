using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(EntityGridMove))]
public class Player : MonoBehaviour, IEntityController
{
    //[SerializeField] private EntityGridMove EntityGridMove;
    [SerializeField] private Bomb BombPrefab;
    [SerializeField] private LayerMask collisionLayer;
    public Color MovePointColor = Color.blue;

    private Bomb placedBomb = null;
    private bool isSettingBomb;
    private Vector2 inputVector;
    private Vector2? movePoint;
    private bool isAtMovePoint;


    private void Start()
    {
        isSettingBomb = false;
        isAtMovePoint = true;
        inputVector = transform.position;
        movePoint = null;

        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    private void Update()
    {
        SetMove(inputVector);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = MovePointColor;

        if (movePoint.HasValue)
            Gizmos.DrawSphere(movePoint.Value, .2f);
    }

    public void OnMove(InputValue _inputValue)
    {
        inputVector = _inputValue.Get<Vector2>();
        //EntityGridMove.SetInputVector(_inputValue.Get<Vector2>());
    }

    public void OnFire(InputValue _inputValue)
    {
        Debug.Log("FIRE");
        isSettingBomb = _inputValue.isPressed && (placedBomb == null || placedBomb.IsDead()) && isAtMovePoint;
        //if (_inputValue.isPressed && placedBomb == null && EntityGridMove.isAtMovePoint)
        //{
        //    placedBomb = Instantiate(BombPrefab, transform.position, Quaternion.identity);
        //    //EntityGridMove.UsedAction();
        //}
    }

    public void SetMove(Vector2 moveVector)
    {
        if (isAtMovePoint && !isSettingBomb)
        {
            bool _hasMove = false;
            Vector3 _newPosition = transform.position;
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
            else if (_hasMove)
            { //else the move is valid set the movePoint and keep _hasMove to true
                movePoint = _newPosition;
            }

            //if _hasMove is true then is not at MovePoint
            //if _hasMove is false then has no new move and value should still be same
            isAtMovePoint = !_hasMove && isAtMovePoint; //simplified of:  = !_hasMove ? false : isAtMovepoint
        }
    }

    private bool IsStuck()
    {
        bool canMove = false;
        foreach (Vector2 _move in EntityMoveManager.Directions)
        {
            Vector2 move = _move + new Vector2(transform.position.x, transform.position.y);
            if (!Physics2D.OverlapCircle(move, 0.2f, collisionLayer))
            {
                canMove = true;
                break;
            }
        }
        return !canMove;
    }

    public Vector2? ReadInput()
    {
        Vector2? input;
        if (IsStuck())
            GetComponent<PlayerDeathBehavior>().Die();

        if (isSettingBomb)
        {
            placedBomb = Instantiate(BombPrefab, transform.position, Quaternion.identity);
            input = transform.position;
        }
        else
        {
            input = movePoint;
        }
        return input;
    }

    public void EndOfTurnAction()
    {
        //Make sure the position isn't offset and leads to weird collisions
        transform.position = Vector3Int.RoundToInt(transform.position);

        //Reset turn variables
        isSettingBomb = false;
        isAtMovePoint = true;
        movePoint = null;
    }
}
