using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : CreatureController2D
{
    [SerializeField] private LayerMask goalLayerMask;
    
    public delegate void SelectDirectionDelegate(Direction direction);
    public delegate void ExecuteMoveDelegate(MoveQuality moveQuality);
    public event SelectDirectionDelegate OnSelectDirection;
    public event ExecuteMoveDelegate OnExecuteMove;

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
        var goal = Physics2D.Linecast(transform.position, transform.position, goalLayerMask);
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
        if (selectedDirection == Direction.None)
        {
            return;
        }

        var movementQuality = GetMovementQuality();
        OnExecuteMove?.Invoke(movementQuality);

        if (movementQuality != MoveQuality.Ko)
        {
            base.ExecuteMove();
        }
    }
}
