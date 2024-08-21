using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public UnitHealth enemyHealth = new UnitHealth(10, 10);
    public int enemyDefence = 2;

    void Awake()
    {
        if (enemyHealth == null)
        {
            enemyHealth = new UnitHealth(10, 10);
            Debug.Log("enemyHealth was null, reinitialized in Awake");
        }
    }
}
