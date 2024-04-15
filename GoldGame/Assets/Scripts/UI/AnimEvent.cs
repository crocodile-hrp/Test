using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum AnimType
{
    Explode,
    Size,
}
public class AnimEvent : MonoBehaviour
{
    Vector3 startPos;
    Vector3 startSize;
    Quaternion startRot;
    public float animTime;
    Color startColor;
    public int dir;
    public int valueOffsetX;
    public int valueOffsetY;
    public int valueSize;
    public int valueMinRot = 60;
    public int valueMaxRot = 180;
    public AnimType animType = AnimType.Explode;

    private void OnEnable()
    {
        startPos = transform.localPosition;
        startSize = transform.localScale;
        startRot = transform.localRotation;
        if (GetComponent<Image>())
        {
            startColor = GetComponent<Image>().color;
        }
        if (GetComponent<SpriteRenderer>())
        {
            startColor = GetComponent<SpriteRenderer>().color;
        }
        switch (animType)
        {
            case AnimType.Explode:
                ExplodeAnim();
                break;
            case AnimType.Size:
                SizeAnim();
                break;
        }
    }

    private void OnDisable()
    {
        ResetUI();
    }

    private void OnDestroy()
    {
        ResetUI();
    }

    void ExplodeAnim()
    {
        Vector3 posOffset = new Vector3(0.1f * dir* valueOffsetX, 0.1f*valueOffsetY);
        Vector3 sizeOffset = new Vector3(1* valueSize, 1* valueSize);
        float targetRotation = Random.Range(valueMinRot, valueMaxRot);
        UIAnim(posOffset, sizeOffset, targetRotation);
    }

    void SizeAnim()
    {
        Vector3 sizeOffset = new Vector3(1 * valueSize, 1 * valueSize);
        StartCoroutine(IEUIAnimSize(sizeOffset));
    }

    public void UIAnim(Vector3 posOffset,Vector3 sizeOffset, float targetRotation,float targetAlpha = 0)
    {
        StartCoroutine(IEUIAnim(posOffset, sizeOffset, targetRotation));
    }


    IEnumerator IEUIAnim(Vector3 posOffset, Vector3 sizeOffset, float targetRotation, float targetAlpha = 0)
    {
        yield return 0;
        float timer = 0;
        while(timer < animTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, startPos + posOffset, timer / animTime);
            transform.localScale = Vector3.Lerp(startSize,sizeOffset, timer / animTime);
            float t = timer / animTime;
            float currentAngle = Mathf.Lerp(0, targetRotation, t); // 在起始和目标旋转之间进行线性插值
            transform.rotation = startRot * Quaternion.Euler(new Vector3(0,0,-dir) * currentAngle);
            timer += Time.deltaTime;
            yield return 0;
        }
        while (timer > 0)
        {
            if (GetComponent<Image>())
            {
                GetComponent<Image>().color =new Color (startColor.r, startColor.g, startColor.b,timer/animTime);
            }
            if (GetComponent<SpriteRenderer>())
            {
                GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, timer / animTime);
            }
            timer -= Time.deltaTime;
            yield return 0;
        }
    }

    IEnumerator IEUIAnimSize( Vector3 sizeOffset)
    {
        yield return 0;
        float timer = 0;
        while (timer < animTime)
        {
            transform.localScale = Vector3.Lerp(startSize, sizeOffset, timer / animTime);
            timer += Time.deltaTime;
            yield return 0;
        }
        while (timer > 0)
        {
            transform.localScale = Vector3.Lerp(startSize, sizeOffset, timer / animTime);
            timer -= Time.deltaTime;
            yield return 0;
        }
    }

    private void ResetUI()
    {
        transform.localPosition = startPos;
        transform.localScale = startSize;
        transform.localRotation = startRot;
        if (GetComponent<Image>())
        {
            GetComponent<Image>().color = startColor;
        }
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().color= startColor;
        }
    }


}
