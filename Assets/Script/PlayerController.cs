using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction moveAction;
    Vector3 moveInput;
    Vector2 minBounce;
    Vector2 maxBounce;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float leftPadding = -10f;
    [SerializeField] float rightPadding = 10f;
    [SerializeField] float upPadding = -10f;
    [SerializeField] float bellowPadding = 10f;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        InitBounced();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        Vector3 newPosition = transform.position + moveInput * moveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minBounce.x + leftPadding, maxBounce.x - rightPadding);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounce.y + bellowPadding, maxBounce.y - upPadding);

        transform.position = newPosition;
    }

    void InitBounced()
    {
        Camera mainCamera = Camera.main;
        minBounce = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounce = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }
}
