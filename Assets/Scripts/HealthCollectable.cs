using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private GameObject CollectableFront;
    [SerializeField] private GameObject CollectableBack;
    [SerializeField] private GameObject Collider;

    [field: SerializeField] public GameEventScriptableObject HealthObjectCollectedEvent { get; private set; }

    private void Start()
    {
        //parentGameObject.transform.DOMoveY(parentGameObject.transform.position.y + floatDistance, 1.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        /*float startingYRotation = Random.Range(0, 180);

        parentGameObject.transform.Rotate(new Vector3(0, startingYRotation, 0));

        parentGameObject.transform.DORotate(new Vector3(0, startingYRotation + 180, 0), 1.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);*/
    }

    private void OnEnable()
    {
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
        GameDataReader.Instance.GameData.PlayerHealth = SettingsReader.Instance.GameSettings.PlayerCharacterProperties.MaxHealth;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.healthCollected, transform.position);

        HealthObjectCollectedEvent?.Raise();
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.objectCollected, this.transform.position);
        //Destroy(parentGameObject);
        CollectableFront.SetActive(false);
        CollectableBack.SetActive(false);
        Collider.SetActive(false);
    }
}
