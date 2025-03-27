using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerData Data { get; private set; }
    public PlayerState PlayerState { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public bool isGrounded { get; set; }
    public bool isInvincible { get; set; }


    private void Awake()
    {
        PlayerState = new PlayerState(Data);

        Controller = GetComponent<PlayerController>();
        Rigidbody = GetComponent<Rigidbody2D>();
        StateMachine = new PlayerStateMachine(this);
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
    }
}
