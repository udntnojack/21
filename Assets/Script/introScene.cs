using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void startGame()
    {
        score.lives = 5;
        SceneManager.LoadScene("game");
    }
}
