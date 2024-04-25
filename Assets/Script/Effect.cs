using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float afterFalse = 0.3f;

    private void OnEnable()
    {
        Invoke("SetActiveFalse", afterFalse);
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
