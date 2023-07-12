using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreateTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestUnitCreate(int index)
    {
        UnitManager.Instance.UnitCreate((UnitType)index, new Vector3(-3, 1, 0));
    }

    public void TestUnitLevelup(int index)
    {
        UnitManager.Instance.LevelUp((UnitType)index);
    }
}
