using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BulletMotherCircle : BulletBase
{
    [Header("BulletParams")]
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _destroyTime = 4f;

    [SerializeField]
    private float _genBulletRadius = 1f;
    [SerializeField]
    private int _genBulletNum = 8;

    [SerializeField]
    private GameObject _circleBulletPrefab;
    private float _divideTime = 0f;

    public override void OnEnable()
    {
        base.OnEnable();
        base.lifeTime = _destroyTime;
        _divideTime = 0f;
    }

    private void GenCircleBullet()
    {
        if (_circleBulletPrefab == null) return;

        float angle = 360 / (float)_genBulletNum;

        for (int i = 0; i < _genBulletNum; i++)
        {
            float currentAngle = angle * i;
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3();
            pos.y = _genBulletRadius * Mathf.Sin(rad);
            pos.x = _genBulletRadius * Mathf.Cos(rad);

            GameObject obj = Instantiate(_circleBulletPrefab, this.gameObject.transform.position, Quaternion.identity);

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

    public void Initialize(PlayerCollider playerCollider, GameObject circleBulletPrefab = null)
    {
        base.Initialize(playerCollider);
        if (circleBulletPrefab != null)
        {
            _circleBulletPrefab = circleBulletPrefab;
        }
    }
}
