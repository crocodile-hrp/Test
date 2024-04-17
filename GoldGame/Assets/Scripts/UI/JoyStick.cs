using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class JoyStick : Singleton<JoyStick>,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerExitHandler
{
    public RectTransform joyStickBg;
    public RectTransform joyStickCenter;
    /// <summary>
    /// ҡ�˰뾶
    /// </summary>
    public float mRadius;
    public Vector2 joyPos;
    public Vector2 startPos;
    public Vector2 movePos;
    //public static event Action OnJump;

    Image Bg;

    protected override void Awake()
    {
        base.Awake();
        Bg = GetComponent<Image>();
        ResetJoy();
    }

    private void OnDisable()
    {
        //ResetJoy();
    }

    private void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joyStickBg.gameObject.SetActive(true);
        joyStickBg.position = eventData.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        joyStickBg.gameObject.SetActive(true);
        /*  Vector3 eventPos = Camera.main.ScreenToWorldPoint(eventData.position)*/
        ;
        joyStickBg.position = eventData.position;
        startPos = joyStickCenter.position;
        mRadius = joyStickBg.rect.width/2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 eventPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 moveRange = eventData.position - startPos;
        float moveDis = Mathf.Clamp(moveRange.magnitude, -mRadius, mRadius);
        movePos = moveRange.normalized;
        //Debug.Log("moveRange.normalized" + movePos);
        joyStickCenter.position = startPos + moveDis * movePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetJoy();
        //joyStickBg.gameObject.SetActive(false);
    }

    public void JumpEvent()
    {
        //OnJump?.Invoke();
    }

    void ResetJoy()
    {
        joyStickCenter.localPosition = new Vector3(0,0);
        movePos = new Vector2(0, 0);
        joyStickBg.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ResetJoy();
    }
}
