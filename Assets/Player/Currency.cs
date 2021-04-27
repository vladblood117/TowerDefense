using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Currency : MonoBehaviour
{
    //----Private Fields-----
    public static int ID = 6;
    private Canvas Overlay;
    private int gold;
    private TMP_Text label;

    public int Gold { get { return gold; } }
    // Start is called before the first frame update
    void Start()
    {
        Overlay = this.transform.Find("Overlay").gameObject.GetComponent<Canvas>();
        label = Overlay.gameObject.transform.Find("Currency").gameObject.transform.Find("Label").gameObject.GetComponent<TMP_Text>();
        gold = 100;
        Refresh();
    }

    private void Refresh()
    {

        label.text = "$" + gold;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveGold(int value)
    {
        gold -= value;
        if (gold < 0)
        {
            gold = 0;
        }
        Refresh();
    }
    public void AddGold(int value)
    {
        gold += value;
        Refresh();
    }
}
