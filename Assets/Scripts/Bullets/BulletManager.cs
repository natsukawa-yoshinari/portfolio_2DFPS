using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCollider _playerCollider;
    [SerializeField]
    private GameObject _triangleBulletPrefab;
    [SerializeField]
    private GameObject _motherCircleBulletPrefab;
    [SerializeField]
    private GameObject _circleBulletPrefab;
    [SerializeField]
    private GameObject _starBulletPrefab;

    private GameObject _enemy;

    public void SetEnemyObject(GameObject enemy)
    {
        _enemy = enemy;
    }

    public void GenerateBulletTriangle()
    {
        if (_enemy == null || _triangleBulletPrefab == null) return;

        GameObject obj = Instantiate(_triangleBulletPrefab, _enemy.transform.position, Quaternion.identity);

        BulletTriangle bullet = obj.GetComponent<BulletTriangle>();
        if (bullet != null)
        {
            bullet.Initialize(_playerCollider);
        }
    }

    public void GenerateBulletMotherCircle()
    {
        if (_enemy == null || _motherCircleBulletPrefab == null) return;

        GameObject obj = Instantiate(_motherCircleBulletPrefab, _enemy.transform.position, Quaternion.identity);

        BulletMotherCircle bullet = obj.GetComponent<BulletMotherCircle>();
        if (bullet != null)
        {
            bullet.Initialize(_playerCollider, _circleBulletPrefab);
        }
    }

    public void GenerateBulletStar()
    {
        if (_enemy == null || _starBulletPrefab == null) return;

        GameObject obj = Instantiate(_starBulletPrefab, _enemy.transform.position, Quaternion.identity);

        BulletStar bullet = obj.GetComponent<BulletStar>();
        if (bullet != null)
        {
            bullet.Initialize(_playerCollider);
        }
    }

    public void ClearAllPool()
    {
        BulletBase[] activeBullets = FindObjectsByType<BulletBase>(FindObjectsSortMode.None);
        for (int i = 0; i < activeBullets.Length; i++)
        {
            if (activeBullets[i] != null)
            {
                Destroy(activeBullets[i].gameObject);
            }
        }
    }
}
