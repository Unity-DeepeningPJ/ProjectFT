﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput { get; private set; }
    public PlayerInput.PlayerActions playerActions { get; private set; }

    void Start()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
