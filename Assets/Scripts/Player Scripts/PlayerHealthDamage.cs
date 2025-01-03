using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthDamage : MonoBehaviour
{
    public float distanceBeforeNewPlatforms = 120f;
    [HideInInspector]
    public bool canShoot;

    private LevelGenerator levelGenerator;
    /*private LevelPooling levelPooling;*/
    [SerializeField]
    private Transform playerBullet;

    // Start is called before the first frame update
    void Start()
    {
        /*levelPooling = GameObject.Find(Tags.LEVEL_GENERATOR_GAME_OBJ).GetComponent<LevelPooling>();*/
        levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR_GAME_OBJ).GetComponent<LevelGenerator>();
    }

    void FixedUpdate()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                Vector3 bulletPosition = transform.position;
                bulletPosition.x += 1f;
                bulletPosition.y += 2f;

                Transform newBullet = Instantiate(playerBullet, bulletPosition, Quaternion.identity);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
                newBullet.parent = transform;
            }
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(Tags.DEMON_BULLET_TAG) || target.CompareTag(Tags.BOUNDS_TAG))
        {
            // inform game controller that player died
            GameplayController.gamePlayInstance.TakeDamage();
            Destroy(gameObject);
        }

        if (target.CompareTag(Tags.HEALTH_TAG))
        {
            GameplayController.gamePlayInstance.IncrementHealth();
            target.gameObject.SetActive(false);
        }

        if (target.CompareTag(Tags.MORE_PLATFORMS_TAG))
        {
            Vector3 temp = target.transform.position;
            temp.x += distanceBeforeNewPlatforms;
            target.transform.position = temp;

            levelGenerator.GenerateLevel(false);
            /*levelPooling.PoolingPlatforms();*/
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.CompareTag(Tags.DEMON_TAG))
        {
            GameplayController.gamePlayInstance.TakeDamage();
            Destroy(gameObject);
        }
    }
}
