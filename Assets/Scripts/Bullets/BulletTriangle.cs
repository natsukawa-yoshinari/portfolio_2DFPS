using UnityEngine;

public class BulletTriangle : BulletBase
{
    /// <summary>
    /// まっすぐ飛んでくる弾
    /// </summary>

    [Header("BulletParams")]
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _destroyTime = 4f;

    public override void OnEnable()
    {
        base.OnEnable();
        base.lifeTime = _destroyTime;
    }

    public override void Update()
    {
        if (GameManager.IsGameEnd) return;
        this.gameObject.transform.position += Vector3.left * _speed * Time.deltaTime;

        base.Update();
    }

    public override void OnHitPlayer(PlayerCollider playerCollider)
    {
        base.OnHitPlayer(playerCollider);
    }
}
