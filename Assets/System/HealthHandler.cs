using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static int ID = 9;
    [SerializeField] public int MaxHealth;

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

    public void TakeDamage(int damage)
    {
        print("Taking damage");
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
            print("Destroyed!");
        }
    }

}
