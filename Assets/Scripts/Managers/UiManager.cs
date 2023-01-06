using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Player;

namespace UI
{
    public class UiManager : GenericSingleton<UiManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverFinalScoreText;
        private int score = -1;
        private PlayerController playerController;

        private void Start()
        {
            playerController = PlayerController.Instance ;
        }
        public void UpdateScore()
        {
            score++;
            scoreText.text = score.ToString();
        }

        public void GameOver()
        {
            gameOverFinalScoreText.text = "Your Score: " + scoreText.text;
            gameOverPanel.SetActive(true);
        }

        public void ChangeScene(int n)
        {
            SceneManager.LoadScene(n);
        }

        public void LoadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GameExit()
        {
            Application.Quit();
        }

        public void SetJump(bool temp)
        {
            playerController.SetJump(temp);
        }

    }
}