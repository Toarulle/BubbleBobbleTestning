using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{

    [SerializeField] private AudioClip playerJumpAudio = null; 
    [SerializeField] private AudioClip bubbleShootBubbleAudio = null; 
    [SerializeField] private AudioClip popBubbleWithEnemyAudio = null; 
    [SerializeField] private float volume = 0.6f; 
    private AudioSource audioSource;
    
    
    private static GameBehaviour instance = null;
    public static GameBehaviour Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayerJumpSound()
    {
        audioSource.PlayOneShot(playerJumpAudio, volume);
    }
    public void PopBubbleWithEnemySound()
    {
        audioSource.PlayOneShot(popBubbleWithEnemyAudio, volume);
    }
    public void ShootBubbleSound()
    {
        audioSource.PlayOneShot(bubbleShootBubbleAudio, volume);
    }
}
