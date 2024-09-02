using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpP : MonoBehaviour
{
    public int expValue;

    private bool isMoveToPlayer;
    public float moveSpeed;

    public float timeBetweenChecks = .2f;
    private float checkCounter;

    private PlayeManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthMgr.Instance.gameObject.GetComponent<PlayeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveToPlayer == true)
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
                    moveSpeed += player.speed;
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
