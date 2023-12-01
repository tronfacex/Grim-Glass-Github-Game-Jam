using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeWeaponInstantiator : MonoBehaviour
{
    //This script is here to be triggered in the enemy animator to throw prefabs
    //It had to be separated out into it's own component, so that the ThrowPrefabForward method can be called from within the animation

    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float throwForce = 400;
    [SerializeField] private Transform enemyTransform;


    public void ThrowPrefabForward()
    {
        GameObject thrownObject = Instantiate(objectPrefab, new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z), Quaternion.identity);
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

        rb.AddForce((enemyTransform.forward) * throwForce, ForceMode.Impulse);
    }
}
