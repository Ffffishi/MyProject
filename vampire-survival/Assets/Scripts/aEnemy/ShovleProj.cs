using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShovleProj : MonoBehaviour
{
    public float moveSpeed;

    public float lifeTime;

    public int count;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.up*moveSpeed*Time.deltaTime;
        lifeTime-=Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }


}
