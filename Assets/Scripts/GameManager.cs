using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private AudioSource scoreSFX;

    [Space(10)]
    [SerializeField] private GameObject winCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        txtScore.text = $"Score: {score}";

        scoreSFX.Play();
    }

    public void Win()
    {
        winCanvas.SetActive(true);
        Invoke("ReloadScene", 2);
    }
    private void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
