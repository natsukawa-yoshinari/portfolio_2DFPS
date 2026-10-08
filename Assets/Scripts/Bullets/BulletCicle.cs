using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

public class BulletCicle : BulletBase
{
    /// <summary>
    /// ホーミングする弾
    /// Updateで常にPlayerを検知している
    /// </summary>

    [Header("BulletParams")]
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _destroyTime = 4f;
    [SerializeField]
    private float _homingTime = 1f;
    [SerializeField]
    private float _turnSpeed = 0.5f;
    private float _currentHomingTime = 0f;
    private bool _isEndHoming = false;
    private bool _isEndInitialize = true;

    private Vector3 _moveDirection;
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public override void OnEnable()
    {
        base.OnEnable();
        base.lifeTime = _destroyTime;
        _cts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    public async UniTask Initialize(PlayerCollider playerCollider, Vector3 startPos)
    {
        _isEndInitialize = false;
        base.Initialize(playerCollider);

        if (playerCollider != null)
        {
            Vector3 playerPos = playerCollider.GetPlayerTransform().position + playerCollider.offset;
            Vector3 myPos = this.gameObject.transform.position;

            Vector3 direction = playerPos - myPos;

            _moveDirection = direction.normalized;
        }

        _currentHomingTime = 0f;
        _isEndHoming = false;

        await LMotion.Create(this.gameObject.transform.position, startPos, 0.25f)
                .WithEase(Ease.Linear)
                .Bind(x =>
                {
                    this.gameObject.transform.position = x;
                }).ToUniTask(cancellationToken: _cts.Token);

        await UniTask.WaitForSeconds(0.25f, cancellationToken: _cts.Token);
        _cts = null;
        _isEndInitialize = true;
    }

    public override void Update()
    {
        if (GameManager.IsGameEnd) return;
        if (_isEndInitialize == false) return;

        _currentHomingTime += Time.deltaTime;
        if (_currentHomingTime > _homingTime) _isEndHoming = true;

        if (playerCollider != null && _isEndHoming == false)
        {
            Vector3 playerPos = playerCollider.GetPlayerTransform().position + playerCollider.offset;
            Vector3 targetDirection = (playerPos - this.gameObject.transform.position).normalized;
            targetDirection.z = 0f;

            _moveDirection = Vector3.RotateTowards(_moveDirection, targetDirection, _turnSpeed * Time.deltaTime, 0f);

            _moveDirection.Normalize();
        }

        this.gameObject.transform.position += _moveDirection * _speed * Time.deltaTime;

        base.Update();
    }
}
