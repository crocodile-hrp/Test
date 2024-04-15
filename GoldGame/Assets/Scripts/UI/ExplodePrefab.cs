using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodePrefab : MonoBehaviour
{
    public float showTime;


    private void OnEnable()
    {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.SetActive(false);
    }


}
