using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int ticksLeft = 3;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask Layers;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.MoveUpdate += UpdateBomb;
    }

    private void OnDestroy()
    {
        EventManager.Instance.MoveUpdate -= UpdateBomb;
    }

    private void UpdateBomb()
    {
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
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawSphere(transform.position, radius);
    }
}
