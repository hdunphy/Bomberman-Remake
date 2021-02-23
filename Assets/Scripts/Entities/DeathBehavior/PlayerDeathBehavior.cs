using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathBehavior : MonoBehaviour, IDeathBehavior
{
    public void Die()
    {
        StartCoroutine(DeathActions());
        GetComponent<Player>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator DeathActions()
    {
        yield return new WaitForSeconds(1f);
        EventManager.Instance.OnTriggerPlayerDies();
        Destroy(gameObject);
    }
}
