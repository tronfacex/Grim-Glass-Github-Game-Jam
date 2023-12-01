using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformTweener : MonoBehaviour
{
    [SerializeField] private MovingPlatformStateMachine stateMachine;

    [SerializeField] private TweenType tweenType;

    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private float originalMoveSpeed;

    public int currentIndex = 0;

    [SerializeField] private bool playOnce;

    [SerializeField] private bool hasBeenPlayed;

    [SerializeField] private GameEventScriptableObject PlayedOnceEvent;

    [SerializeField] private GameEventScriptableObject TimedReturnResetEvent;

    [SerializeField] private bool timedReturnPlatform;
    [SerializeField] private bool timedReturnCountdownStarted;
    [SerializeField] private bool timedReturnCancelled;
    [SerializeField] private bool quickReturn;

    public GameObject OnDropVFX;


    [SerializeField] private bool destroyOnDrop;
    public float TimeLeftBeforeReturn { get { return timeLeftBeforeReturn; } private set { } }
    private float timeLeftBeforeReturn;
    [SerializeField] private float waitDuration;

    public enum TweenType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc
    }

    private void Update()
    {
        if (timedReturnCountdownStarted && !timedReturnCancelled)
        {
            timeLeftBeforeReturn = Mathf.Max(timeLeftBeforeReturn - Time.deltaTime, 0f);
        }
    }

    public void TweenPlatform()
    {
        MoveToNextPosition();
    }

    private void MoveToNextPosition()
    {
        if (hasBeenPlayed) { return; }

        if (!timedReturnPlatform)
        {
            Transform targetTransform = stateMachine.TweenTransforms[currentIndex];

            float distance = Vector3.Distance(transform.position, targetTransform.position);
            float duration = distance / moveSpeed;

            switch (tweenType)
            {
                case TweenType.Linear:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.Linear)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseIn:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InSine)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseOut:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutSine)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInOut:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutSine)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInCubic:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InCubic)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseOutCubic:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutCubic)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInOutCubic:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutCubic)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInBack:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InBack)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseOutBack:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutBack)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInOutBack:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutBack)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInCirc:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InCirc)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseOutCirc:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutCirc)
                             .OnComplete(OnTweenComplete);
                    break;

                case TweenType.EaseInOutCirc:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutCirc)
                             .OnComplete(OnTweenComplete);
                    break;

                default:
                    Debug.LogWarning("Unknown TweenType: " + tweenType);
                    break;
            }
        }
        else
        {
            Transform targetTransform = stateMachine.TweenTransforms[1];

            float distance = Vector3.Distance(transform.position, targetTransform.position);
            float duration = distance / moveSpeed;

            switch (tweenType)
            {
                case TweenType.Linear:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.Linear)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseIn:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InSine)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseOut:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutSine)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInOut:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutSine)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInCubic:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InCubic)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseOutCubic:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutCubic)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInOutCubic:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutCubic)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInBack:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InBack)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseOutBack:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutBack)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInOutBack:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutBack)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInCirc:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InCirc)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseOutCirc:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.OutCirc)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                case TweenType.EaseInOutCirc:
                    transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.InOutCirc)
                             .OnComplete(TimedReturnPlatformWait);
                    break;

                default:
                    Debug.LogWarning("Unknown TweenType: " + tweenType);
                    break;
            }
        }
    }

    private void OnTweenComplete()
    {
        currentIndex++;

        if (currentIndex >= stateMachine.TweenTransforms.Count)
        {
            if (playOnce)
            {
                hasBeenPlayed = true;

                PlayedOnceEvent?.Raise();
                return;
            }
            else
            {
                currentIndex = 0;
            }
        }

        MoveToNextPosition();
    }

    public void OnIdle()
    {
        transform.DOPause();
    }

    private void OnTweenReturn()
    {
        if (destroyOnDrop)
        {
            stateMachine.PlatformPlayerMover.enabled = true;
            stateMachine.PlayerDetectorCollider.enabled = true;
            stateMachine.gameObject.GetComponent<MeshRenderer>().enabled = true;
            stateMachine.gameObject.GetComponent<MeshCollider>().enabled = true;
            /*if (OnDropVFX != null)
            {
                OnDropVFX.SetActive(false);
                StartCoroutine(TurnOffOnDropVFX(1.5f));
            }*/
            
        }

        if (quickReturn)
        {
            moveSpeed = originalMoveSpeed;
        }

        stateMachine.SwitchState(new MovingPlatformIdleState(stateMachine, false));
        TimedReturnResetEvent?.Raise();
    }

    IEnumerator TurnOffOnDropVFX(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnDropVFX.SetActive(false);
    }

    public void ReturnTweenToOrigin()
    {
        currentIndex = 0;

        if (timedReturnCancelled) { return; }
        //Debug.Log("Return Tween");

        if (quickReturn)
        {
            originalMoveSpeed = moveSpeed;
            moveSpeed = 40;
        }

        hasBeenPlayed = false;

        Transform targetTransform = stateMachine.TweenTransforms[0];

        float distance = Vector3.Distance(transform.position, targetTransform.position);
        float duration = distance / moveSpeed;

        switch (tweenType)
        {
            case TweenType.Linear:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.Linear)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseIn:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InSine)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseOut:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.OutSine)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInOut:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InOutSine)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInCubic:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InCubic)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseOutCubic:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.OutCubic)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInOutCubic:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InOutCubic)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInBack:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InBack)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseOutBack:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.OutBack)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInOutBack:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InOutBack)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInCirc:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InCirc)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseOutCirc:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.OutCirc)
                         .OnComplete(OnTweenReturn);
                break;

            case TweenType.EaseInOutCirc:
                transform.DOMove(targetTransform.position, duration)
                         .SetEase(Ease.InOutCirc)
                         .OnComplete(OnTweenReturn);
                break;

            default:
                Debug.LogWarning("Unknown TweenType: " + tweenType);
                break;
        }
    }

    private void TimedReturnPlatformWait()
    {
        if (destroyOnDrop)
        {
            stateMachine.PlatformPlayerMover.enabled = false;
            stateMachine.PlayerDetectorCollider.enabled = false;
            stateMachine.gameObject.GetComponent<MeshRenderer>().enabled = false;
            stateMachine.gameObject.GetComponent<MeshCollider>().enabled = false;
            if (OnDropVFX != null)
            {
                OnDropVFX.SetActive(true);
                StartCoroutine(TurnOffOnDropVFX(1.5f));
            }
        }
        
        StartCoroutine(TimedReturnPlatformWaitDelay());
    }
    private IEnumerator TimedReturnPlatformWaitDelay()
    {
        yield return new WaitForSeconds(waitDuration);
        ReturnTweenToOrigin();
    }
    public void ToggleCancelTimedReturnPlatform()
    {
        if (!timedReturnCancelled)
        {
            timedReturnCancelled = true;
            timedReturnCountdownStarted = false;
            stateMachine.SwitchState(new MovingPlatformIdleState(stateMachine, true));
            return;
        }
        else
        {
            timedReturnCancelled = false;
            stateMachine.SwitchState(new MovingPlatformMovingState(stateMachine));
        }
    }
}
