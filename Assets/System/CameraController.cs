using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IPointerDownHandler
{
    private Vector3 _direction;

    public void OnMove(InputAction.CallbackContext context)
    {
        float moveSpeed = 5f;
        if (context.performed)
        {
            var dir = context.ReadValue<Vector2>() * moveSpeed;
            _direction = new Vector3(dir.x, dir.y, 0);
        }
        else if (context.canceled)
        {
            _direction = Vector3.zero;
        }

    }

    void Start()
    {
        _direction = Vector2.zero;
        addPhysicsRaycaster();
    }
    void Update()
    {
        if (_direction != Vector3.zero)
        {
            Camera.main.transform.position += (_direction * Time.deltaTime);
        }
    }
    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }
}

