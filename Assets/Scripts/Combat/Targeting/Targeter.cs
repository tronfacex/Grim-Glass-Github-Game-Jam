using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> targets = new List<Target>();
    public bool TargetClosest;

    private Camera cameraMain;
    private float closestToCenterDistance;

    [field: SerializeField] public Target CurrentTarget { get; private set; }

    private void Start()
    {
        cameraMain = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Add(target);
        //target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Remove(target);
        //target.OnDestroyed -= RemoveTarget;
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            foreach (var target in targets)
            {
                if (target == null)
                {
                    targets.Remove(target);
                    return;
                }
            }

            if (TargetClosest)
            {
                //Order targets by closest to player
                targets = targets.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).ToList();
            }
            else
            {
                //Order targets by which one is closest to the center of the screen.
                targets = targets.OrderBy(t =>
                {
                    Vector2 viewPos = Camera.main.WorldToViewportPoint(t.transform.position);
                    return (viewPos - new Vector2(0.5f, 0.5f)).sqrMagnitude;
                }).ToList();
            }

            if (CurrentTarget == targets[0]) { return; }
            SelectTarget();
        }
        else
        {
            Cancel();
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        CurrentTarget = targets[0];

        Debug.Log("Current Target: " + CurrentTarget.gameObject.name);

        return true;
    }

    public void Cancel()
    {
        CurrentTarget = null;
    }

    private void RemoveTarget()
    {
        foreach (var target in targets)
        {
            if (target == null)
            {
                targets.Remove(target);
            }
        }
    }
}
