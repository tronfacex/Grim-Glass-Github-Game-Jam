using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropShadowController : MonoBehaviour
{

    [Header("Settings")]
    public Transform _parent;
    public Vector3 _parentOffset = new Vector3(0f, 0.01f, 0f);
    public LayerMask GroundMasks;


    void Update()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 300f, GroundMasks))
        {
            // Position
            _parent.position = hitInfo.point + _parentOffset;

            // Rotate to same angle as ground
            _parent.up = hitInfo.normal;

            // Draw a green ray to represent a successful hit
            Debug.DrawRay(transform.position, Vector3.down * hitInfo.distance, Color.green);
        }
        else
        {
            // If raycast not hitting (air beneath feet), position it far away
            _parent.position = new Vector3(0f, 1000f, 0f);

            // Draw a red ray to represent no hit
            Debug.DrawRay(transform.position, Vector3.down * 300f, Color.red);
        }
    }
}
