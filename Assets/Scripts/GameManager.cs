using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Scripts")]
    [SerializeField]
    private PlayerManager _playerManager;
    [SerializeField]
    private BulletManager _bulletManager;
    [SerializeField]
    private EnemyManager _enemyManager;
    [SerializeField]
    private PlayerCollider _playerCollider;

    [Header("Params")]
    [SerializeField]
    private ReactiveProperty<int> _timeLimit = new ReactiveProperty<int>(60);
    private float _currentTime = 0f;
    private int _defaultTimeLimit;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private Slider _hpSlider;
    [SerializeField]
    private GameObject _startCanvas;
    [SerializeField]
    private GameObject _endCanvas;
    [SerializeField]
    private TextMeshProUGUI _resultText;

    static public bool IsGameEnd = true;

    private float _triangleBulletInterval = 0.35f;
    private float _triangleBulletTime = 0f;
    private float _circleBulletInterval = 1f;
    private float _circleBulletTime = 0f;
    private float _starBulletInterval = 0.2f;
    private float _starBulletTime = 0f;

    private void Start()
    {
        _defaultTimeLimit = _timeLimit.Value;
        _timeText.gameObject.SetActive(false);

        _playerManager.playerHp.Subscribe(x =>
        {
            _hpSlider.value = x / _playerManager.maxHp;
            if (x <= 0)
            {
                GameOver();
            }
        }).AddTo(this);

        _timeLimit.Subscribe(x =>
        {
            _timeText.text = x.ToString("00");

            if (_timeLimit.Value <= 0)
            {
                GameWin();
            }
        }).AddTo(this);
    }

    private void Update()
    {
        if (IsGameEnd) return;

        EnemyBehaviour();

        _currentTime += Time.deltaTime;
        if (_currentTime > 1f)
        {
            _timeLimit.Value -= 1;

            _currentTime = 0f;
        }
    }

    private void EnemyBehaviour()
    {
        if (_enemyManager.currentEnemySequence == EnemyManager.EnemySequence.None) return;

        if (_enemyManager.currentEnemySequence == EnemyManager.EnemySequence.Triangle)
        {
            TriangleBehaviour();
        }

        if (_enemyManager.currentEnemySequence == EnemyManager.EnemySequence.UpDown)
        {
            UpDownBehaviour();
        }
    }

    private void TriangleBehaviour()
    {
        _triangleBulletTime += Time.deltaTime;
        if (_triangleBulletTime > _triangleBulletInterval)
        {
            _bulletManager.GenerateBulletTriangle();
            _triangleBulletTime = 0f;
        }

        _circleBulletTime += Time.deltaTime;
        if (_circleBulletTime > _circleBulletInterval)
        {
            _bulletManager.GenerateBulletMotherCircle();
            _circleBulletTime = 0f;
        }
    }

    private void UpDownBehaviour()
    {
        _starBulletTime += Time.deltaTime;
        if (_starBulletTime > _starBulletInterval)
        {
            _bulletManager.GenerateBulletStar();
            _starBulletTime = 0f;
        }

        _circleBulletTime += Time.deltaTime;
        if (_circleBulletTime > _circleBulletInterval)
        {
            _bulletManager.GenerateBulletMotherCircle();
            _circleBulletTime = 0f;
        }
    }

    private void GameOver()
    {
        GameEnd();
        _resultText.text = "You are dead...";
    }

    private void GameWin()
    {
        GameEnd();
        _resultText.text = "You survived!";
    }

    private void GameEnd()
    {
        _enemyManager.StopLoop();
        _bulletManager.ClearAllPool();
        _endCanvas.SetActive(true);
        IsGameEnd = true;
    }

    public void Gamestart()
    {
        IsGameEnd = false;
        _enemyManager.currentEnemySequence = EnemyManager.EnemySequence.None;

        _startCanvas.SetActive(false);
        _endCanvas.SetActive(false);
        _timeText.gameObject.SetActive(true);

        _playerCollider.ResetPlayer();


        _playerManager.Restart();

        _enemyManager.GameStart();

        _timeLimit.Value = _defaultTimeLimit;
        _currentTime = 0f;
        _circleBulletTime = 0f;
        _starBulletTime = 0f;
        _triangleBulletTime = 0f;

    }
}
