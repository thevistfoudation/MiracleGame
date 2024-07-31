using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // Kích thước khi phóng to
    public float scaleSpeed = 2f; // Tốc độ phóng to/thu nhỏ
    private Vector3 originalScale;
    private bool isScaling = false;

    void Start()
    {
        originalScale = transform.localScale; // Lưu kích thước ban đầu
    }

    void Update()
    {
        if (isScaling)
        {
            // Phóng to đối tượng dần dần
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
        else
        {
            // Thu nhỏ đối tượng về kích thước ban đầu dần dần
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isScaling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isScaling = false;
    }
}
