using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeManager : MonoBehaviour
{
    //public WeaponsF activeWeapon;

    public static PlayeManager instance;
    void Awake()
    {
        instance = this;
    }

    public float perDamage;

    public float pickupRange = 1.5f;

    public float speed;

    private Rigidbody2D rb;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    public bool isBagOpen;

    public GameObject bag;

    public List<WeaponsF> unassignedWeapons, assignedWeapons;
    public int maxWeapon = 3;
    [HideInInspector]
    public List<WeaponsF> fullyLevelledWeapons = new List<WeaponsF>();
    void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }
        rb =GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        OpenBag();
    }

    public void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBagOpen = !isBagOpen;
            bag.SetActive(isBagOpen);
            if (bag.activeSelf == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    private void Run()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }else if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (moveX == 0 && moveY == 0)
        {
            animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isMove", true);
        }
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        
        rb.velocity = movement * speed;
    }

    public void AddWeapon(int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public void AddWeapon(WeaponsF weapon)
    {
        weapon.gameObject.SetActive(true);
        assignedWeapons.Add(weapon);
        unassignedWeapons.Remove(weapon);
    }
}
