using UnityEngine;

public class BulletStar : BulletBase
{
    /// <summary>
    /// 射出時のPlayerの位置に向かって飛んでくる弾
    /// </summary>

    [Header("BulletParams")]
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _destroyTime = 4f;
    private Vector3 _moveDirection;

    public override void OnEnable()
    {
        base.OnEnable();
        base.lifeTime = _destroyTime;
    }

    public override void Initialize(PlayerCollider playerCollider)
    {
        base.Initialize(playerCollider);

        if (playerCollider != null)
        {
            Vector3 playerPos = playerCollider.GetPlayerTransform().position + playerCollider.offset;
            Vector3 myPos = this.gameObject.transform.position;

            Vector3 direction = playerPos - myPos;

            _moveDirection = direction.normalized;
        }
    }


    public override void Update()
    {
        if (GameManager.IsGameEnd) return;
        this.gameObject.transform.position += _moveDirection * _speed * Time.deltaTime;

        base.Update();
    }
}
