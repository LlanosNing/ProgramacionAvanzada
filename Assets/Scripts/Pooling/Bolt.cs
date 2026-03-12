using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bolt : MonoBehaviour
{
    //el pool al que pertenece este objeto
   public ObjectPool<Bolt> pool;

    private void OnCollisionEnter(Collision collision)
    {
        //cuando choca contra algo, se devuelve a sí mismo al pool
        pool.Release(this);
    }
}
