using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour

{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip rocketCrash;
    [SerializeField] AudioClip rocketSuccess;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;  // toggle collision
        }
    }
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionDisabled) { return; }
        

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This this rocks!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence(); 
                break;
        }
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();
        isTransitioning = true;
        audioSource.PlayOneShot(rocketSuccess);
        GetComponent<Move>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
        successParticles.Play();
    }

    void StartCrashSequence()
    {
        audioSource.Stop();
        isTransitioning = true;
        audioSource.PlayOneShot(rocketCrash);
        crashParticles.Play();
       
        //add particle effect
        GetComponent<Move>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void ReloadLevel()
    
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
}
