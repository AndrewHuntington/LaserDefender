using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // using Awake instead of Start
    void Awake()
    {
        // will keep music persistant through all scenes
        SetUpSingleton(); 
    }

    private void SetUpSingleton()
    {
        // if it finds another gameObeject of type MusicPlayer, will destroy it, else will not destroy
        if (FindObjectsOfType(GetType()).Length > 1) // GetType() = MusicPlayer; gets type of this class
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
