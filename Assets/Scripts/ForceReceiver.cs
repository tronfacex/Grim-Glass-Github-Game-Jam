using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ForceReceiver : MonoBehaviour
{
    //This is where gravity livess
    [SerializeField] private CharacterController controller;

    [SerializeField] private PlayerStateMachine stateMachine;

    private float verticalVelocity;
    [field: SerializeField] public float gravityModifier { get; private set; }
    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float knockBackDuration = .33f;
    [SerializeField] private float drag = 0.3f;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        //When you add new enemies you will need to add OR statements here, but the syntax on each statemachine should be the same after that check
        if (this.TryGetComponent<MilkBottleEnemyStateMachine>(out MilkBottleEnemyStateMachine stateMachine))
        {
            if (stateMachine.InKnockback && knockBackDuration > 0)
            {
                knockBackDuration -= Time.deltaTime;
            }
            if (stateMachine.InKnockback && knockBackDuration <= 0 && stateMachine.InKnockback)
            {
                stateMachine.InKnockback = false;
            }
        }
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = (Physics.gravity.y * gravityModifier) * Time.deltaTime;
        }
        else
        {
            verticalVelocity += (Physics.gravity.y * gravityModifier) * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;

        //When you add new enemies you will need to add OR statements here, but the syntax on each statemachine should be the same after that check
        if (this.TryGetComponent<MilkBottleEnemyStateMachine>(out MilkBottleEnemyStateMachine stateMachine))
        {
            knockBackDuration = .33f;
            stateMachine.InKnockback = true;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
    public void DoubleJump(float jumpForce)
    {
        StartCoroutine(DoubleJumpGravityDelay(jumpForce));
    }
    private IEnumerator DoubleJumpGravityDelay(float jumpForce)
    {
        //This makes it so that the player hangs in the air briefly before the double jump is applied
        //This is good because it makes a uniform double jump everytime
        ChangeGravityModifier(0f);
        verticalVelocity = 0f;
        yield return new WaitForSeconds(.05f);
        ChangeGravityModifier(1.5f);
        verticalVelocity += jumpForce;
        //Debug.Log("Double Jump Completed");
    }

    public void SlamAttack()
    {
        StartCoroutine(SlamGravityDelay());
    }
    private IEnumerator SlamGravityDelay()
    {
        //This makes it so that the player hangs in the air briefly before the slam attack is applied
        gravityModifier = 0f;
        verticalVelocity = 0f;
        yield return new WaitForSeconds(.05f);
        gravityModifier = 5f;
    }
    public void ReturnGravityModifier()
    {
        gravityModifier = stateMachine.GravityModifier;
    }
    public void FloatGravity()
    {
        if (verticalVelocity > 0)
        {
            verticalVelocity = 0;
        }

        ChangeGravityModifier(0.15f);
    }
    public void JumpCutoffGravity(float delay, float VerticalVelocity)
    {
        //This exists because I trigger the jump force a few frames into the jump animation 
        //This makes the jump animation more natural
        //but if the player releases the jump button immediately we need to give time for the jump force to be applied before we can cut it off
        StartCoroutine(JumpCutoffDelay(delay, VerticalVelocity));
    }
    private IEnumerator JumpCutoffDelay(float delay, float VerticalVelocity)
    {
        yield return new WaitForSeconds(delay);
        //I am setting the vertical velocity to 6 so that the player doesn't immediately drop
        //This way they hang for second before dropping
        if (verticalVelocity > VerticalVelocity)
        {
            verticalVelocity = VerticalVelocity;
        }
    }
    public void ChangeGravityModifier(float modifierValue)
    {
        gravityModifier = modifierValue;
        //Debug.Log("gravity modifier = " + gravityModifier);
    }

    public IEnumerator ChangeGravityModifierDelay(float modifierValue, float delayAmount)
    {
        //Not sure I need this, but it seems useful
        yield return new WaitForSeconds(delayAmount);
        ChangeGravityModifier(modifierValue);
    }
    public void ApplyPlatformVerticalVelocity(float yVelocity)
    {
        verticalVelocity += yVelocity;
    }
}
