using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBuildings : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<int, Structures> Buildings;
    void Start()
    {
        Buildings = new Dictionary<int, Structures>();
        Object[] load = Resources.LoadAll("Structures", typeof(GameObject));
        foreach (var v in load)
        {
            var z = v as GameObject;
            var strut = z.GetComponent<Structures>();

            Buildings.Add(strut.SID, strut);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
