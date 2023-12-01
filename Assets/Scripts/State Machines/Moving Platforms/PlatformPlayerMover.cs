using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformPlayerMover : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }

    private Vector3 lastPosition;

    private PlayerStateMachine stateMachine;

    [SerializeField] private Transform movingPlatformObj;

    [SerializeField] private Transform playerRig;

    [SerializeField] private GameEventScriptableObject playerOnPlatformEvent;
    [SerializeField] private GameEventScriptableObject playerOffPlatformEvent;

    private void Start()
    {
        stateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        playerRig = GameObject.Find("Player Rig").transform;
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 displacement = transform.position - lastPosition;
        if (stateMachine.ActivePlatform != movingPlatformObj)
        {
            return;
        }



        stateMachine.ActivePlatformVelocity = displacement / Time.deltaTime;


        //Debug.Log("Platform velocity = " + Velocity);

        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateMachine stateMachine = other.GetComponent<PlayerStateMachine>();
            stateMachine.ActivePlatform = movingPlatformObj;
            //stateMachine.IsGrounded = true;
            other.gameObject.transform.SetParent(movingPlatformObj);
            playerOnPlatformEvent?.Raise();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateMachine stateMachine = other.GetComponent<PlayerStateMachine>();
            stateMachine.ActivePlatform = null;
            other.gameObject.transform.SetParent(GameObject.Find("Player Rig").transform);
            other.transform.localScale = new Vector3(1, 1, 1);
            playerOffPlatformEvent?.Raise();
        }
    }

    private void OnDisable()
    {
        if (stateMachine == null) { return; }
        if (playerRig == null) { return; }
        stateMachine.ActivePlatform = null;
        stateMachine.transform.SetParent(playerRig);
        stateMachine.transform.localScale = new Vector3(1, 1, 1);
        playerOffPlatformEvent?.Raise();
    }
}
