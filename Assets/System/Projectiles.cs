using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] public float ProjectileSpeed;
    private GameObject _target;
    private int _damage;
    private bool _tracking;
    private float delta;
    private bool debounce;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        delta = 0f;
        debounce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tracking)
        {
            delta = Mathf.Clamp(delta + Time.deltaTime, 0f, ProjectileSpeed);
            if (!debounce)
            {
                debounce = true;
                print("Tracking!");
                if (_target != null)
                {
                    print("Fire speed");
                    print(Time.deltaTime * ProjectileSpeed);
                    gameObject.transform.position = Vector3.Lerp(_startPosition, _target.transform.position, delta);
                    var mag = (gameObject.transform.position - _target.transform.position).magnitude;
                    if (mag <= .3f)
                    {
                        HitTarget();
                    }
                }
                else
                {
                    print("Enemy died, bye bye");
                    Destroy(gameObject);
                }
                debounce = false;
            }
        }
    }
    public void SetTarget(GameObject target) { _target = target; }
    public void SetDamage(int damage) { _damage = damage; }

    public void Fire()
    {
        _startPosition = transform.position;
        _tracking = true;
        gameObject.SetActive(true);
    }

    public void HitTarget()
    {
        HealthHandler h = _target.GetComponent<HealthHandler>();
        h.TakeDamage(_damage);
        if (h.CurrentHealth <= 0)
        {
            Debug.Log("Enemy is dead");
            _target = null;
            _tracking = false;
        }
        Destroy(gameObject);
    }

}
