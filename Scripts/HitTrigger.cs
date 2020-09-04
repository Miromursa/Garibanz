using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    public CombatStats cs;
    public Collider collider;
    public Animator anim;

    void Start()
    {
        collider.enabled = false;
        cs = GetComponentInParent<CombatStats>();
        anim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (!collider.enabled && anim.GetBool("isAttackButton"))
        {
            collider.enabled = true;
        }

        if (collider.enabled &&
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            collider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<CombatStats>().takeDamage(cs.getDamage());
            Debug.Log("Hit!");
        }
    }
}
