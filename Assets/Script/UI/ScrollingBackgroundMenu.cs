using UnityEngine;

public class ScrollingBackgroundMenu : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;

    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector2 offset = material.mainTextureOffset;
        offset.y += Time.deltaTime * scrollSpeed;
        material.mainTextureOffset = offset;
    }
}
