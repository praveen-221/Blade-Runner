/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPooling : MonoBehaviour
{
    [SerializeField]
    private Transform platformPrefab, platformParent;

    [SerializeField]
    private Transform demonPrefab, demonParent;

    [SerializeField]
    private Transform healthCollectiblePrefab, healthCollectibleParent;

    [SerializeField]
    private int levelLength = 100;

    [SerializeField]
    private float distance_Between_Platforms = 10f;

    [SerializeField]
    private float platformPosition_MinY = 0f, platformPosition_MaxY = 7f;

    [SerializeField]
    private int minPlatformLength = 2, maxPlatformLength = 4;

    [SerializeField]
    private float chanceForDemonExistence = 0.25f, chanceForCollectibleExistence = 0.1f;

    [SerializeField]
    private float healthCollectiblePosition_MinY = 1f, healthCollectiblePosition_MaxY = 3f;

    private float platformLastPositionX;
    private Transform[] platformArray;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePlatforms()
    {
        platformArray = new Transform[levelLength];

        for (int i = 0; i < platformArray.Length; i++)
        {
            Transform platform = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity);
            platformArray[i] = platform;
        }

        for (int i = 0; i < platformArray.Length; ++i)
        {
            float platformPositionY = Random.Range(platformPosition_MinY, platformPosition_MaxY);

            Vector3 position;

            if (i < 5)
            {
                platformPositionY = 0f;
            }

            position = new Vector3(distance_Between_Platforms * i, platformPositionY, 0);
            platformLastPositionX = position.x;

            platformArray[i].position = position;
            platformArray[i].parent = platformParent;

            // spawn demons & health collectibles
            SpawnHealthAndDemons(position, i, true);
        }
    }

    public void PoolingPlatforms()
    {
        for (int i = 0; i < platformArray.Length; ++i)
        {
            if (!platformArray[i].gameObject.activeInHierarchy)     // check if the platform is not active
            {
                platformArray[i].gameObject.SetActive(true);
                float platformPositionY = Random.Range(platformPosition_MinY, platformPosition_MaxY);

                Vector3 position;
                position = new Vector3(distance_Between_Platforms + platformLastPositionX, platformPositionY, 0);

                platformArray[i].position = position;
                platformLastPositionX = position.x;

                // spawn health collectible & demons
                SpawnHealthAndDemons(position, i, false);
            }
        }
    }

    void SpawnHealthAndDemons(Vector3 platformPosition, int i, bool isFirstSpawn)
    {
        if (i > 5)
        {
            if (Random.Range(0f, 1f) < chanceForDemonExistence)
            {
                if (isFirstSpawn)
                {
                    platformPosition = new Vector3(distance_Between_Platforms * i, platformPosition.y + 0.1f, 0);
                } else
                {
                    platformPosition = new Vector3(distance_Between_Platforms + platformLastPositionX, platformPosition.y + 0.1f, 0);
                }
                Transform createDemon = Instantiate(demonPrefab, platformPosition, Quaternion.Euler(0, -90, 0));
                createDemon.parent = demonParent;
            }

            if (Random.Range(0f, 1f) < chanceForCollectibleExistence)
            {
                if (isFirstSpawn)
                {
                    platformPosition = new Vector3(distance_Between_Platforms * i, 
                        platformPosition.y + Random.Range(healthCollectiblePosition_MinY, healthCollectiblePosition_MaxY), 
                        0);
                } else
                {
                    platformPosition = new Vector3(distance_Between_Platforms + platformLastPositionX,
                        platformPosition.y + Random.Range(healthCollectiblePosition_MinY, healthCollectiblePosition_MaxY),
                        0);
                }
                Transform createCollectible = Instantiate(healthCollectiblePrefab, platformPosition, Quaternion.identity);
                createCollectible.parent = healthCollectibleParent;
            }
        }
    }
}
*/