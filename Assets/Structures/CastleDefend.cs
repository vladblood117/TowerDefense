using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDefend : MonoBehaviour
{
    HealthHandler _health;
    public HealthHandler.OnDeath _method;
    private SpriteRenderer _renderer;
    private static List<CastleDefend> _defend = new List<CastleDefend>();
    // Start is called before the first frame update
    void Start()
    {
        _health = this.gameObject.GetComponent<HealthHandler>();
        _method = OnDeath;
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();
        _health.RegisterDeathMethod(_method);
        _defend.Add(this);
    }

    void OnDeath(GameObject source)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static CastleDefend GetStructure(GameObject caller)
    {
        CastleDefend result = null;
        foreach (CastleDefend d in _defend)
        {

            if (result == null)
            {
                result = d;
            }
            else
            {
                var magA = (caller.transform.position - d.transform.position).magnitude;
                var magB = (caller.transform.position - result.transform.position).magnitude;
                if (magA < magB)
                {
                    result = d;
                }
            }
        }
        return result;
    }

    public void BuildingTakeDamage(MobHandler MH)
    {
        if (MH)
        {
            _health.TakeDamage(MH.gameObject, MH.Damage);
            Debug.Log(_health.CurrentHealth);
            Debug.Log(_health.MaxHealth);
            var p = (float)_health.CurrentHealth / _health.MaxHealth;
            Debug.Log(p);
            _renderer.material.SetFloat("_Health", p);
            MH.RemoveMob();
        }
    }
}
