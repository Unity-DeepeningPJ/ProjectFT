using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    static readonly int Idle = Animator.StringToHash("Idle");
    static readonly int Move = Animator.StringToHash("Move");
    static readonly int Attack = Animator.StringToHash("Attack");
    static readonly int Jump = Animator.StringToHash("Jump");
    static readonly int Die = Animator.StringToHash("Die");

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnIdle(bool isIdle)
    {
        animator.SetBool(Idle, isIdle);
    }

    public void OnMove(bool isMove)
    {
        animator.SetBool(Move, isMove);
    }

    public void OnAttack(bool isAttack)
    {
        animator.SetBool(Attack, isAttack);

    }

    public void OnJump(bool isJump)
    {
        animator.SetBool(Jump, isJump);
    }

    public void OnDie()
    {
        animator.SetTrigger(Die);
    }

}
