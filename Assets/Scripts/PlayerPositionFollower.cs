using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionFollower : MonoBehaviour
{
    [SerializeField] private float customYPosition;

    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, customYPosition, player.position.z);
        //Debug.Log("collider pos: " + transform.position);
    }
}
