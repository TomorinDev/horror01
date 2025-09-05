using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowmanAnimator : MonoBehaviour
{
    [Header("Enemy Animator")]
    public Animator animator; // Enemy Animator




    // Get Component
    public void InitialGetComponents()
    {
        animator = GetComponent<Animator>();
    }




    // Set Animator
    // Attack
    public void StartAttack()
    {
        // Animator
        animator.SetBool("isAttack", true);
    }

    public void EndAttack()
    {
        animator.SetBool("isAttack", false);
    }



}
