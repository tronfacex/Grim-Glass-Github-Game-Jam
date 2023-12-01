using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public CharPropertiesSO characterProperties;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public event Action OnPlatformFailTakeDamage;

    //[SerializeField] private GameEventScriptableObject OnTakePlatformDamageEvent;
    //[SerializeField] private GameEventScriptableObject OnDieEvent;

    [field: SerializeField] public int maxHealth { get; private set; }
    [field: SerializeField] public int health { get; private set; }
    [SerializeField] public bool IsInvincible;
    //[SerializeField] public bool IsMilkBottleBoss1;



    // Start is called before the first frame updates
    void Start()
    {
        maxHealth = characterProperties.MaxHealth;
        health = maxHealth;
        if (gameObject.CompareTag("Player"))
        {
            GameDataReader.Instance.GameData.PlayerHealth = health;
            //Debug.Log("Player Health Data Object " + GameDataReader.Instance.GameData.PlayerHealth);
        }
        /*else
        {
            if (gameObject.name == "Milk Bottle Boss")
            {
                GameDataReader.Instance.GameData.MilkBoss1Health = health;
                IsMilkBottleBoss1 = true;
            }
            if (gameObject.name == "Milk Bottle Boss (1)")
            {
                GameDataReader.Instance.GameData.MilkBoss2Health = health;
                IsMilkBottleBoss1 = false;
            }

            //Debug.Log("Player Health Data Object " + GameDataReader.Instance.GameData.PlayerHealth);
        }*/
    }

    public void DealDamage(int damageAmount)
    {
        if (health == 0) { return; }

        if (IsInvincible) { return; }

        health = Mathf.Max(health - damageAmount, 0);

        if (gameObject.CompareTag("Player"))
        {
            GameDataReader.Instance.GameData.PlayerHealth = health;
            //Debug.Log("Player Health Data Object " + GameDataReader.Instance.GameData.PlayerHealth);
        }
        /*else
        {
            if (IsMilkBottleBoss1)
            {
                GameDataReader.Instance.GameData.MilkBoss1Health = health;

            }
            else
            {
                GameDataReader.Instance.GameData.MilkBoss2Health = health;

            }
        }*/

        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            //Destroy(gameObject);
            OnDie?.Invoke();
        }
    }

    public void ResetHealth()
    {
        maxHealth = characterProperties.MaxHealth;
        health = maxHealth;
    }

    public void DealPlatformFailDamage()
    {
        if (health == 0)
        {
            OnDie?.Invoke();
        }
        else
        {
            OnPlatformFailTakeDamage?.Invoke();
        }
        if (IsInvincible)
        {
            health++;
            GameDataReader.Instance.GameData.PlayerHealth = health;
        }
    }
}
