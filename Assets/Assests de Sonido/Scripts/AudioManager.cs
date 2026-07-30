using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public AmbienceManager Ambience {get; private set;}
    public SFXManager SFX {get; private set;}
   

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            SFX = GetComponentInChildren<SFXManager>();
            Ambience = GetComponentInChildren<AmbienceManager>();

            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Debug.Log("Cuidado! Más de un AudioManager en escena.");
            Destroy(gameObject);
        }
        //Simplifies the code to call it from other scripts.
        
    }

        
   }

/* 

Declare this in the CharacterController Script: 

        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip successSound;

Use this in the function: 

        AudioManager.Instance.SFX.Play(jumpSound)
        AudioManager.Instance.SFX.Play(damageSound)
        AudioManager.Instance.SFX.Play (successSound)

*/


