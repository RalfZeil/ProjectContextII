using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public GameGrid gameGrid;

    public void Start()
    {
        GridManager.Instance = this;
    }
}
