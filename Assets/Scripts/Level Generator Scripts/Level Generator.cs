using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int levelLength = 100;

    [SerializeField]
    private int startPlatformLength = 5, endPlatformLength = 5;

    [SerializeField]
    private int distanceBetweenPlatforms = 6;

    [SerializeField]
    private Transform platformPrefab, platformParent;

    [SerializeField]
    private Transform demonPrefab, demonParent;

    [SerializeField]
    private Transform healthCollectiblePrefab, healthCollectibleParent;

    [SerializeField]
    private float platformPosition_MinY = 0, platformPosition_MaxY = 10;

    [SerializeField]
    private int minPlatformLength = 2, maxPlatformLength = 4;

    [SerializeField]
    private float chanceForDemonExistence = 0.25f, chanceForCollectibleExistence = 0.1f;

    [SerializeField]
    private float healthCollectiblePosition_MinY = 1f, healthCollectiblePosition_MaxY = 3f;

    private float platformLastPositionX;

    private enum PlatformType
    {
        None, // platform does not exist
        Flat  // platform exists
    }

    private class PlatformPositionInfo
    {
        public PlatformType platformType;
        public float platformPositionY;
        public bool hasDemon;
        public bool hasHealthCollectible;

        public PlatformPositionInfo(PlatformType platformType, float platformPositionY, bool hasDemon, bool hasHealthCollectible)
        {
            this.platformType = platformType;
            this.platformPositionY = platformPositionY;
            this.hasDemon = hasDemon;
            this.hasHealthCollectible = hasHealthCollectible;
        }
    }

    // create the platforms 
    void FillPlatformInfo(PlatformPositionInfo[] platformInfo)
    {
        int currentPlatformIndex = 0;

        // create starting platform without demons & collectibles
        for (int i = 0; i < startPlatformLength; i++)
        {
            platformInfo[currentPlatformIndex].platformType = PlatformType.Flat;
            platformInfo[currentPlatformIndex].platformPositionY = 0f;

            currentPlatformIndex++;
        }

        while (currentPlatformIndex < levelLength - endPlatformLength)
        {
            if (platformInfo[currentPlatformIndex].platformType != PlatformType.None)
            {
                currentPlatformIndex++;
                continue;
            }

            float positionY = Random.Range(platformPosition_MinY, platformPosition_MaxY);
            int len = Random.Range(minPlatformLength, maxPlatformLength);

            for (int i = 0; i < len; i++)
            {
                bool demon = (Random.Range(0f, 1f) < chanceForDemonExistence);
                bool collectible = (Random.Range(0f, 1f) < chanceForCollectibleExistence);

                platformInfo[currentPlatformIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformIndex].platformPositionY = positionY;
                platformInfo[currentPlatformIndex].hasDemon = demon;
                platformInfo[currentPlatformIndex].hasHealthCollectible = collectible;

                currentPlatformIndex++;

                if (currentPlatformIndex > levelLength - endPlatformLength)
                {
                    currentPlatformIndex = levelLength - endPlatformLength; 
                    break;
                }
            }

            for (int i = 0; i < endPlatformLength;  i++)
            {
                platformInfo[currentPlatformIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformIndex].platformPositionY = 0f;

                currentPlatformIndex++;
            }
        }
    }

    void CreatePlatforms (PlatformPositionInfo[] platformInfo, bool isFirstSpawn)
    {
        for (int i = 0; i < platformInfo.Length; i++)
        {
            PlatformPositionInfo platform = platformInfo[i];

            if (platform.platformType == PlatformType.None)
            {
                continue;
            }

            Vector3 position;

            // check whether game started
            if (isFirstSpawn)
            {
                position = new Vector3(distanceBetweenPlatforms * i, platform.platformPositionY, 0);
            } else
            {
                position = new Vector3(distanceBetweenPlatforms + platformLastPositionX, platform.platformPositionY, 0);
            }

            // save the platform pos x for later use
            platformLastPositionX = position.x;

            Transform createBlock = Instantiate (platformPrefab, position, Quaternion.identity);
            createBlock.parent = platformParent;

            if (platform.hasDemon)
            {
                if (isFirstSpawn)
                {
                    position = new Vector3(distanceBetweenPlatforms * i, platform.platformPositionY + 0.1f, 0);
                } else
                {
                    position = new Vector3(distanceBetweenPlatforms + platformLastPositionX, platform.platformPositionY + 0.1f, 0);
                }

                Transform createDemon = Instantiate(demonPrefab, position, Quaternion.Euler(0, -90, 0));
                createDemon.parent = demonParent;
            }

            if (platform.hasHealthCollectible)
            {
                if (isFirstSpawn)
                {
                    position = new Vector3(distanceBetweenPlatforms * i, platform.platformPositionY + Random.Range(healthCollectiblePosition_MinY, healthCollectiblePosition_MaxY), 0);
                }
                else
                {
                    position = new Vector3(distanceBetweenPlatforms + platformLastPositionX, platform.platformPositionY + Random.Range(healthCollectiblePosition_MinY, healthCollectiblePosition_MaxY), 0);
                }

                Transform createCollectible = Instantiate(healthCollectiblePrefab, position, Quaternion.identity);
                createCollectible.parent = healthCollectibleParent;
            }
        }
    }

    // Generate the platforms
    public void GenerateLevel(bool isFirstSpawn)
    {
        PlatformPositionInfo[] platforms = new PlatformPositionInfo[levelLength];
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i] = new PlatformPositionInfo(PlatformType.None, -1f, false, false);
        }

        FillPlatformInfo(platforms);
        CreatePlatforms(platforms, isFirstSpawn);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel(true);
    }
}
