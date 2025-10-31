using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsJump = Animator.StringToHash("IsJump");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > .5f);
    }

    public void Jump(Vector2 obj) 
    {
        animator.SetBool(IsJump, true);
    }

    public void JumpEnd()
    {
        animator.SetBool(IsJump, false);
    }

    public void Dead(Vector2 obj) 
    {
        animator.SetBool(IsDead, true);
    }
}
