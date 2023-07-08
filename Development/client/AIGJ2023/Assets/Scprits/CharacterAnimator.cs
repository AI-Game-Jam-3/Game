using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public RuntimeAnimatorController controller;
    Animator animator;
    Character character;

    void Start()
    {
        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;
        character = GetComponent<Character>();
        animator.runtimeAnimatorController = controller;
    }

    void Update()
    {
        if (character.MoveDirection == Vector2.up) animator.SetTrigger("back");
        if (character.MoveDirection == Vector2.down) animator.SetTrigger("front");
        if (character.MoveDirection == Vector2.left) animator.SetTrigger("left");
        if (character.MoveDirection == Vector2.right) animator.SetTrigger("right");
    }
}
