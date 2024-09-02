using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    void Awake()
    {
        instance = this;
    }
    public float moveSpeed;
    public Animator animator;
    public float pickupRange=1.5f;
    //public Weapon activeWeapon;
    public List<Weapon> unassignedWeapons, assignedWeapons;
    public int maxWeapon = 3;
    [HideInInspector]
    public List<Weapon> fullyLevelledWeapons=new List<Weapon>();

    void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }

        moveSpeed = PlayerStatesController.instance.moveSpeed[0].value;
        pickupRange = PlayerStatesController.instance.pickUpRange[0].value;
        maxWeapon = Mathf.RoundToInt(PlayerStatesController.instance.maxWeapons[0].value);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        moveInput.Normalize();

        transform.position += moveInput * moveSpeed * Time.deltaTime;
        if (moveInput!= Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void AddWeapon(int weaponNumber)
    {
        /****/
        if (weaponNumber< unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        weapon.gameObject.SetActive(true);
        assignedWeapons.Add(weapon);
        unassignedWeapons.Remove(weapon);
    }
}
