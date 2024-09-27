using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static int ID = 9;
    [SerializeField] public int MaxHealth;

    public delegate void OnDeath(GameObject source);
    public OnDeath m_methodToCall;

    private int _health;
    public int CurrentHealth { get { return _health; } }
    void Start()
    {
        _health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterDeathMethod(OnDeath m)
    {
        m_methodToCall = m;
    }

    public void TakeDamage(GameObject source, int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            m_methodToCall(source);

        }
    }

}
