using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : TDSystem

{
    private int Level;
    private float _roundTime;
    [SerializeField] private float _roundTimeDelay;
    [SerializeField] private int _spawnerCount;
    [SerializeField] private List<GameObject> _spawners;

    private Dictionary<int, int> toKillThisRound;
    private static Levels _thisLevel;
    private int _KilledThisRound;
    private int _toKillThisRound;
    private static List<MobSpawner> _mobSpawners;
    private bool _running;
    private bool _debounce;

    // Start is called before the first frame update
    void Start()
    {
        _thisLevel = this;
        Level = 0;
        _debounce = false;
        toKillThisRound = new Dictionary<int, int>();
        _roundTime = 0f;
        _running = false;
        _mobSpawners = new List<MobSpawner>();
        for (int i = 0; i < _spawnerCount; i++)
        {
            var r = Random.Range(1, _spawners.Count + 1);
            GameObject obj = _spawners[r];
            var spwnr = obj.AddComponent<MobSpawner>();
            spwnr.SetSpawnTimer(5f);
            _mobSpawners.Add(spwnr);
            _spawners.RemoveAt(r);

        }
        StartCoroutine(WaitForLoaded(Rounds.Loaded, NewRound));

        Debug.Log("----------------");
    }
    public static void IncrementKilledThisRound()
    {
        _thisLevel._KilledThisRound++;
        if (_thisLevel._KilledThisRound >= _thisLevel._toKillThisRound)
        {
            _thisLevel.NewRound();
        }
    }

    private void NewRound()
    {
        Level++;
        Debug.Log("New round start!");
        if (!Rounds.Loaded) { }
        _running = false;
        _KilledThisRound = 0;
        _toKillThisRound = 0;
        _roundTime = 0f;
        Debug.Log(Level);
        var count = Rounds.RoundMobs[Level];
        foreach (int mobId in Rounds.RoundMobs[Level].Keys)
        {
            Debug.Log("From Rounds >>" + Rounds.RoundMobs[Level][mobId]);
            if (toKillThisRound.ContainsKey(mobId))
            {
                toKillThisRound[mobId] += Rounds.RoundMobs[Level][mobId];
            }
            else
            {
                toKillThisRound[mobId] = Rounds.RoundMobs[Level][mobId];
            }
        }
        foreach (MobSpawner m in _mobSpawners)
        {
            m.StopSpawner();
            foreach (int mobId in toKillThisRound.Keys)
            {
                var v = Mathf.CeilToInt(toKillThisRound[mobId] / _mobSpawners.Count);
                m.AddMobs(mobId, v);
                _toKillThisRound += v;
            }

        }
        Debug.Log("Start round");
        StartCoroutine(WaitForSeconds((float)_roundTimeDelay, StartRound));
    }

    private void StartRound()
    {
        _running = true;
        _roundTime = 0f;
        Debug.Log("Current round level " + Level);
        foreach (MobSpawner m in _mobSpawners)
        {
            m.StartSpawner();
        }
    }
}
