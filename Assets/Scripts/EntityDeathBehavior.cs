using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDeathBehavior : MonoBehaviour, IDeathBehavior
{
    public void Die()
    {
        Destroy(gameObject);
    }
}
