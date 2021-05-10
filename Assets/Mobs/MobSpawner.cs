using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static int ID = 10;
    private Dictionary<int, int> Spawns;
    private float _spawnRate;
    public float SpawnRate { get { return _spawnRate; } }
    private bool _running;
    public bool Running { get { return _running; } }

    private float deltaTime;
    private bool _debounce;
    void Start()
    {
        deltaTime = 0f;
        _debounce = false;
        Spawns = new Dictionary<int, int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_running)
        {
            if (!_debounce)
            {
                _debounce = true;
                deltaTime += Time.deltaTime;
                if (deltaTime >= SpawnRate)
                {
                    GameObject spawn = GetMob();
                    if (spawn != null)
                    {
                        deltaTime = 0f;
                        spawn.transform.parent = this.gameObject.transform;
                        spawn.transform.localPosition = Vector3.zero;
                    }
                }
                _debounce = false;
            }
        }
    }

    private GameObject GetMob()
    {
        GameObject mob = null;
        var r = Random.Range(1, Spawns.Count + 1);
        var MobCount = Spawns[r];
        if (MobCount > 0)
        {
            mob = (GameObject)Instantiate(Rounds.AllMobs[r].gameObject) as GameObject;
            Spawns[r] -= 1;
        }
        return mob;
    }

    public void StartSpawner()
    {
        _running = true;
    }
    public void StopSpawner()
    {
        _running = false;
    }

    public void AddMobs(int MobId, int Quantity)
    {
        if (Spawns.ContainsKey(MobId))
        {
            Spawns[MobId] = Mathf.Max(Spawns[MobId] + Quantity, Quantity);
        }
        else
        {
            Spawns[MobId] = Quantity;
        }
    }

    public void SetSpawnTimer(float value)
    {
        _spawnRate = value;
    }

}
