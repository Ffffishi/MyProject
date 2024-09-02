using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPPickup : MonoBehaviour
{
    public int expValue;

    private bool isMoveToPlayer;
    private float moveSpeed=2f;

    public float timeBetweenChecks=.2f;
    private float checkCounter;

    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player=PlayerHealthController.instance.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveToPlayer==true)
        {

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;
                if (Vector3.Distance(transform.position, player.transform.position) < player.pickupRange)
                {
                    isMoveToPlayer = true;
                    moveSpeed += player.moveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ExpController.instance.AddExp(expValue);
            Destroy(gameObject);
        }
    }
}
