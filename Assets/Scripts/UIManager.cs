using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText, _bestScoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartLevel;

    private GameManager _gameManager;
    private int _bestScore = 0;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartLevel.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();

        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
        _bestScoreText.text = "Best: " + _bestScore;

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CheckForBestScore()
    {
        if (player._score > _bestScore)
        {
            _bestScore = player._score;
            PlayerPrefs.SetInt("Best Score", _bestScore);
        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartLevel.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void BackToMainMenu()
    {
        _gameManager.IsNotPaused();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
