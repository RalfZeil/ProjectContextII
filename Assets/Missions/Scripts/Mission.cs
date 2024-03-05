using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public virtual bool IsCompleted()
    {
        return false;
    }
    public virtual void GetReward()
    {

    }
}
