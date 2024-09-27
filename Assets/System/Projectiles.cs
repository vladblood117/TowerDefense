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

    private PlayerHandler _owner;

    // Start is called before the first frame update
    void Start()
    {
        delta = 0f;
        debounce = false;
    }

    public void SetOwner(PlayerHandler owner) { _owner = owner; }

    // Update is called once per frame
    void Update()
    {
        if (_tracking)
        {

            delta = (delta + Time.deltaTime);
            if (!debounce)
            {
                debounce = true;
                if (_target != null)
                {
                    var dir = (_target.transform.position - gameObject.transform.position).normalized;
                    gameObject.transform.position = gameObject.transform.position + (dir * ProjectileSpeed * Time.deltaTime);  //Vector3.Lerp(_startPosition, _target.transform.position, delta * ProjectileSpeed);
                    var mag = (gameObject.transform.position - _target.transform.position).magnitude;
                    if (mag <= .3f)
                    {
                        HitTarget();
                    }
                }
                else
                {
                    delta = 0f;
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
        h.TakeDamage(_owner.gameObject, _damage);
        if (h.CurrentHealth <= 0)
        {
            _target = null;
            _tracking = false;
            delta = 0f;
        }
        Destroy(gameObject);
    }

}
