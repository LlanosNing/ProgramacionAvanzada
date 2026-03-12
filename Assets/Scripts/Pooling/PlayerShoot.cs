using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//para poder usar el sistema de Pooling de unity
using UnityEngine.Pool;


public class PlayerShoot : MonoBehaviour
{
    //el prefab que utilizara para crear los objetos del pool
    [SerializeField] private Bolt boltPrefab;
    [SerializeField] private Transform shootOrigin;
    //el pool de proyectiles (el pool es como un cajon)
    private ObjectPool<Bolt> boltPool;

    private void Start()
    {
        //opcion 1: cantidad de objetos minima y maxima por defecto(10, 10000)
        boltPool = new ObjectPool<Bolt>(CreateBolt, GetBolt, ReleaseBolt);
        //opcion 2: especificar cantidad minima y maxima
        //boltPool = new ObjectPool<Bolt>(CreateBolt, GetBolt, ReleaseBolt, null, true);
    }

    //esta funcion se llama al crear el pool por tantas veces como objetos pueda tener
    //por ejemplo, si se especifica un tamaño de 20 para el pool, llama a la funcion 20 veces
    private Bolt CreateBolt()
    {
        //crear un nuevo proyectil
        Bolt bolt = Instantiate(boltPrefab);
        //asignar el pool del proyectil
        bolt.pool = boltPool;
        //desactivar el proyectil para que este oculto
        bolt.gameObject.SetActive(false);
        return bolt;
    }

    //se llama cada vez que se coja un proyectil del pool
    private void GetBolt(Bolt bolt)
    {
        //al sacar un objeto del pool, lo principal es activarlo
        bolt.gameObject.SetActive(true);
        //movel el proyectil al punto de origen de disparo
        bolt.transform.position = shootOrigin.position;
        //añadir fuerza al proyectil
    }

    //se llama cada vez que un proyectil vuelva al pool
    private void ReleaseBolt(Bolt bolt)
    {
        //desactivar el objeto al devolverlo al pool    
        bolt.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1)){

            //coger un proyectil de los que haya en el pool
            boltPool.Get();
        }
    }
}
