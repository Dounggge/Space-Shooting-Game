using System.Collections;
using UnityEngine;

public class MinibossPhase2Behaviour : EnemyBehaviourAbstract
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float horizontalRange = 3f;
    [SerializeField] float zigzagAmplitude = 1.5f;
    [SerializeField] float zigzagFrequency = 2f;

    Transform anchorTransform;
    float zigzagTimer = 0f;
    float dirX = 1f;

    public void SetAnchor(Transform anchor) => anchorTransform = anchor;

    protected override void OnInit()
    {
        zigzagTimer = 0f;
        dirX = 1f;

        if (anchorTransform != null)
            transform.position = anchorTransform.position;
    }

    protected override void OnBehaviourUpdate()
    {
        if (anchorTransform == null) return;

        float ax = anchorTransform.position.x;
        float ay = anchorTransform.position.y;

        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float offsetY = Mathf.Sin(zigzagTimer) * zigzagAmplitude;
        float newX = transform.position.x + dirX * moveSpeed * Time.deltaTime;
        float minX = ax - horizontalRange;
        float maxX = ax + horizontalRange;

        if (newX > maxX || newX < minX)
            dirX *= -1f;

        transform.position = new Vector3(
            Mathf.Clamp(newX, minX, maxX),
            ay + offsetY,
            transform.position.z);
    }

    protected override void OnStop()
    {
        StopAllCoroutines();
    }
}