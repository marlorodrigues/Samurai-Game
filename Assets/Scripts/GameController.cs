using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverPanel;

    private bool returnToInit = false;

    public void showGameOver()
    {
        gameOverPanel.SetActive(true);
        returnToInit = true;
    }

    private void Update()
    {
        if (!returnToInit)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        returnToInit = false;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
