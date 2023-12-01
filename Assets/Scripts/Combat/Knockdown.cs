using System;
using UnityEngine;

public class Knockdown : MonoBehaviour
{
    [SerializeField] private CharPropertiesSO characterProperties;

    [SerializeField] public float knockDownThreshold { get; private set; }
    [SerializeField] public float knockDownAmount;
    [SerializeField] public float knockDownAmountDecayRate { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        knockDownThreshold = characterProperties.KnockDownThreshold;
        knockDownAmount = 0f;
        knockDownAmountDecayRate = characterProperties.KnockDownAmountDecayRate;
    }

    private void Update()
    {
        if (knockDownAmount > 0)
        {
            knockDownAmount -= knockDownAmountDecayRate * Time.deltaTime;
            knockDownAmount = Mathf.Max(knockDownAmount, 0);
        }
    }

    public void DealKnockdown(float weaponKnockDownAmount)
    {
        if (knockDownAmount >= knockDownThreshold) { return; }

        knockDownAmount = Mathf.Min(knockDownAmount + weaponKnockDownAmount, knockDownThreshold);

        MilkBottleEnemyStateMachine stateMachine = GetComponent<MilkBottleEnemyStateMachine>();
        if (knockDownAmount >= knockDownThreshold)
        {
            knockDownAmount = 0;
            Debug.Log("KNOCKDOWN THRESHOLD REACHED !!!! Switch to Knockdown state");
            stateMachine.SwitchState(new MilkBottleEnemyKnockdownState(stateMachine));
        }

    }
}
