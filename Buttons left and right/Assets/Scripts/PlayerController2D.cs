using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : CreatureController2D
{
    [SerializeField] private LayerMask goalLayerMask;

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
            TryMove();
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
            TryMove();
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
}
