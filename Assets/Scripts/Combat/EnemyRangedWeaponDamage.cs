using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedWeaponDamage : MonoBehaviour
{
    //This is basically another WeaponDamage script, but it's for the Throw Prefab objects
    //It is applied to the projectile prefab
    //It also destroys itself lol

    [SerializeField] private Collider myCollider;
    [SerializeField] private Collider enemyDetectionCollider;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private WaitForSeconds ExpirationDelay = new WaitForSeconds(2);

    [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();

    private int damage = 1;
    private float knockbackDistance = 40;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
        enemyDetectionCollider = GameObject.Find("Targeter").GetComponent<SphereCollider>();

        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
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

    public void SetAttack(int damageAmount, float knockbackAmount)
    {
        this.damage = damageAmount;
        this.knockbackDistance = knockbackAmount;
    }

    private void Update()
    {
        if (rb.velocity == Vector3.zero)
        {
            StartCoroutine(ExpirePrefab());
        }
    }

    private IEnumerator ExpirePrefab()
    {
        yield return ExpirationDelay;
        Destroy(gameObject);
    }
}
