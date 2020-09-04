using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStats : MonoBehaviour
{
    private int maxHealth;
    private int health;
    private int stamina;
    private int damage;

    private bool isDead = false;

    //define all starting stats in the game for every being
    void Start()
    {
        if (tag == "Player")
        {
            maxHealth = 100;
            health = maxHealth;
            damage = 100;
        }

        if (tag == "Enemy")
        {
            maxHealth = 200;
            health = maxHealth;
        }
    }

    public void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            Debug.Log("Im ded");
            Destroy(gameObject);
        }
    }
    
    public int getDamage()
    {
        return this.damage;
    }

    public int getHealth()
    {
        return this.health;
    }
}
