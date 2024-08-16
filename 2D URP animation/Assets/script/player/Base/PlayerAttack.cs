using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Collider2D[][] attackColliders;
    Collider2D[] HitObjects;

    Player player;

    private void Start()
    {
        player = gameObject.GetComponent<Player>();

        attackColliders = new Collider2D[3][];

        attackColliders[0] = transform.Find("attack trigger/attack1").GetComponentsInChildren<Collider2D>();
        attackColliders[1] = transform.Find("attack trigger/attack2").GetComponentsInChildren<Collider2D>();
        attackColliders[2] = transform.Find("attack trigger/attack3").GetComponentsInChildren<Collider2D>();

        // disable all colliders
        foreach (var attack in attackColliders)
        {
            foreach (Collider2D collider in attack)
            {
                collider.enabled = false;
            }
        }
    }

    // enable collider (一共9个trigger，colliderIndex则为1-9)
    private void EnableAttackCollider(int colliderIndex)
    {
        int attackIndex = (colliderIndex - 1) / 3;
        colliderIndex = (colliderIndex - 1) % 3;

        if (attackIndex >= 0 && attackIndex < attackColliders.Length)
        {
            if (colliderIndex >= 0 && colliderIndex < attackColliders[attackIndex].Length)
            {
                attackColliders[attackIndex][colliderIndex].enabled = true;
            }
        }
    }

    // disable collider    
    private void DisableAttackColliders(int colliderIndex)
    {
        int attackIndex = (colliderIndex - 1) / 3;
        colliderIndex = (colliderIndex - 1) % 3;
        
        if (attackIndex >= 0 && attackIndex < attackColliders.Length)
        {
            if (colliderIndex >= 0 && colliderIndex < attackColliders[attackIndex].Length)
            {
                attackColliders[attackIndex][colliderIndex].enabled = false;
            }
        }
    }

    private int DamageAmount()
    {
        int damageAmount;

        damageAmount = 1;

        return damageAmount;
    }

    // detect enemies
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            //damageable.Damage(DamageAmount());
            UnitHealth enemyHealth = other.gameObject.GetComponent<TestEnemy>().enemyHealth;
            if (enemyHealth != null)
            {
                enemyHealth.Damage(DamageAmount());
                Debug.Log("enemy health: " + enemyHealth.Health);
            }
        }
    }
}