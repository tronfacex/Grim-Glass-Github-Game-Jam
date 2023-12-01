using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    [SerializeField] private GameObject parentGameObject;

    //[SerializeField] private float floatDistance = .43f;
    [field: SerializeField] public GameEventScriptableObject ObjectCollectedEvent { get; private set; }

    private void Start()
    {
        //parentGameObject.transform.DOMoveY(parentGameObject.transform.position.y + floatDistance, 1.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        float startingYRotation = Random.Range(0, 180);

        parentGameObject.transform.Rotate(new Vector3(0, startingYRotation, 0));

        parentGameObject.transform.DORotate(new Vector3(0, startingYRotation + 180, 0), 1.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        parentGameObject.transform.DOPause();
        OnCollected();
    }

    private void OnCollected()
    {
        GameDataReader.Instance.GameData.CollectablesCount++;
        ObjectCollectedEvent?.Raise();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.objectCollected, this.transform.position);
        Destroy(parentGameObject);
    }
}
