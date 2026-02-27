using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
   public void TakeDamage(int damage)
    {
        //al recibir daño lo resta a su vida y si llega a 0 se muere
        health -= damage;
        if(health <= 0)
            gameObject.SetActive(false);
    }
}
