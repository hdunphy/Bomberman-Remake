using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveManager : MonoBehaviour
{
    private List<IEntityAction> Entities;
    private int CurrentIndex;

    public static EntityMoveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        CurrentIndex = 0;
        Entities = new List<IEntityAction>();
    }

    private void Update()
    {
        if(Entities.Count > 0)
        {
            UpdateAction();
        }
    }

    private void UpdateAction()
    {
        //Need to initialize with startturnaction
        IEntityAction CurrentEntity = Entities[CurrentIndex];
        
        if (CurrentEntity == null || CurrentEntity.IsDead())
        {
            Entities.RemoveAt(CurrentIndex);
            UpdateIndex();
        }
        else if (CurrentEntity.UpdateState())
        {
            UpdateIndex();
        }
    }

    private void UpdateIndex()
    {
        CurrentIndex++;
        if (CurrentIndex >= Entities.Count)
            CurrentIndex = 0;
        Debug.Log($"Current Index: {CurrentIndex}");
    }

    public void AddEntity(IEntityAction entity)
    {
        Entities.Add(entity);
    }
}
