using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TDSystem : MonoBehaviour
{

    public static int ID = 1;

    private float _childWait = 0f;
    public interface ISystem
    {

    }

    public IEnumerator SetupChildWait()
    {
        Debug.Log("Perform a wait action");
        yield return new WaitForSeconds(.1f);
        _childWait += .1f;
        Debug.Log("Waited for child");
    }

    public GameObject WaitForChild(GameObject parent, int id)
    {
        GameObject child = parent.transform.GetChild(id).gameObject;
        if (child == null)
        {
            if (_childWait < 3f)
            {
                StartCoroutine(SetupChildWait());
                child = parent.transform.GetChild(id).gameObject;

            }
        }
        _childWait = 0f;
        return child;
    }
}
