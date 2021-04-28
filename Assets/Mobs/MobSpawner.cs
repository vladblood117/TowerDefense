using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static int ID = 10;
    [SerializeField] public List<Object> Spawns;
    [SerializeField] public float SpawnRate;
    [SerializeField] public GameObject AttackObj;

    private float deltaTime;
    void Start()
    {
        deltaTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= SpawnRate)
        {
            deltaTime = 0;
            GameObject spawn = (GameObject)Instantiate(Spawns[0]) as GameObject;
            spawn.transform.position = this.transform.position;
            spawn.transform.parent = this.gameObject.transform;
        }
    }
}
