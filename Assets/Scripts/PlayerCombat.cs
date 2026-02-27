using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject hitbox;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) {
            StartCoroutine(AttackCrt());
        }
    }

    IEnumerator AttackCrt()
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(.15f);
        hitbox.SetActive(false);
    }
}
