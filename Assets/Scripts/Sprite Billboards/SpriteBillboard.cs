using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;
    private Transform mainCameraTransform;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.rotation = freezeXZAxis ?
                     Quaternion.Euler(0f, mainCameraTransform.rotation.eulerAngles.y, 0f)
                     : mainCameraTransform.rotation;
    }
}