using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public PlayerData Data { get; private set; }
    public PlayerController Controller {  get; private set; }
    public StateMachine StateMachine { get; private set; }  

    private void Awake()
    {
        Controller = GetComponent<PlayerController>();
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
