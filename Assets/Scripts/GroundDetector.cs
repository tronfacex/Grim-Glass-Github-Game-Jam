using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Transform groundCheckPoint;

    private float groundDistance;

    [SerializeField] private PlayerStateMachine stateMachine;

    public List<LayerMask> groundMasks;

    // Start is called before the first frame update
    void Start()
    {
        //stateMachine = gameObject.GetComponent<PlayerStateMachine>();
        groundDistance = stateMachine.GroundRaycastDistance;
    }

    // Update is called once per frame
    /*private void FixedUpdate()
    {
        GroundCheck();
    }*/

    private void Update()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        bool isGrounded = false;
        foreach (var groundMask in groundMasks)
        {
            RaycastHit hit;
            if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out hit, groundDistance, groundMask))
            {
                // Ground was detected for this mask!
                isGrounded = true;
                break;
            }
        }
        if (stateMachine.OnMovingPlatform)
        {
            stateMachine.IsGrounded = true;
        }
        else
        {
            stateMachine.IsGrounded = isGrounded;
        }
        Debug.DrawRay(groundCheckPoint.position, Vector3.down * groundDistance, Color.red);  // Visualizations
    }
}
