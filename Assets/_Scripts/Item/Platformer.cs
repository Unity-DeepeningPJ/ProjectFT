using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platformer : MonoBehaviour
{
    [SerializeField] Vector3 movePos;
    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask target;

    private Vector3 prevPos;
    private Vector3 desiredPos;
    private float marginDistance = 0.05f;

    private void Awake()
    {
        prevPos = transform.position;
        desiredPos = prevPos + movePos;

        Debug.Log(prevPos);
        
    }

    private void FixedUpdate()
    {
        MovePlatformer();
    }

    void MovePlatformer()
    {
        transform.position += Time.fixedDeltaTime * moveSpeed * movePos.normalized;

        if (Mathf.Abs(Vector3.Distance(transform.position, desiredPos)) < marginDistance
            || Mathf.Abs(Vector3.Distance(transform.position, prevPos)) < marginDistance)

        {
            moveSpeed *= -1;
        }
    }
}
