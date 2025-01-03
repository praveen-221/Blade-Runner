using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime = 10f;
    public float startY;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        StartCoroutine(TurnOffBullet());
    }

    // LateUpdate is called after Update function 
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    IEnumerator TurnOffBullet()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
    
    void OnTriggerEnter (Collider target)
    {
        if(target.CompareTag(Tags.DEMON_TAG) || target.CompareTag(Tags.PLAYER_TAG)
            || target.CompareTag(Tags.DEMON_BULLET_TAG) || target.CompareTag(Tags.PLAYER_BULLET_TAG))
        {
            gameObject.SetActive(false);
        }
    }
}
