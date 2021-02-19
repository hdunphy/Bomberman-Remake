using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGridMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask collisionLayer;
    public Color MovePointColor = Color.red;
    public bool IsNewPosition { get; private set; }


    private Vector2 inputVector;
    private Vector2 movePoint;
    private bool canMove = true;

    private void Start()
    {
        IsNewPosition = false;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint) <= 0.05f)
        {
            ArrivedAtNewPosition();
            Vector3 _newPosition = movePoint;
            if (Mathf.Abs(inputVector.x) == 1f)
            {
                _newPosition += new Vector3(inputVector.x, 0f);
            }
            else if (Mathf.Abs(inputVector.y) == 1f)
            {
                _newPosition += new Vector3(0f, inputVector.y);
            }

            if (!Physics2D.OverlapCircle(_newPosition, 0.2f, collisionLayer))
            {
                movePoint = _newPosition;
            }
        }
        else
            IsNewPosition = true;
    }

    private void ArrivedAtNewPosition()
    {
        if (IsNewPosition)
        {
            EventManager.Instance.OnTriggerMoveUpdate();
            IsNewPosition = false;
            //Debug.Log($"New position {transform.position}");
        }
    }

    public void SetInputVector(Vector2 _inputVector)
    {
        if (canMove)
        {
            inputVector = _inputVector;
        }
    }

    public void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = MovePointColor;

        Gizmos.DrawSphere(movePoint, .2f);
    }
}
