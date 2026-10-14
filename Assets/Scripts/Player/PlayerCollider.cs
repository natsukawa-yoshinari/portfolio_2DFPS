using System;
using UniRx;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private Vector3 _offset;

    public Action<float> OnDamageEvent;
    public float radius => _radius;
    public Vector3 offset => _offset;

    private Vector3 _defaultPos;

    private void Start()
    {
        _defaultPos = this.gameObject.transform.position;
    }

    public Transform GetPlayerTransform()
    {
        return _player != null ? _player.transform : this.transform;
    }

    public void AddDamage(float damage)
    {
        OnDamageEvent?.Invoke(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector3 centerPos = (_player != null ? _player.transform.position : this.transform.position) + _offset;

        Gizmos.DrawWireSphere(centerPos, _radius);
    }

    public void ResetPlayer()
    {
        this.gameObject.transform.position = _defaultPos;

        OnDamageEvent = null;
    }
}
