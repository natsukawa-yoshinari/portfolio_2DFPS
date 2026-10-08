using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCollider _playerCollider;
    [SerializeField]
    private TrianglePool _triangleBulletPool;
    [SerializeField]
    private MotherCirclePool _motherCircleBulletPool;
    [SerializeField]
    private CirclePool _circleBulletPool;
    [SerializeField]
    private StarPool _starBulletPool;

    private GameObject _enemy;

    public void SetEnemyObject(GameObject enemy)
    {
        _enemy = enemy;
    }

    // 弾の生成
    public void GenerateBulletTriangle()
    {
        PooledObject obj = _triangleBulletPool.GetPooledObject();

        if (obj == null) return;
        obj.transform.position = _enemy.transform.position;

        BulletTriangle bullet = obj.GetComponent<BulletTriangle>();
        bullet.Initialize(_playerCollider);
    }

    public void GenerateBulletMotherCircle()
    {
        PooledObject obj = _motherCircleBulletPool.GetPooledObject();

        if (obj == null) return;
        obj.transform.position = _enemy.transform.position;

        BulletMotherCircle bullet = obj.GetComponent<BulletMotherCircle>();
        bullet.Initialize(_playerCollider, _circleBulletPool);
    }

    public void GenerateBulletStar()
    {
        PooledObject obj = _starBulletPool.GetPooledObject();

        if (obj == null) return;
        obj.transform.position = _enemy.transform.position;

        BulletStar bullet = obj.GetComponent<BulletStar>();
        bullet.Initialize(_playerCollider);
    }

    public void ClearAllPool()
    {
        _triangleBulletPool.ClearAllPoolObjects();
        _motherCircleBulletPool.ClearAllPoolObjects();
        _circleBulletPool.ClearAllPoolObjects();
        _starBulletPool.ClearAllPoolObjects();
    }
}
