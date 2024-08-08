using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScripts : MonoBehaviour
{
    // Deactivate all the objects that the player passed using the box collider following the player
    void OnTriggerEnter (Collider target)
    {
        if (target.CompareTag(Tags.PLATFORM_TAG) || target.CompareTag(Tags.HEALTH_TAG) || target.CompareTag(Tags.DEMON_TAG))
        {
            target.gameObject.SetActive(false);
        }
    }
}
