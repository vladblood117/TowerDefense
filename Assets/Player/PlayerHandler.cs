using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject plrObj;
    private Currency currency;

    public static int ID = 2;
    public Currency Currency { get { return currency; } }
    void Start()
    {
        plrObj = this.gameObject;
        currency = plrObj.GetComponent<Currency>();
    }

    // Update is called once per frame
    void Update()
    {

    }



}
