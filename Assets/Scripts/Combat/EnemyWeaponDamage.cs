using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponDamage : MonoBehaviour
{
    //This script exists so that enemies can't hurt each other
    //It is a mirror of the WeaponDamage script use it for Enemy Weapons
    [SerializeField] private Collider myCollider;
    [SerializeField] private Collider enemyDetectionCollider;

    [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();

    private int damage;
    private float knockdown;
    private float knockbackDistance;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (enemyDetectionCollider != null)
        {
            if (other == enemyDetectionCollider)
            {
                return;
            }
        }
        if (!other.CompareTag("Player")) { return; }

        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Knockdown>(out Knockdown Knockdown))
        {
            Knockdown.DealKnockdown(knockdown);
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockbackDistance);
        }
    }

    public void SetAttack(int damageAmount, float knockbackAmount, float knockdownAmount)
    {
        this.damage = damageAmount;
        this.knockbackDistance = knockbackAmount;
        this.knockdown = knockdownAmount;
    }
}
