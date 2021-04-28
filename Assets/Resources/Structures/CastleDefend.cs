using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDefend : MonoBehaviour
{
    HealthHandler _health;
    // Start is called before the first frame update
    void Start()
    {
        _health = this.gameObject.GetComponent<HealthHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var MH = collision.gameObject.GetComponent<MobHandler>();
        if (MH)
        {
            _health.TakeDamage(MH.Damage);
            Destroy(collision.gameObject);
        }
    }
}
