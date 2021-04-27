using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TDSystem : MonoBehaviour
{

    public static int ID = 1;
    public interface IMTBehaviour
    {
        int ClassID { get; }
    }
}
