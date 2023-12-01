using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRotationHandler : MonoBehaviour
{
    //This script exists to rotate the milk bottles during their vomit attack

    [SerializeField] private Transform enemyTransform;
    [SerializeField] private GameObject vomitBillboard;

    public void VomitRotation()
    {
        vomitBillboard.SetActive(true);
        enemyTransform.DORotate(new Vector3(0f, 360f, 0f), 1.7f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(TurnOffVomitBillboard);
    }

    public void TurnOffVomitBillboard()
    {
        vomitBillboard.SetActive(false);
    }
}
