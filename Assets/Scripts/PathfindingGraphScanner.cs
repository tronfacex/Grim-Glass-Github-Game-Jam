using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGraphScanner : MonoBehaviour
{
    [SerializeField] private GameObject scaleArm;
    [SerializeField] private GameObject scaleBoundary;
    [SerializeField] private GameObject scalePlate1;
    [SerializeField] private GameObject scalePlate2;


    public void ReScanPathfindingGraphs(float delay)
    {
        StartCoroutine(ReScanPathfindingGraphsDelay(delay));
    }

    IEnumerator ReScanPathfindingGraphsDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AstarPath.active.Scan();
        yield return new WaitForSeconds(delay);
        scaleArm.layer = 8;
        scalePlate1.layer = 8;
        scalePlate2.layer = 8;
        scaleBoundary.SetActive(false);
    }

}
