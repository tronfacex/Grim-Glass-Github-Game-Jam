using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelTweener : MonoBehaviour
{
    [SerializeField] private LevelStateMachine stateMachine;

    [SerializeField] private TweenType tweenType;

    [SerializeField] private float moveSpeed = 1f;

    public int currentIndex = 1;

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

    public void TweenPlatform()
    {
        MoveToNextPosition();
    }

    private void MoveToNextPosition()
    {
        if (stateMachine.InScene)
        {
            return;
        }
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

    private void OnTweenComplete()
    {
        currentIndex++;
        /*if (currentIndex >= stateMachine.TweenTransforms.Count)
        {
            currentIndex = 0;  // Reset the index to loop back to the start.
        }*/

        //MoveToNextPosition();

        stateMachine.InScene = true;
    }

    public void OnIdle()
    {
        transform.DOPause();
    }
}
