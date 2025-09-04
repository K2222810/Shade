using NUnit.Framework;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    int progresAmount;
    public Slider progressSlider;

    public GameObject Player;
    public GameObject LoadCanvas;
    public List<GameObject> levels; 
    private int currentLevelIndex = 0;
    private int progressAmount;

    public GameObject gameOverScreen;
    public TMP_Text surviveText;
    private int survivedlevelCount;

    public static event Action OnReset; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progresAmount = 0;
        progressSlider.value = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;
        HoldToThisLevel.OnHoldComplete += LoadNextLevel;
        HealthPlayer.OnPlayDied += GameOverScreen;
        LoadCanvas.SetActive(false);
        gameOverScreen.SetActive(false);    
    }
    void GameOverScreen()
    { 
        gameOverScreen.SetActive(true);
        surviveText.text = "YOU SURVIVED" + survivedlevelCount + "lEVELS";
        if (survivedlevelCount != 1) surviveText.text += "S";
        Time.timeScale = 0; 
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        survivedlevelCount = 0;
        LoadLevel(0,false); 
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    void IncreaseProgressAmount(int amount) 
    {
        progresAmount += amount;
        progressSlider.value = progresAmount;
        if (progresAmount > 100) 
        {   
            LoadCanvas.SetActive(true); 
            Debug.Log("Level complete");
        }
    }

    void LoadLevel(int level,bool wantSurvivedIncrease)
    {
        LoadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        Player.transform.position = new Vector3(-6, 2, 0);

        currentLevelIndex = level;
        progressAmount = 0;
        progressSlider.value = 0;
        if(wantSurvivedIncrease) survivedlevelCount++; 


    }
    void LoadNextLevel() 
    { 
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex,true);
    }
}
