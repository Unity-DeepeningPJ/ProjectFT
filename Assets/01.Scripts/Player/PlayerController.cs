using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput { get; private set; }
    public PlayerInput.PlayerActions playerActions { get; private set; }

    public bool canMove = true;

    [SerializeField] GameObject inventory;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;

        //playerActions.Inventory.performed += OnInventoryPressed;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerActions.Inventory.started += OnInventoryPressed;
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void OnInventoryPressed(InputAction.CallbackContext context)
    {
        inventory.SetActive(!inventory.activeInHierarchy);
    }
}
