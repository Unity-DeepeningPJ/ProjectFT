using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseNPC : MonoBehaviour, INPC
{
    [Header("기본 NPC 설정")]
    [SerializeField] protected string npcName; // NPC 이름
    [SerializeField] protected Sprite portrait; // NPC 초상화
    [SerializeField] protected bool interactable = true; // 상호작용 가능 여부
    
    [Header("상호작용 설정")]
    [SerializeField] protected float interactionRadius = 2f; // 상호작용 범위
    [SerializeField] protected GameObject interactionPrompt; // 상호작용 프롬프트
    [SerializeField] protected KeyCode interactionKey = KeyCode.W;
    [SerializeField] protected KeyCode cancelKey = KeyCode.S;
    
    protected bool playerInRange = false; 
    protected PlayerController nearbyPlayer; // 근처 플레이어
    
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
            //Debug.Log($"{npcName}: 플레이어가 범위 내에 있음, 상호작용 키 대기 중");
            
            // 상호작용 키를 눌렀을 때
            if (Input.GetKeyDown(interactionKey))
            {
                Debug.Log($"{npcName}: 상호작용 키 감지됨! 상호작용 시작");
                Interact(nearbyPlayer);
            }
        }

        // 상호작용 중 취소키를 눌렀을 때
        if (Input.GetKeyDown(cancelKey))
        {
            Debug.Log($"{npcName}: 상호작용 취소키 감지됨! 상호작용 취소");
            // 추후 UI매니저를 통해 추가할 예정
        }
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 접근했을 때
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{npcName}: 플레이어 감지됨!");
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

        // 만약 플레이어가 범위 내에 있고 상호작용 중이라면
        // 상호작용하여 열려있던 UI를 닫도록 함 추후 UI매니저를 통해 추가할 예정

    }
    
    public abstract void Interact(PlayerController player);
    
    public virtual void UpdateState(GameManager.GameState gameState)
    {
        // 게임 상태에 따른 NPC 동작 변경
        interactable = (gameState == GameManager.GameState.Playing);
    }
}