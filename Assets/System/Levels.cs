using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : TDSystem

{
    [SerializeField] private int Level;
    private float _roundTime;
    [SerializeField] private float _roundTimeLimit;
    [SerializeField] private float _roundTimeDelay;
    [SerializeField] private int _spawnerCount;

    private Dictionary<int, int> toKillThisRound;
    private List<MobSpawner> _mobSpawners;
    private bool _running;
    private bool _debounce;

    // Start is called before the first frame update
    void Start()
    {
        _debounce = false;
        toKillThisRound = new Dictionary<int, int>();
        _roundTime = 0f;
        _running = false;
        _mobSpawners = new List<MobSpawner>();
        for (int i = 0; i < _spawnerCount; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = this.transform;
            var spwnr = obj.AddComponent<MobSpawner>();
            spwnr.SetSpawnTimer(5f);
            _mobSpawners.Add(spwnr);


        }
        StartCoroutine(WaitForLoaded(Rounds.Loaded, NewRound));
        Debug.Log("----------------");
    }

    // Update is called once per frame
    void Update()
    {
        _roundTime += Time.deltaTime;
        if (!_debounce)
        {
            _debounce = true;
            if (_running)
            {
                if (_roundTime >= _roundTimeLimit)
                {
                    Debug.Log("Jazongaaaa*-*-*-*-*-*-*-*");
                    NewRound();
                }
            }
            else
            {
                if (_roundTime >= _roundTimeDelay)
                {
                    Debug.Log("Start round");
                    StartRound();
                }
            }
            _debounce = false;
        }

    }

    private void NewRound()
    {
        this.Level++;
        if (!Rounds.Loaded) { }
        _running = false;
        _roundTime = 0f;
        Debug.Log(this.Level);
        var count = Rounds.RoundMobs[this.Level];
        foreach (int mobId in Rounds.RoundMobs[this.Level].Keys)
        {
            Debug.Log("From Rounds >>" + Rounds.RoundMobs[this.Level][mobId]);
            if (toKillThisRound.ContainsKey(mobId))
            {
                toKillThisRound[mobId] += Rounds.RoundMobs[this.Level][mobId];
            }
            else
            {
                toKillThisRound[mobId] = Rounds.RoundMobs[this.Level][mobId];
            }
        }
        foreach (MobSpawner m in _mobSpawners)
        {
            m.StopSpawner();
            foreach (int mobId in toKillThisRound.Keys)
            {
                var v = Mathf.CeilToInt(toKillThisRound[mobId] / _mobSpawners.Count);
                m.AddMobs(mobId, v);
            }

        }
    }

    private void StartRound()
    {
        _running = true;
        _roundTime = 0f;
        Debug.Log("Current round level " + this.Level);
        foreach (MobSpawner m in _mobSpawners)
        {
            m.StartSpawner();
        }
    }
}
