using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] float boostedScrollSpeed = 0.2f;
    [SerializeField] float acceleration = 0.5f;
    float currentSpeed;
    Material material;
    PlayerMovement playerInput;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        playerInput = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        Vector2 offset = material.mainTextureOffset;
        offset.y += Time.deltaTime * SetScrollSpeed();
        material.mainTextureOffset = offset;
    }

    float SetScrollSpeed()
    {
        float targetSpeed = (playerInput.IsMoving()) ? boostedScrollSpeed : scrollSpeed;
        return currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

}
