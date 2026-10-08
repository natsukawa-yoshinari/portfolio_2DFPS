using UnityEngine;

public class AllTimeRotation : MonoBehaviour
{
    [SerializeField]
    private float _rotSpeed;

    void Update()
    {
        if (GameManager.IsGameEnd) return;

        this.gameObject.transform.Rotate(0f, 0f, _rotSpeed);
    }
}
