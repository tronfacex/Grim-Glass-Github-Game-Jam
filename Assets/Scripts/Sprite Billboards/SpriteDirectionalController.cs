using UnityEngine;

public class SpriteDirectionalController : MonoBehaviour
{
    private Transform mainCameraTransform;
    [Range(0f, 180f)] [SerializeField] private float backAngle = 65f;
    [Range(0f, 180f)] [SerializeField] private float sideAngle = 155f;
    [SerializeField] private Transform mainTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(mainCameraTransform.forward.x, 0f, mainCameraTransform.forward.z);

        Debug.DrawRay(mainCameraTransform.position, camForwardVector * 5f, Color.magenta);

        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);

        Vector2 animationDirection = new Vector2(0f, -1f);

        float angle = Mathf.Abs(signedAngle);

        if (angle < backAngle)
        {
            //Play back animation
            animationDirection = new Vector2(0f, -1f);
        }
        else if (angle < sideAngle)
        {
            //Play side animation
            animationDirection = new Vector2(1f, 0f);

            //Flips animation depending on left or right view
            if (signedAngle < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            //Play front animation
            animationDirection = new Vector2(0f, 1f);
        }

        animator.SetFloat("MoveX", animationDirection.x);
        animator.SetFloat("MoveY", animationDirection.y);
    }
}
