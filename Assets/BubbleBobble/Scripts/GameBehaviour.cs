using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{

    [SerializeField] private AudioClip playerJumpAudio = null; 
    [SerializeField] private AudioClip bubbleShootBubbleAudio = null; 
    [SerializeField] private AudioClip popBubbleWithEnemyAudio = null; 
    [SerializeField] private float volume = 0.6f; 
    [SerializeField] private DeathPortObject deathPort; 
    [SerializeField] private SFXPortObject sfxPort; 
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerLivesPanel;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private string gameScene;
    [SerializeField] private string loadingScene;

    private List<GameObject> lives = new List<GameObject>();
    private AudioSource audioSource;
    private int enemyCount;
    
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
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == loadingScene)
            return;
        
        audioSource = GetComponent<AudioSource>();
        lives = new List<GameObject>();
        enemyCount = FindObjectsOfType<EnemyBehaviour>().Length;
        foreach (var life in playerLivesPanel.GetComponentsInChildren<Image>())
        {
            lives.Add(life.gameObject);
        }
    }

    private void PlaySound(string sound)
    {
        switch (sound)
        {
            case "PlayerJump":
                audioSource.PlayOneShot(playerJumpAudio, volume);
                break;
            case "ShootBubble":
                audioSource.PlayOneShot(bubbleShootBubbleAudio, volume);
                break;
            case "PopBubbleWithEnemy":
                audioSource.PlayOneShot(popBubbleWithEnemyAudio, volume);
                break;
        }
    }

    private void EndGame(string endGameText)
    {
        this.endGameText.text = endGameText;
        endGamePanel.SetActive(true);
        PlayAgain();
    }

    private void RemoveLife()
    {
        Destroy(lives.Last().gameObject);
        lives.RemoveAt(lives.Count-1);
        if (lives.Count == 0)
        {
            EndGame("You died");
        }
    }

    public void RespawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    }
    private void SomethingDied(int pts)
    {
        if (pts == -1)
        {
            RemoveLife();
            RespawnPlayer();
        }
        else
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                EndGame("Level complete!");
            }
        }
    }

    public void PlayAgain()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(gameScene))
        {
            SceneManager.LoadScene(loadingScene, LoadSceneMode.Single);
            return;
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(loadingScene))
        {
            SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
            return;
        }
    }
    
    private void ResetLevel(Scene scene, LoadSceneMode mode)
    {
        Start();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(loadingScene))
        {
            PlayAgain();
        }
    }
    
    private void OnEnable()
    {
        deathPort.OnDeath += SomethingDied;
        SceneManager.sceneLoaded += ResetLevel;
        sfxPort.OnPlaySound += PlaySound;
    }

    private void OnDisable()
    {
        deathPort.OnDeath -= SomethingDied;
        SceneManager.sceneLoaded -= ResetLevel;
        sfxPort.OnPlaySound -= PlaySound;
    }
}
