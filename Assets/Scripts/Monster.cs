using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IEntityController
{
    [SerializeField] private LayerMask collisionLayer;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        GetPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.otherCollider.TryGetComponent(out Player _player))
        {
            _player.GetComponent<IDeathBehavior>().Die();
        }
    }

    public void GetPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    private Vector2 GetPlayerDirection()
    {
        Vector2 playerDir = Vector2.zero;

        Vector2 difference = player.transform.position - transform.position;

        if (Mathf.Abs(difference.x) > 0)
            playerDir.x = Mathf.Sign(difference.x);
        if (Mathf.Abs(difference.y) > 0)
            playerDir.y = Mathf.Sign(difference.y);
        return playerDir;
    }

    private Vector2 FindMove()
    {
        Vector2 nextMove = transform.position;
        int currentDistance = GetDistanceToPlayer(nextMove);

        foreach (Vector2 move in EntityMoveManager.Directions)
        {
            Vector2 _move = move + new Vector2(transform.position.x, transform.position.y);
            Collider2D collider = Physics2D.OverlapCircle(_move, 0.2f, collisionLayer);
            if (!collider && GetDistanceToPlayer(_move) <= currentDistance)
                nextMove = _move;
        }

        return nextMove;
    }

    private int GetDistanceToPlayer(Vector2 move)
    {
        return Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - move.x) + Mathf.Abs(player.transform.position.y - move.y));
    }

    public Vector2? ReadInput()
    {
        Vector2? move = null;
        if (player != null)
        {
            move = FindMove();
            if (GetDistanceToPlayer(move.Value) == 0)
                player.GetComponent<IDeathBehavior>().Die();
        }

        Debug.Log($"Monster move {move}");
        return move;
    }

    public void EndOfTurnAction()
    {
        //Make sure the position isn't offset and leads to weird collisions
        transform.position = Vector3Int.RoundToInt(transform.position);
    }
}
