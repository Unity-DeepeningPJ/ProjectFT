using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Controller {  get; private set; }

    private void Awake()
    {
        Controller = GetComponent<PlayerController>();
    }
}
