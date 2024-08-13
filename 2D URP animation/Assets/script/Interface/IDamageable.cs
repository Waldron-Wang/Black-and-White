using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int MaxHealth { get; set; }
    int Health { get; set; }
    void Damage(int damageAmount);
    void Heal(int healAmount);
    void Die();
}
