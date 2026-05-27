using System.Collections;
using UnityEngine;

public class MinibossPhase1Behaviour : EnemyBehaviourAbstract
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float horizontalRange = 3f;
    [SerializeField] float stopDurationMin = 1f;
    [SerializeField] float stopDurationMax = 2.5f;

    Transform anchorTransform;
    Vector3 currentTarget;
    bool isWaiting = false;
    bool hasArrived = false;

    public void SetAnchor(Transform anchor) => anchorTransform = anchor;

    protected override void OnInit()
    {
        isWaiting = false;
        hasArrived = false;

        if (anchorTransform != null)
            transform.position = anchorTransform.position;

        PickNewTarget();
    }

    protected override void OnBehaviourUpdate()
    {
        if (isWaiting) return;

        transform.position = Vector3.MoveTowards(
            transform.position, currentTarget, moveSpeed * Time.deltaTime);

        if (!hasArrived && Vector3.Distance(transform.position, currentTarget) < 0.05f)
        {
            hasArrived = true;
            StartCoroutine(WaitThenMove());
        }
    }

    protected override void OnStop()
    {
        StopAllCoroutines();
        isWaiting = false;
        hasArrived = false;
    }

    IEnumerator WaitThenMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(Random.Range(stopDurationMin, stopDurationMax));
        PickNewTarget();
        isWaiting = false;
        hasArrived = false;
    }

    void PickNewTarget()
    {
        if (anchorTransform == null) return;
        float offsetX = Random.Range(-horizontalRange, horizontalRange);
        currentTarget = new Vector3(
            anchorTransform.position.x + offsetX,
            anchorTransform.position.y,
            anchorTransform.position.z);
    }
}