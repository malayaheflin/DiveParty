using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPostion : MonoBehaviour
{
    public Vector3 originLocalPosition;
    private void Start()
    {
        originLocalPosition = transform.localPosition;
    }
    private void Update()
    {
        transform.localPosition = originLocalPosition;
    }
}
