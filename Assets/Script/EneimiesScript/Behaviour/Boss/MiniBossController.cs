using System.Collections;
using UnityEngine;

public class MinibossController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MinibossPhase1Behaviour phase1Behaviour;
    [SerializeField] private MinibossPhase2Behaviour phase2Behaviour;

    [Header("Intro Settings")]
    [SerializeField] private float introSpeed = 1.5f;

    [Header("Anchor")]
    [SerializeField] private Transform pointStart;
    [SerializeField] private Transform pointFloating;

    [Header("Phase 2 Ray")]
    [SerializeField] private Pools rayPool;
    [SerializeField] private Transform[] raySpawnPoints;
    [SerializeField] private float raySpawnInterval = 2f;

    [Header("Phase 2 Laser")]
    [SerializeField] private Pools laserPool;
    [SerializeField] private Transform[] laserSpawnPoints;
    [SerializeField] private float laserSpawnInterval = 3f;

    private Animator animator;
    private BossHealth bossHealth;
    private MiniBossSound miniBossSound;

    private Coroutine rayCoroutine;
    private Coroutine laserCoroutine;
    private Coroutine transitionCoroutine;
    private bool phase2Triggered = false;
    private bool isDying = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();
        miniBossSound = GetComponent<MiniBossSound>();
    }

    private void Start()
    {
        if (bossHealth != null)
            bossHealth.OnHalfHealth += OnBossHalfHealth;

        phase1Behaviour.StopBehaviour();
        phase2Behaviour.StopBehaviour();
        SetPhase2Active(false);

        if (pointStart != null)
            transform.position = pointStart.position;

        StartCoroutine(IntroRoutine());
    }

    private IEnumerator IntroRoutine()
    {

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveToPosition(pointFloating, introSpeed));

        phase1Behaviour.SetAnchor(pointFloating);
        phase1Behaviour.InitBehaviour(null);
    }

    private void OnBossHalfHealth()
    {
        if (phase2Triggered || isDying) return;
        phase2Triggered = true;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        transitionCoroutine = StartCoroutine(TransitionToPhase2());
    }

    private IEnumerator TransitionToPhase2()
    {
        phase1Behaviour.StopBehaviour();
        SetPhase2Active(false);

        if (pointStart != null)
        {
            yield return StartCoroutine(MoveToPosition(pointStart, introSpeed));
            if (isDying) yield break;
        }

        animator?.SetTrigger("Phase2");
        CameraShake.Instance?.Shake();

        yield return new WaitForSeconds(0.5f);
        if (isDying) yield break;

        if (pointFloating != null)
        {
            yield return StartCoroutine(MoveToPosition(pointFloating, introSpeed));
            if (isDying) yield break;
        }

        phase2Behaviour.SetAnchor(pointFloating);
        phase2Behaviour.InitBehaviour(null);

        phase1Behaviour.StopBehaviour();
        phase1Behaviour.enabled = false;

        SetPhase2Active(true);

        transitionCoroutine = null;
    }

    public void OnBossDead()
    {
        if (isDying) return;
        isDying = true;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        phase1Behaviour.StopBehaviour();
        phase2Behaviour.StopBehaviour();
        SetPhase2Active(false);

    }

    private void SetPhase2Active(bool active)
    {
        if (active)
        {
            if (rayCoroutine == null)
                rayCoroutine = StartCoroutine(SpawnLoop(rayPool, raySpawnPoints, raySpawnInterval, true));
            if (laserCoroutine == null)
                laserCoroutine = StartCoroutine(SpawnLoop(laserPool, laserSpawnPoints, laserSpawnInterval, false));
        }
        else
        {
            if (rayCoroutine != null) { StopCoroutine(rayCoroutine); rayCoroutine = null; }
            if (laserCoroutine != null) { StopCoroutine(laserCoroutine); laserCoroutine = null; }
        }
    }

    private IEnumerator SpawnLoop(Pools pool, Transform[] points, float interval, bool isRay)
    {
        if (pool == null || points == null || points.Length == 0) yield break;
        while (!isDying)
        {
            GameObject obj = pool.SetObject();
            if (obj != null)
            {
                obj.transform.position = points[Random.Range(0, points.Length)].position;
                obj.SetActive(true);

                if (miniBossSound != null)
                {
                    if (isRay) miniBossSound.PlayRay();
                    else miniBossSound.PlayLaser();
                }
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator MoveToPosition(Transform target, float speed)
    {
        if (target == null) yield break;
        while (!isDying && Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.position = newPos;
            yield return null;
        }
        if (!isDying)
        {
                transform.position = target.position;
        }
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
            bossHealth.OnHalfHealth -= OnBossHalfHealth;
    }
}