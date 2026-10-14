using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField]
    private float _radius;
    [SerializeField]
    private Vector3 _offset;

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

    public virtual void Update()
    {
        _currentTime += Time.deltaTime;
        if (_lifeTime < _currentTime)
        {
            _currentTime = 0f;
            Destroy(gameObject);
            return;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_playerCollider != null)
        {
            if (collision.transform == _playerCollider.GetPlayerTransform() || collision.gameObject == _playerCollider.gameObject)
            {
                OnHitPlayer(_playerCollider);
                _currentTime = 0f;
                Destroy(gameObject);
                return;
            }
        }

        PlayerCollider hitPlayer = collision.GetComponent<PlayerCollider>();
        if (hitPlayer == null) hitPlayer = collision.GetComponentInParent<PlayerCollider>();
        if (hitPlayer != null)
        {
            OnHitPlayer(hitPlayer);
            _currentTime = 0f;
            Destroy(gameObject);
        }
    }

    public virtual void OnHitPlayer(PlayerCollider playerCollider)
    {
        if (playerCollider == null) return;
        playerCollider.AddDamage(_damage);
    }

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
