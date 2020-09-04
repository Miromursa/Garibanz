using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator anim;
    Vector3 direction;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }
    

    void FixedUpdate()
    {
        if (direction.magnitude >= 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
                anim.SetBool("isWalking", false);
        }

        if (Input.GetKey("left shift"))
        {
            anim.SetBool("isShiftKey", true);
        }
        else
        {
            anim.SetBool("isShiftKey", false);
        }
    }

    public Animator getAnimator()
    {
        return anim;
    }
}



