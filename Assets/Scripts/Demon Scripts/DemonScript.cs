using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public GameObject demonDiedEffect;
    public Transform bullet;
    public float distanceFromPlayerToStartMove = 20f;
    public float movementSpeed_Min = 1f;
    public float movementSpeed_Max = 2f;
    public bool canShoot;

    private bool moveRight;
    private float movementSpeed;
    private bool isPlayerInRegion;
    private Transform playerTransform;

    private string InvokeFunc = "StartShooting";

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            moveRight = true;
        } else
        {
            moveRight= false;
        }
        movementSpeed = Random.Range(movementSpeed_Min, movementSpeed_Max);

        playerTransform = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform)
        {
            float distanceFromPlayer = (playerTransform.position - transform.position).magnitude;
            if (distanceFromPlayer < distanceFromPlayerToStartMove)
            {
                if (moveRight)
                {
                    transform.position = new Vector3(transform.position.x + (Time.deltaTime * movementSpeed),
                        transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - (Time.deltaTime * movementSpeed),
                        transform.position.y, transform.position.z);
                }

                if (!isPlayerInRegion)
                {
                    if (canShoot)
                    {
                        InvokeRepeating(InvokeFunc, 0.5f, 1.5f);
                    }
                    isPlayerInRegion = true;
                }
            } else
            {
                CancelInvoke(InvokeFunc);
            }
        }
    }

    void StartShooting()
    {
        if (playerTransform)
        {
            Vector3 bulletPosition = transform.position;
            bulletPosition.y += 2f;
            bulletPosition.x -= 1f;
            Transform newBullet = Instantiate(bullet, bulletPosition, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }
    }

    void DemonDied()
    {
        Vector3 effectPosition = transform.position;
        effectPosition.y += 2f;

        Instantiate(demonDiedEffect, effectPosition, Quaternion.identity);
        // Destroy(gameObject) - computationally expensive process 
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider target)
    {
        if(target.CompareTag(Tags.PLAYER_BULLET_TAG))
        {
            GameplayController.gamePlayInstance.IncrementScore(10);
            DemonDied();
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if(target.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            GameplayController.gamePlayInstance.IncrementScore(10);
            DemonDied();
        }
    }
}
