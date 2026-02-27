using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    [SerializeField] private float force = 10;
    public void TakeDamage(int damage)
    {
        //cuando este objeto "reciba daño", se aplica una fuerza hacia atras
        GetComponent<Rigidbody>().AddForce(-transform.forward * force, ForceMode.VelocityChange);
    }
}
