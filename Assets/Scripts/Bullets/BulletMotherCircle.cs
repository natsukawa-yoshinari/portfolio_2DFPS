using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BulletMotherCircle : BulletBase
{
    /// <summary>
    /// 発射数秒後に円状に分裂、
    /// 追尾弾であるCircleBulletを生成する
    /// </summary>

    [Header("BulletParams")]
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _destroyTime = 4f;

    [SerializeField]
    private float _genBulletRadius = 1f;
    [SerializeField]
    private int _genBulletNum = 8;

    private CirclePool _circlePool;
    private float _divideTime = 0f;

    public override void OnEnable()
    {
        base.OnEnable();
        base.lifeTime = _destroyTime;
        _divideTime = 0f;
    }


    private void GenCircleBullet()
    {
        float angle = 360 / (float)_genBulletNum;

        for (int i = 0; i < _genBulletNum; i++)
        {
            // 現在の角度をループ回数で記録
            float currentAngle = angle * i;

            // なんとMathfの三角関数の引数はラジアンなので、角度から弧度法に変換
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3();
            pos.y = _genBulletRadius * Mathf.Sin(rad);
            pos.x = _genBulletRadius * Mathf.Cos(rad);

            PooledObject obj = _circlePool.GetPooledObject();
            obj.transform.position = this.gameObject.transform.position;

            BulletCicle bullet = obj.GetComponent<BulletCicle>();
            bullet.Initialize(base.playerCollider, pos + this.gameObject.transform.position).Forget();
        }
    }

    public override void Update()
    {
        if (GameManager.IsGameEnd) return;
        this.gameObject.transform.position += Vector3.left * _speed * Time.deltaTime;

        _divideTime += Time.deltaTime;
        if (_divideTime > _destroyTime)
        {
            GenCircleBullet();
        }

        base.Update();
    }

    public void Initialize(PlayerCollider playerCollider, CirclePool circlePool)
    {
        base.Initialize(playerCollider);
        _circlePool = circlePool;
    }
}
