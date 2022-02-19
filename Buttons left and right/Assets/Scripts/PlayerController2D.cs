using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : CreatureController2D
{
    private const string DIE_ANIMATION_PARAMETER = "Die";
    private static readonly int DieAnimationParameter = Animator.StringToHash(DIE_ANIMATION_PARAMETER);
    [SerializeField] private LayerMask goalLayerMask;
    [SerializeField] private float greatThreshold;
    [SerializeField] private float okThreshold;
    private bool acceptInput;
    
    public delegate void SelectDirectionDelegate(Direction direction);
    public event SelectDirectionDelegate OnSelectDirection;
    public delegate void ExecuteMoveDelegate(MoveQuality moveQuality);
    public event ExecuteMoveDelegate OnExecuteMove;

    protected override void Awake()
    {
        base.Awake();
        acceptInput = true;
    }
    
    protected override void HandleUpdate()
    {
#if UNITY_ANDROID || UNITY_IOS
        HandleInputMobile();
#else
        HandleInput();
#endif
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!acceptInput)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ExecuteMove();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetSelectedDirection(GetNextAvailableDirection());
        }
    }

    private void HandleInputMobile()
    {
        if (Input.touches.Any(t => t.phase == TouchPhase.Began && t.position.x < Screen.width / 2f))
        {
            ExecuteMove();
        }
        else if (Input.touches.Any(t => t.phase == TouchPhase.Began && t.position.x >= Screen.width / 2f))
        {
            SetSelectedDirection(GetNextAvailableDirection());
        }
    }

    protected override IEnumerator Move(Vector3 targetMovement)
    {
        yield return StartCoroutine(base.Move(targetMovement));
        var position = transform.position;
        var goal = Physics2D.Linecast(position, position, goalLayerMask);
        if (goal)
        {
            SceneManager.LoadScene(1);
        }
    }

    protected override void SetSelectedDirection(Direction direction)
    {
        base.SetSelectedDirection(direction);
        OnSelectDirection?.Invoke(selectedDirection);
    }

    protected override void ExecuteMove()
    {
        var movementQuality = GetMovementQuality();
        OnExecuteMove?.Invoke(movementQuality);

        if (movementQuality != MoveQuality.Ko)
        {
            base.ExecuteMove();
        }
    }

    private MoveQuality GetMovementQuality()
    {
        var distance = musicManager.GetDistanceToClosestBeatNormalized();
        if (distance < greatThreshold)
        {
            return MoveQuality.Ace;
        }
        return distance < okThreshold ? MoveQuality.Ok : MoveQuality.Ko;
    }

    public void Die()
    {
        acceptInput = false;
        animator.SetTrigger(DieAnimationParameter);
        musicManager.StopTrackingBeats();
        SetSelectedDirection(Direction.None);
        Invoke(nameof(RestartLevel), 3f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
