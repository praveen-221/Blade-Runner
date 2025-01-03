using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    /*
     * SerializeField serializes the field & make the private variables visible in inspecctor
     * HideInInspector hides the public variables in inspector
     */
    public float offsetSpeed = 0.001f;
    [HideInInspector]
    public bool canScroll;
    private Renderer meshRednderer;

    // Start is called before the first frame update
    void Awake()
    {
        meshRednderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canScroll)
        {
            meshRednderer.material.mainTextureOffset += new Vector2(offsetSpeed, 0);
        }
    }
}
