using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseNPC : MonoBehaviour, INPC
{
    [Header("기본 NPC 설정")]
    [SerializeField] protected string npcName;
    [SerializeField] protected Sprite portrait;
    [SerializeField] protected bool interactable = true;
    
    [Header("상호작용 설정")]
    [SerializeField] protected float interactionRadius = 2f;
    [SerializeField] protected GameObject interactionPrompt;
    [SerializeField] protected KeyCode interactionKey = KeyCode.W;
    
    protected bool playerInRange = false;
    protected PlayerController nearbyPlayer;
    
    public string NPCName => npcName;
    public Sprite NPCPortrait => portrait;
    public bool IsInteractable => interactable;
    
    protected virtual void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    protected virtual void Update()
    {
        // 플레이어가 범위 내에 있고 상호작용 가능하면
        if (playerInRange && interactable)
        {
            // 상호작용 키를 눌렀을 때
            if (Input.GetKeyDown(interactionKey))
            {
                Interact(nearbyPlayer);
            }
        }
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 접근했을 때
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            nearbyPlayer = other.GetComponent<PlayerController>();
            
            // 상호작용 프롬프트 표시
            if (interactionPrompt != null && interactable)
                interactionPrompt.SetActive(true);
        }
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 범위를 벗어났을 때
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            nearbyPlayer = null;
            
            // 상호작용 프롬프트 숨기기
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
    
    public abstract void Interact(PlayerController player);
    
    public virtual void UpdateState(GameManager.GameState gameState)
    {
        // 게임 상태에 따른 NPC 동작 변경
        interactable = (gameState == GameManager.GameState.Playing);
    }
}