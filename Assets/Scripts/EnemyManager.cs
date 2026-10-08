using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private BulletManager _bulletManager;
    private GameObject _enemy;

    private float upDownDistance = 4f;
    private float upDownTimes = 3f;
    private Vector3 _defaultPos = new Vector3(7f, 0f, 0f);

    private Vector3 _triangleFirstPos = new Vector3(3.5f, 3.3f, 0f);
    private Vector3 _triangleSecondPos = new Vector3(7f, 0f, 0f);
    private Vector3 _triangleThirdPos = new Vector3(3.5f, -3.3f, 0f);
    private MotionHandle _activeMotion;
    private CancellationTokenSource _restartCts;
    public EnemySequence currentEnemySequence;
    public enum EnemySequence
    {
        None = 0,
        Triangle,
        UpDown,
    }

    public void GameStart()
    {
        if (_enemy == null) _enemy = Instantiate(_enemyPrefab);
        _bulletManager.SetEnemyObject(_enemy);

        currentEnemySequence = EnemySequence.None;
        _enemy.transform.position = _defaultPos;

        _restartCts = new CancellationTokenSource();
        EnemyLoop(_restartCts.Token).Forget();
    }

    public void StopLoop()
    {
        if (_restartCts != null)
        {
            _restartCts.Cancel();
            _restartCts.Dispose();
            _restartCts = null;
        }

        if (_activeMotion.IsActive())
        {
            _activeMotion.Cancel();
        }

        if (_enemy != null) Destroy(_enemy);
    }

    public async UniTaskVoid EnemyLoop(CancellationToken ct)
    {
        currentEnemySequence = EnemySequence.None;
        await UniTask.Delay(1000);

        while (!ct.IsCancellationRequested)
        {

            int nextSequence = UnityEngine.Random.Range(0, 1 + 1);
            switch (nextSequence)
            {
                case 0:
                    int times = UnityEngine.Random.Range(5, 7 + 1);
                    currentEnemySequence = EnemySequence.Triangle;
                    await TriangleSequence(times, ct);
                    break;
                case 1:
                    float moveTime = UnityEngine.Random.Range(5, 7 + 1);
                    currentEnemySequence = EnemySequence.UpDown;
                    await UpDownSequence(moveTime, ct);
                    break;

                default:
                    break;
            }

            currentEnemySequence = EnemySequence.None;
            await UniTask.Delay(2000, cancellationToken: ct);
        }
    }

    private async UniTask TriangleSequence(int time, CancellationToken ct)
    {
        int currentTime = 0;
        int currentTarget = 0;
        int nextTarget = 0;
        while (currentTime < time)
        {
            while (currentTarget == nextTarget)
            {
                nextTarget = UnityEngine.Random.Range(0, 2 + 1);
            }
            currentTarget = nextTarget;

            Vector3 targetPos = Vector3.zero;
            switch (nextTarget)
            {
                case 0:
                    targetPos = _triangleFirstPos;
                    break;
                case 1:
                    targetPos = _triangleSecondPos;
                    break;
                case 2:
                    targetPos = _triangleThirdPos;
                    break;
            }

            if (_enemy == null) return;

            _activeMotion = LMotion.Create(_enemy.transform.position, targetPos, 1.5f)
                        .WithEase(Ease.Linear)
                        .Bind(x =>
                        {
                            _enemy.transform.position = x;
                        });


            currentTime += 1;
        }

        if (_enemy == null) return;

        await _activeMotion.ToUniTask(cancellationToken: ct);

        _activeMotion = LMotion.Create(_enemy.transform.position, _defaultPos, 1f)
                    .WithEase(Ease.Linear)
                    .Bind(x =>
                    {
                        _enemy.transform.position = x;
                    });

        await _activeMotion.ToUniTask(cancellationToken: ct);
    }

    private async UniTask UpDownSequence(float time, CancellationToken ct)
    {
        float currentTime = 0f;
        float startTime = Time.time;

        while (currentTime < time)
        {
            if (_enemy == null) return;

            float passedTime = Time.time - startTime;
            float newY = _defaultPos.y + Mathf.Sin((passedTime * 2 * Mathf.PI) / upDownTimes) * upDownDistance;
            _enemy.transform.position = new Vector3(_defaultPos.x, newY, _defaultPos.z);

            currentTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: ct);
        }

        if (_enemy == null) return;

        _activeMotion = LMotion.Create(_enemy.transform.position, _defaultPos, 1f)
                    .WithEase(Ease.Linear)
                    .Bind(x =>
                    {
                        _enemy.transform.position = x;
                    });

        await _activeMotion.ToUniTask(cancellationToken: ct);
    }
}