using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var obstacles = Physics2D.Linecast(transform.position, transform.position + Vector3.right, obstaclesLayerMask);
            if (!obstacles)
            {
                transform.Translate(Vector3.right);    
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down);
        }
    }
}
