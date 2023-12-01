using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
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

        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Knockdown>(out Knockdown Knockdown))
        {
            Knockdown.DealKnockdown(knockdown);
            Debug.Log("Knockdown Amount: " + Knockdown.knockDownAmount + " Threshold: " + Knockdown.knockDownThreshold);
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
            Debug.Log(health.health);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Debug.Log("Player cheat distance");
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            Debug.Log(other.name + " force direction = " + direction);
            forceReceiver.AddForce(direction * knockbackDistance);
            Debug.Log(other.name + " should have been knocked back");
        }
    }

    public void SetAttack(int damageAmount, float knockbackAmount, float knockdownAmount)
    {
        this.damage = damageAmount;
        this.knockbackDistance = knockbackAmount;
        this.knockdown = knockdownAmount;
    }
}
