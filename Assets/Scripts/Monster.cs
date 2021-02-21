using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityGridMove))]
public class Monster : MonoBehaviour
{
    [SerializeField] private EntityGridMove EntityGridMove;
    [SerializeField] private LayerMask collisionLayer;

    private Player player;
    private Vector2 PlayerLastPosition;
    private bool isDead = false;
    private readonly List<Vector2> Directions = new List<Vector2>() { Vector2.up, Vector2.left, Vector2.right, Vector2.down };

    // Start is called before the first frame update
    void Start()
    {
        EntityMoveManager.Instance.AddEntity(EntityGridMove);
        GetPlayer();
        EntityGridMove.PreActionCallback += SetMove;
    }

    private void OnDestroy()
    {
        EntityGridMove.PreActionCallback -= SetMove;
        isDead = true;
    }

    public void GetPlayer()
    {
        player = FindObjectOfType<Player>();
        PlayerLastPosition = player.transform.position;
    }

    private void Update()
    {
        if (EntityGridMove.isAtMovePoint)
            EntityGridMove.SetInputVector(Vector2.zero);
    }

    private void SetMove()
    {
        if (player != null)
        {
            Vector2 move = FindMove();
            if (move.x == 0 && move.y == 0)
                EntityGridMove.UsedAction();
            else
                EntityGridMove.SetInputVector(move);
        }
    }

    private Vector2 GetPlayerDirection()
    {
        Vector2 playerDir = Vector2.zero;

        Vector2 difference = player.transform.position - transform.position;

        if (Mathf.Abs(difference.x) > 0)
            playerDir.x = Mathf.Sign(difference.x);
        if (Mathf.Abs(difference.y) > 0)
            playerDir.y = Mathf.Sign(difference.y);
        //Vector2 playerPos = player.transform.position;
        //Vector2 myPos = transform.position;
        //if (playerPos.x - myPos.x > 0)
        //    playerDir.x = 1;
        //else if (playerPos.x - myPos.x < 0)
        //    playerDir.x = -1;
        return playerDir;
    }

    private Vector2 FindMove()
    {
        Vector2 nextMove = Vector2.zero;
        int currentDistance = GetDistanceToPlayer(transform.position);

        foreach (Vector2 move in Directions)
        {
            Vector2 _move = move + new Vector2(transform.position.x, transform.position.y);
            if (!Physics2D.OverlapCircle(_move, 0.2f, collisionLayer) && GetDistanceToPlayer(_move) <= currentDistance)
                nextMove = move;
        }

        return nextMove;
    }

    private int GetDistanceToPlayer(Vector2 move)
    {
        return Mathf.RoundToInt(Mathf.Abs(player.transform.position.x - move.x) + Mathf.Abs(player.transform.position.y - move.y));
    }

    //public bool IsDead()
    //{
    //    return isDead;
    //}

    //public bool IsTurnOver()
    //{
    //    return true;
    //}

    //public void StartTurnAction()
    //{
    //    if(player != null)
    //    {
    //        var Move = FindMove() + new Vector2(transform.position.x, transform.position.y);
    //        StartCoroutine(MoveToPosition(Move));
    //    }
    //}

    //private IEnumerator MoveToPosition(Vector3 movePoint)
    //{
    //    while (Vector3.Distance(transform.position, movePoint) > 0.05f)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, movePoint, 5 * Time.deltaTime);
    //        yield return null;
    //    }
    //}
}
