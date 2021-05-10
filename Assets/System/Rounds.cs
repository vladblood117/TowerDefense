using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rounds : MonoBehaviour
{
    public readonly static Dictionary<int, Dictionary<int, int>> RoundMobs = new Dictionary<int, Dictionary<int, int>>();
    public readonly static Dictionary<int, MobHandler> AllMobs = new Dictionary<int, MobHandler>();

    public static bool Loaded = false;

    void Awake()
    {
        var loadMobs = Resources.LoadAll("Mobs", typeof(GameObject));
        foreach (GameObject obj in loadMobs)
        {
            MobHandler mh = obj.GetComponent<MobHandler>();
            AllMobs[mh.MobId] = mh;
        }

        RoundMobs[1] = new Dictionary<int, int>();
        RoundMobs[1][1] = 5; //Round 1, Mob ID 1, Quantity 5
        RoundMobs[1][2] = 5;

        RoundMobs[2] = new Dictionary<int, int>();
        RoundMobs[2][1] = 7; //Round 2, Mob ID 1, Quantity 7
        RoundMobs[2][2] = 10;

        RoundMobs[3] = new Dictionary<int, int>();
        RoundMobs[3][1] = 12; //Round 3, Mob ID 1, Quantity 12
        RoundMobs[3][2] = 13;
        Debug.Log(RoundMobs[1].Count);
        Loaded = true;
    }
}
