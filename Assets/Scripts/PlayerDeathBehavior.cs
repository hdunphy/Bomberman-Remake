using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathBehavior : MonoBehaviour, IDeathBehavior
{
    public void Die()
    {
        Invoke(nameof(DeathActions), 1f);
        GetComponent<Player>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void DeathActions()
    {
        EventManager.Instance.OnTriggerPlayerDies();
        Destroy(gameObject);
    }
}
