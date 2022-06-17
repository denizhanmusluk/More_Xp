using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{     
    [SerializeField(), Range(1.0f, 20.0f)] private float sensivity;

    public JoyStickDirection JoystickDirection = JoyStickDirection.Both;
    public RectTransform Background;
    public RectTransform Handle;
    [Range(0, 10f)] public float HandleLimit = 1f;
    private Vector2 direction = new Vector2(0, 1);
    private Vector2 speed = Vector2.zero;
    //Output
    public float Vertical { get { return direction.y; } }
    public float Horizontal { get { return direction.x; } }
    Vector2 JoyPosition = Vector2.zero;
    public void OnPointerDown(PointerEventData eventdata)
    {

        Background.gameObject.SetActive(true);
        OnDrag(eventdata);
        JoyPosition = eventdata.position;
        Background.position = eventdata.position;
        Handle.anchoredPosition = Vector2.zero;

    }
    public void OnDrag(PointerEventData eventdata)
    {
        Vector2 JoyDriection = eventdata.position - JoyPosition;
        direction = (JoyDriection.magnitude > Background.sizeDelta.x / 2f) ? JoyDriection.normalized :
        speed = (JoyDriection.magnitude > Background.sizeDelta.x / 2f) ? JoyDriection.normalized :

            JoyDriection / (Background.sizeDelta.x / 2f);
        if (JoystickDirection == JoyStickDirection.Horizontal)
        {
            direction = new Vector2(direction.x, 0f);
            speed = new Vector2(speed.x, 0f);


        }
        if (JoystickDirection == JoyStickDirection.Vertical)
        {
            direction = new Vector2(0f, direction.y);
            speed = new Vector2(0f, speed.y);

        }
        Handle.anchoredPosition = (sensivity * direction * Background.sizeDelta.x / 20f) * HandleLimit;
    }
    public void OnPointerUp(PointerEventData eventdata)
    {
        Background.gameObject.SetActive(false);
        speed = Vector2.zero;
        Handle.anchoredPosition = Vector2.zero;
    }
}

