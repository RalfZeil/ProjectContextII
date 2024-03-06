using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    [HideInInspector] public bool isJustCompleted = false;
    public virtual bool IsCompleted()
    {
        return false;
    }
    public virtual void GetReward()
    {

    }
}
