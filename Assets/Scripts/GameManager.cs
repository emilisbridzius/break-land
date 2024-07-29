using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Movement move;
    public FirstPersonCam cam;
    public PlayerAttack attack;
    public PlayerTimer timer;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        move = FindObjectOfType<Movement>();
        cam = FindObjectOfType<FirstPersonCam>();
        attack = FindObjectOfType<PlayerAttack>();
        timer = FindObjectOfType<PlayerTimer>();
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        move.enabled = false;
        cam.enabled = false;
        attack.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.SetActive(true);
    }

    public void QuickRestart()
    {
        SceneManager.LoadScene("Golf");
        Time.timeScale = 1f;
        move.enabled = true;
        cam.enabled = true;
        attack.enabled = true;
        timer.time = timer.maxTime;
        gameOverPanel.SetActive(false);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
