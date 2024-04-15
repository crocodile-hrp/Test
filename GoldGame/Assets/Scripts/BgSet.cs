using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///游戏屏幕适配
public class BgSet : MonoBehaviour
{
    [Header("游戏有效宽度：Screen.width/ Screen.height*2*orthographicSize")]
    [Tooltip("游戏有效宽度,自行计算")]
    public float vaildWidth;

    private void Awake()
    {
        Adaptiontion();
    }

    /// <summary>
    /// 屏幕适配
    /// </summary>
    private void Adaptiontion()
    {
        float aspectRatio = Screen.width * 1f / Screen.height;
        //float currentWidth = Screen.width / Screen.height * 2 * Camera.main.orthographicSize;
        float orthograhicSize = vaildWidth / aspectRatio / 2;
        if(orthograhicSize < vaildWidth)
            Camera.main.orthographicSize = orthograhicSize;
    }
}
