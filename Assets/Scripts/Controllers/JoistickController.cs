using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class JoistickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    protected bool isActive;
    [SerializeField]
    protected Camera cam;
    [SerializeField]
    protected RectTransform back, stick;
    [SerializeField]
    protected Vector2 fillProcent;
    protected Vector2 vector;
    protected float angle;
    protected Quaternion quaternion;

    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            if (!value)
            {
                vector = Vector2.zero;
                angle = 0.0f;
                quaternion = Quaternion.identity;
                stick.anchoredPosition = back.anchoredPosition;
            }
        }
    }
    public Vector2 Vector => vector;
    public float Angle => angle;
    public Quaternion Quaternion => quaternion;
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(back, eventData.position, cam, out vector))
        {
            vector.x = 2.0f * (vector.x / back.sizeDelta.x);
            vector.y = 2.0f * (vector.y / back.sizeDelta.y);
            vector = (vector.sqrMagnitude > 1.0f) ? vector.normalized : vector;
            stick.anchoredPosition = new Vector2(vector.x * back.sizeDelta.x / 2.0f * fillProcent.x, vector.y * back.sizeDelta.y / 2.0f * fillProcent.y) + back.anchoredPosition;
            angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            quaternion = Quaternion.Euler(0.0f, 0.0f, angle);
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        IsActive = true;
        OnDrag(eventData);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        IsActive = false;
    }
}