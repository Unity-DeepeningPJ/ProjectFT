using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcm;

    private int currentPriority = 5;
    private int activiePriority = 20;

    private void Awake()
    {
        vcm = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            vcm.Priority = activiePriority;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            vcm.Priority = currentPriority;
        }
    }
}
