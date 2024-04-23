using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///游戏屏幕适配
public class BgSet : MonoBehaviour
{
    [Header("游戏有效宽度：Screen.width/ Screen.height*2*orthographicSize")]
    [Tooltip("游戏有效宽度,自行计算")]
    public float vaildWidth;
    public SpriteRenderer bg;
    public Sprite[] bgs;

    private void Awake()
    {
        Adaptiontion();
    }

    private void Start()
    {
        GameManager.GameOver += RandomBg;
        GameManager.BossDead += RandomBg;
    }
    
    private void OnDestroy()
    {
        GameManager.GameOver -= RandomBg;
        GameManager.BossDead -= RandomBg;
    }

    private void RandomBg()
    {
        int randomIcon = Random.Range(0, bgs.Length);
        bg.sprite = bgs[randomIcon];
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
