using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        //accede a la interfaz IDamageable y ejecuta la funcion TakeDamage
        other.GetComponent<IDamageable>().TakeDamage(damage);
    }
}
