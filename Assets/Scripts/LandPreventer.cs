using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPreventer : MonoBehaviour
{
    //This goes on enemy heads along with a box collider that keeps the player from landing on top of enemies

    private float slideSpeed = 14f;

    private void OnTriggerStay(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.SimpleMove((controller.transform.forward * -1.25f) * slideSpeed);
        }
    }
}
