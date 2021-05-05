using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StructureController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _open = false;
    private PlayerHandler _player;
    public InputActionAsset actionAsset;
    private InputActionMap gameplayMap;

    private InputAction clickAction;
    private Structures _structure;
    private bool _hovered = false;
    private InputAction.CallbackContext clickMethod;
    private bool _opened;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        gameplayMap = actionAsset.FindActionMap("Structures");
        _structure = gameObject.GetComponent<Structures>();
        gameplayMap.Enable();
        _opened = false;
        clickAction = actionAsset.FindAction("GuiControls");
        clickAction.performed += ctx => OnClick(ctx);

    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (_hovered)
        {
            if (_opened)
            {
                StructureGui.CloseActiveStructure();
                _opened = false;
            }
            else
            {
                _opened = true;
                _structure.GetComponent<SpriteRenderer>().material = TDMaterials.outline;
                StructureGui.OpenStructure(this);
            }



        }
    }

    public void OnGuiClose()
    {
        _opened = false;
        if (_structure.DefaultMaterial == null)
        {
            Debug.Log("default_material is null");
        }
        _structure.GetComponent<SpriteRenderer>().material = _structure.DefaultMaterial;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_structure.IsPlaced)
        {
            _hovered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_structure.IsPlaced)
        {
            _hovered = false;
        }
    }

    public Structures GetStructure() { return _structure; }
}
