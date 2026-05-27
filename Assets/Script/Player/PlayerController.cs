using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float leftPadding = 0.5f;
    [SerializeField] private float rightPadding = 0.5f;
    [SerializeField] private float upPadding = 0.5f;
    [SerializeField] private float downPadding = 0.5f;

    private InputAction moveAction;
    private InputAction shootAction;
    private PlayerShoot playerShoot;

    private Vector2 minBound;
    private Vector2 maxBound;

    private void Awake()
    {
        playerShoot = GetComponent<PlayerShoot>();
        /*
        if (playerShoot == null)
            Debug.LogError("PlayerController: none component PlayerShoot!");
        */
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        shootAction = InputSystem.actions.FindAction("Attack");

        //if (moveAction == null || shootAction == null)
        //{
        //    Debug.LogError("PlayerController: none Input Action!");
        //    return;
        //}

        shootAction.performed += _ => playerShoot.StartShooting();
        shootAction.canceled += _ => playerShoot.StopShooting();

        InitBounds();
    }

    private void OnDestroy()
    {
        if (shootAction != null)
        {
            shootAction.performed -= _ => playerShoot.StartShooting();
            shootAction.canceled -= _ => playerShoot.StopShooting();
        }
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 newPos = transform.position + (Vector3)(input * moveSpeed * Time.deltaTime);

        newPos.x = Mathf.Clamp(newPos.x, minBound.x + leftPadding, maxBound.x - rightPadding);
        newPos.y = Mathf.Clamp(newPos.y, minBound.y + downPadding, maxBound.y - upPadding);

        transform.position = newPos;
    }

    private void InitBounds()
    {
        Camera cam = Camera.main;
        minBound = cam.ViewportToWorldPoint(Vector2.zero);
        maxBound = cam.ViewportToWorldPoint(Vector2.one);
    }

    public bool IsMoving()
    {
        return moveAction != null && moveAction.ReadValue<Vector2>().sqrMagnitude > 0f;
    }
}