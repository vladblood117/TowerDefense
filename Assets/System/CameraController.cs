using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IPointerDownHandler
{
    private Vector3 _direction;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _minZoom;

    public void OnMove(InputAction.CallbackContext context)
    {
        float moveSpeed = 5f;
        var dir = context.ReadValue<Vector2>() * moveSpeed;

        if (context.performed)
        {

            _direction = new Vector3(dir.x, dir.y, 0);
        }
        else if (context.canceled)
        {
            _direction = Vector3.zero;
        }

    }

    public void OnZoom(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            Debug.Log("On Zoom Called");
            var dir = context.ReadValue<Vector2>();
            if (dir.y > 0f)
            {
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - .5f, _minZoom);
            }
            else if (dir.y < 0f)
            {
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + .5f, _maxZoom);
            }



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
            var size = Camera.main.orthographicSize;
            Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x + (_direction.x * Time.deltaTime), (-_maxZoom + size), (_maxZoom - size)),
            Mathf.Clamp(Camera.main.transform.position.y + (_direction.y * Time.deltaTime), (-_maxZoom + size), (_maxZoom - size)),
             -10);
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

