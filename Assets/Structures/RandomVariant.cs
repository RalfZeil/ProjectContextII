using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVariant : MonoBehaviour
{
    [SerializeField] private GameObject[] models;

    void Start()
    {
        models[Random.Range(0, models.Length)].SetActive(false);
    }
}
