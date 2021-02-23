using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, IEntityAction
{
    [SerializeField] private int ticksLeft = 3;
    [SerializeField] private float radius = 1f;
    [SerializeField] private SpriteRenderer BombSprite;
    [SerializeField] private AudioSource BombAudio;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Sprite> BombStates;
    [SerializeField] private TMPro.TextMeshProUGUI Text;
    [SerializeField] private GameObject Fire;

    private Dictionary<MoveState, MoveStateBase> ActionState;
    private MoveState CurrentMove;
    private bool isDead = false;
    private bool isTurnOver = false;

    private void Start()
    {
        EntityMoveManager.Instance.AddEntity(this);
        ActionState = new Dictionary<MoveState, MoveStateBase>() {
            { MoveState.Waiting, MoveStateFactory.CreateMoveState(MoveState.Waiting, this)},
            { MoveState.HasMove, MoveStateFactory.CreateMoveState(MoveState.HasMove, this)},
            { MoveState.InProcess, MoveStateFactory.CreateMoveState(MoveState.InProcess, this)},
            { MoveState.Done, MoveStateFactory.CreateMoveState(MoveState.Done, this)},
        };
        CurrentMove = MoveState.Waiting;

        Text.text = ticksLeft.ToString();
        animator.enabled = false;
    }

    private void OnDestroy()
    {
        isDead = true;
    }

    private void ExplodeBomb()
    {
        Text.enabled = false;
        BombAudio.Play();
        animator.enabled = true;

        Vector2 start = transform.position - Vector3.one * radius;
        Vector2 end = transform.position + Vector3.one * radius;

        int begin = Mathf.RoundToInt(-1 * radius);
        for (int i = begin; i < radius; i++)
        {
            for (int j = begin; j < radius; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    GameObject _flame = Instantiate(Fire, new Vector3(i, j) + transform.position, Quaternion.identity);
                    Destroy(_flame, .5f);
                }
            }
        }

        Collider2D[] colliders = Physics2D.OverlapAreaAll(start, end);
        foreach (Collider2D _collider in colliders)
        {
            if (_collider.TryGetComponent(out IDeathBehavior obj))
            {
                obj.Die();
            }
        }

        //BombSprite.gameObject.SetActive(false);
        isTurnOver = true;
        isDead = true;
        Destroy(gameObject, 0.5f);
    }

    //private void OnDrawGizmos()
    //{
    //    if (!isTurnOver)
    //    {
    //        Gizmos.color = Color.white;

    //        Gizmos.DrawCube(transform.position, radius * Vector3.one);
    //    }
    //}

    public bool IsDead()
    {
        return isDead;
    }

    public bool UpdateState()
    {
        CurrentMove = ActionState[CurrentMove].UpdateState();

        return (CurrentMove.Equals(MoveState.Done));
    }

    public bool ReadInput()
    {
        //Has no input
        return true;
    }

    public void StartMove()
    {
        //TODO: Update countdown UI
        isTurnOver = false;

        ticksLeft--;
        Text.text = ticksLeft.ToString();
        if (ticksLeft <= 0)
        {
            ExplodeBomb();
        }
        else
        {
            BombSprite.sprite = BombStates[BombStates.Count - ticksLeft];
        }
        isTurnOver = true;
    }

    public bool IsActionComplete()
    {
        return isTurnOver;
    }

    public void EndOfTurnAction()
    {
        //Nothing
    }
}
