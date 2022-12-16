using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    //跟随的平滑速度
    public float smoothSpeed = 0.125f;
    //摄像机的偏移量，可以外部调节
    public Vector3 offset;
    public bool isSet = default;
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
        isSet = transform.position == smoothedPosition ? true : false;
    }
}
