using UnityEngine;
using TMPro;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerContainer;

    private bool debugLogged = false;

    private void Start()
    {
        // Validate that timerText is assigned
        if (timerText == null)
        {
            Debug.LogError("WaveTimerUI: timerText is not assigned in the Inspector!");
            return;
        }

        // Ensure GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("WaveTimerUI: GameManager.Instance not found. Creating a new GameManager.");
            CreateGameManager();
        }
    }

    private void CreateGameManager()
    {
        // Create a new GameManager if none exists
        GameObject gmObj = new GameObject("GameManager");
        gmObj.AddComponent<GameManager>();
    }

    private void Update()
    {
        // Wait for GameManager to be available
        if (GameManager.Instance == null)
        {
            if (!debugLogged)
            {
                Debug.LogWarning("WaveTimerUI: Waiting for GameManager.Instance to initialize...");
                debugLogged = true;
            }
            return;
        }

        if (timerText == null)
        {
            if (!debugLogged)
            {
                Debug.LogError("WaveTimerUI: timerText is null");
                debugLogged = true;
            }
            return;
        }

        float timeRemaining = GameManager.Instance.GetWaveTimeRemaining();
        bool waveActive = GameManager.Instance.IsWaveActive();

        // Show or hide the timer based on wave active state
        if (timerContainer != null)
        {
            timerContainer.SetActive(waveActive);
        }
        else
        {
            timerText.gameObject.SetActive(waveActive);
        }

        // Update the timer display
        if (waveActive)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Wave {GameManager.Instance.currentWave}: {seconds}s";
        }
    }
}
