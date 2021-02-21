using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

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

    public Action PlayerExit;
    public Action PlayerDies;

    public void OnTriggerPlayerDies()
    {
        PlayerDies?.Invoke();
    }

    public Action<Vector3, float> ExplodeBomb;

    public void OnTriggerPlayerExit()
    {
        PlayerExit?.Invoke();
    }

    public void OnTriggerExplodeBomb(Vector3 position, float radius)
    {
        ExplodeBomb?.Invoke(position, radius);
    }
}
