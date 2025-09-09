using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject cube;

    void Start()
    {
        
    }

    private void Update()
    {

        pingpong();

        

    }

    public void pingpong()
        {
        cube.transform.position += new Vector3(0.01f, 0);

        if (cube.transform.position.x > 5)
        {
            cube.transform.position = new Vector3(0, 0);
        }

    }



}
