using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageable
{
    public int MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public UnitHealth enemyHealth = new UnitHealth(10, 10);

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;

        if (Health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;

        if (Health >= MaxHealth)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
