using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathBehavior : MonoBehaviour, IDeathBehavior
{
    public void Die()
    {
        EventManager.Instance.OnTriggerPlayerDies();
        Destroy(gameObject);
    }
}
