using UniRx;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCollider _playerCollider;
    [SerializeField]
    private float _maxHp = 10f;
    public FloatReactiveProperty playerHp = new FloatReactiveProperty(10f);
    public float maxHp => _maxHp;

    private void ApplyDamage(float damage)
    {
        playerHp.Value -= damage;
    }

    public void Restart()
    {
        _playerCollider.OnDamageEvent += ApplyDamage;
        playerHp.Value = maxHp;
    }

}
