using UnityEngine;

public class BulletBase : PooledObject
{
    [Header("Collider")]
    [SerializeField]
    private float _radius; // 判定の大きさ
    [SerializeField]
    private Vector3 _offset; // 中心

    [Header("BaseParams")]
    [SerializeField]
    private float _damage = 1f;

    private float _lifeTime;
    public float lifeTime { get => _lifeTime; set => _lifeTime = value; }
    private float _currentTime = 0f;
    private PlayerCollider _playerCollider;
    public PlayerCollider playerCollider => _playerCollider;

    public virtual void OnEnable()
    {
        _currentTime = 0f;
    }

    /// <summary>
    /// BulletBase.cs 敵が発射する弾の基底クラス。当たり判定を自作。
    /// </summary>
    public virtual void Update()
    {
        // 時間経過で破壊
        _currentTime += Time.deltaTime;
        if (_lifeTime < _currentTime)
        {
            _currentTime = 0f;
            Release();
            return;
        }

        // 当たり判定
        bool isHit = CheckHitPlayer(_playerCollider);
        if (isHit)
        {
            OnHitPlayer(_playerCollider);
            _currentTime = 0f;
            Release();
        }
    }

    // Playerにあたったかの確認
    public virtual bool CheckHitPlayer(PlayerCollider playerCollider)
    {
        Transform playerTransform = playerCollider.GetPlayerTransform();
        Vector3 playerPos = playerTransform.position + playerCollider.offset;

        Vector3 myPos = this.gameObject.transform.position + _offset;

        // 自分とプレイヤーの距離
        float objSqrDistance = (playerPos - myPos).sqrMagnitude;

        // コライダー範囲
        float colDistance = (playerCollider.radius + _radius) * (playerCollider.radius + _radius);

        // 自分とプレイヤーの距離が範囲内であれば当たった
        bool isHit = objSqrDistance < colDistance;

        return isHit;
    }

    // プレイヤーにあたった時に呼ばれる
    public virtual void OnHitPlayer(PlayerCollider playerCollider)
    {
        if (playerCollider == null) return;
        playerCollider.AddDamage(_damage);
    }

    // BulletManagerが自身を生成する際に呼び出す
    public virtual void Initialize(PlayerCollider playerCollider)
    {
        _playerCollider = playerCollider;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 centerPos = this.gameObject.transform.position + _offset;

        Gizmos.DrawWireSphere(centerPos, _radius);
    }
}
