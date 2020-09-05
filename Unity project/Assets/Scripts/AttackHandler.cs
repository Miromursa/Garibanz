using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public MovementHandler movementHandler;
    public Rigidbody rb;
    public AnimationHandler animationHandler;

    private bool isAttackEnabled = true;
    private bool isAttacking = false;

    void Start()
    {
        movementHandler = GetComponent<MovementHandler>();
        rb = GetComponent<Rigidbody>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttackEnabled = false;
            movementHandler.disableMovement();
            animationHandler.anim.SetBool("isAttackButton", true);
        }

        //WE SHOULD MOVE THIS TO ANIMATION TAB
            if (animationHandler.anim.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack") &&
    animationHandler.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isAttackEnabled = true;
            movementHandler.enableMovement();
            animationHandler.anim.SetBool("isAttackButton", false); 
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isAttackEnabled)
            Attack();
    }

    void Attack()
    {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, transform.eulerAngles.y - 180, 0f) * Vector3.forward;
            rb.velocity = moveDir.normalized * 2f;

        if (animationHandler.anim.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack") &&
           animationHandler.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            moveDir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
            rb.velocity = moveDir.normalized * 20f;
        }

        if (animationHandler.anim.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack") &&
           animationHandler.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
}
