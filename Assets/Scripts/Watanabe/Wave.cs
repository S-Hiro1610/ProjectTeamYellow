using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public GameObject Enemy => _enemy;
    public int Level => _level;

    public float SpawneDelay => _spawneDelay;
    public int SpawnUnits => _spawnUnits;

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private int _level;
    [SerializeField]
    private float _spawneDelay;
    [SerializeField]
    private int _spawnUnits;
}
