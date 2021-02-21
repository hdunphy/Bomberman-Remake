using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, IEntityAction
{
    [SerializeField] private int ticksLeft = 3;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask Layers;

    private bool isDead = false;
    private bool isTurnOver = false;

    private void Start()
    {
        EntityMoveManager.Instance.AddEntity(this);
    }

    private void OnDestroy()
    {
        isDead = true;
    }

    public void StartTurnAction()
    {
        isTurnOver = false;
        if (--ticksLeft <= 0)
        {
            ExplodeBomb();
        }
    }

    private void ExplodeBomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
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
        Destroy(gameObject);
        isTurnOver = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawSphere(transform.position, radius);
    }

    public bool IsTurnOver()
    {
        return isTurnOver;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
