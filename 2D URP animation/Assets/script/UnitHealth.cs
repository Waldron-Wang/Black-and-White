using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IDamageable
{
    int CurrentHealth;
    int CurrentMaxHealth;

    public int Health
    {
        get { return CurrentHealth; }
        set { CurrentHealth = value; }
    }

    public int MaxHealth
    {
        get { return CurrentMaxHealth; }
        set { CurrentMaxHealth = value; }
    }

    public UnitHealth(int health, int maxHealth)
    {
        CurrentHealth = health;
        CurrentMaxHealth = maxHealth;
    }

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
