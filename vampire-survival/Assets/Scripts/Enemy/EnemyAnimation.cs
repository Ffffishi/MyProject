using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {

        public Transform sprite;
        public float speed;
        public float minSize, maxSize;
        private float activeSize;
        void Start()
        {
            activeSize = maxSize;
            speed=speed*Random.Range(0.75f,1.25f);
        }

        // Update is called once per frame
        void Update()
        {
            sprite.localScale =Vector3.MoveTowards(sprite.localScale, Vector3.one*activeSize, speed*Time.deltaTime);
            if (sprite.localScale.x ==activeSize)
            {
                if (activeSize == minSize)
                {
                    activeSize = maxSize;
                }
                else
                {
                    activeSize = minSize;
                }
            }
        }
    }
}