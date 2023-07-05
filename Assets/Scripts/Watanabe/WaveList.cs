using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWave.asset", menuName = "ScriptableObjects/Wave")]
public class WaveList : ScriptableObject
{
    public List<Wave> waveEnemy;
}
