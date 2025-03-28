using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player _player;
    public Player  player => _player;

    private void Awake()
    {
        _player= FindObjectOfType<Player>();
    }
}
