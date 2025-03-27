using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{    
    public int Power { get; set; }
    public int Defense { get; set; }
    public int health { get; set; }
    public float speed { get; set; }
    public float jumpPower { get; set; }


    public BaseState(int Power,  int Defense, int health, float speed, float jumpPower)
    {
        this.Power = Power;
        this.Defense = Defense;
        this.health = health;
        this.speed = speed;
        this.jumpPower = jumpPower;
    }
}
