using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private float _moveSpeed = 2f;

    private void Update()
    {
        if (GameManager.IsGameEnd) return;

        Vector3 input = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            input.y += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            input.y -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            input.x -= 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            input.x += 1;
        }

        input = input.normalized;

        Vector3 moveDirection = new Vector3(input.x, input.y, 0);

        _player.transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        CheckCanMove();
    }

    private void CheckCanMove()
    {
        float cameraHeight = _mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * _mainCamera.aspect;

        float maxX = (cameraWidth - 0.2f);
        float minX = (cameraWidth - 0.2f) * -1f;

        float maxY = (cameraHeight - 0.2f);
        float minY = (cameraHeight - 0.2f) * -1f;

        Vector3 pos = _player.transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        _player.transform.position = pos;
    }
}
