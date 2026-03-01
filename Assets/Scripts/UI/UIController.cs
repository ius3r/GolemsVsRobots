using UnityEngine;

public sealed class UIController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    [Header("Input")]
    [SerializeField] private KeyCode pauseKey = KeyCode.P;

    private Health _coreHealth;
    private bool _isPaused;
    private bool _isGameOver;

    private void Awake()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
    }

    private void Start()
    {
        CoreTarget core = Object.FindFirstObjectByType<CoreTarget>();
        if (core != null)
        {
            _coreHealth = core.GetComponent<Health>();
            if (_coreHealth != null)
            {
                _coreHealth.Died += OnCoreDied;
            }
        }
    }

    private void OnDestroy()
    {
        if (_coreHealth != null)
        {
            _coreHealth.Died -= OnCoreDied;
        }
    }

    private void Update()
    {
        if (_isGameOver) return;

        if (Input.GetKeyDown(pauseKey))
        {
            SetPaused(!_isPaused);
        }
    }

    private void OnCoreDied(Health _)
    {
        _isGameOver = true;
        _isPaused = false;

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(true);

        Time.timeScale = 0f;
    }

    private void SetPaused(bool paused)
    {
        _isPaused = paused;

        if (pauseMenu != null) pauseMenu.SetActive(paused);

        Time.timeScale = paused ? 0f : 1f;
    }
}
