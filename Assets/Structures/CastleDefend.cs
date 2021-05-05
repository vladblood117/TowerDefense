using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDefend : MonoBehaviour
{
    HealthHandler _health;
    public HealthHandler.OnDeath _method;
    // Start is called before the first frame update
    void Start()
    {
        _health = this.gameObject.GetComponent<HealthHandler>();
        _method = OnDeath;
        _health.RegisterDeathMethod(_method);
    }

    void OnDeath()
    {
        Destroy(gameObject);
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
            Debug.Log(_health.CurrentHealth);
            Debug.Log(_health.MaxHealth);
            var p = (float)_health.CurrentHealth / _health.MaxHealth;
            Debug.Log(p);
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Health", p);
            Destroy(collision.gameObject);
        }
    }
}
