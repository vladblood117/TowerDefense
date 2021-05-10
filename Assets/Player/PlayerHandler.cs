using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Start is called before the first frame update 
    private Currency _currency;
    private Handler _overlay;
    public Currency Currency { get { return _currency; } }

    private Buildings _building;
    public Buildings Building { get { return _building; } }
    void Start()
    {
        _currency = gameObject.GetComponent<Currency>();
        _overlay = gameObject.transform.GetChild(0).GetComponent<Handler>();
        _building = gameObject.GetComponent<Buildings>();
    }





}
