using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    public GameObject capsule;

    //public static GameController gameController;


    public void Start()
    {
        Instantiate(capsule,Vector3.zero,Quaternion.identity);
    }

}
