using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DNController : MonoBehaviour
{
    public static DNController instance;
    public DamageNumber numberToSpawn;
    public Transform numberCanvans;
    private List<DamageNumber> numbersPool=new List<DamageNumber>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpwanDamageNumber(Vector3 position, float damage)
    {
        int rounded=Mathf.RoundToInt(damage);
        //DamageNumber dn = Instantiate(numberToSpawn, position, Quaternion.identity,numberCanvans);
        DamageNumber dn=GetFromPool();
        dn.SetUp(rounded);

        dn.gameObject.SetActive(true);
        dn.transform.position=position;
    }

    public DamageNumber GetFromPool()
    {
        DamageNumber dn = null;
        if (numbersPool.Count == 0)
        {
            dn = Instantiate(numberToSpawn, numberCanvans);
        }
        else
        {
            dn = numbersPool[0];
            numbersPool.RemoveAt(0);
        }
        return dn;
    }

    public void ReturnToPool(DamageNumber dn)
    {
        dn.gameObject.SetActive(false);
        numbersPool.Add(dn);
    }
}
