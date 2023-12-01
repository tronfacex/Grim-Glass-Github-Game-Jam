using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateMachine : StateMachine
{
    [field: SerializeField] public LevelTweener LevelTweener { get; private set; }
    [field: SerializeField] public List<Transform> TweenTransforms { get; private set; }

    [field: SerializeField] public int CurrentState;

    private int StagingStateHash = Animator.StringToHash("Staging State");
    private int InSceneStateHash = Animator.StringToHash("In-Scene State");

    [field: SerializeField] public bool InScene;


    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        //MainCameraTransform = Camera.main.transform;

       SwitchState(new LevelStagingState(this));
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    public void LevelStagingCompleted()
    {
        InScene = true;
    }

    public void MoveLevelIntoPlace()
    {
        SwitchState(new LevelMovingState(this));
    }
}
