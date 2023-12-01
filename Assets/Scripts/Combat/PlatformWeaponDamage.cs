using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWeaponDamage : MonoBehaviour
{
    //This script exists so that enemies can't hurt each other
    //It is a mirror of the WeaponDamage script use it for Enemy Weapons
    [SerializeField] private Collider myCollider;
    [SerializeField] private Collider enemyDetectionCollider;

    [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();

    [SerializeField] private int damage;
    [SerializeField] private float knockdown;
    [SerializeField] private float knockbackDistance;

    [SerializeField] private GameEventScriptableObject PlatformImpactTakenEvent;

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

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            PlatformImpactTakenEvent?.Raise();
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockbackDistance);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        alreadyCollidedWith.Clear();
    }

    public void SetAttack(int damageAmount, float knockbackAmount, float knockdownAmount)
    {
        this.damage = damageAmount;
        this.knockbackDistance = knockbackAmount;
        this.knockdown = knockdownAmount;
    }
}

