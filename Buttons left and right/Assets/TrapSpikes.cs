using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameObject spikesOff;
    [SerializeField] private GameObject spikesOn;
    [SerializeField] private PlayerController2D player;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private bool startingValue;
    private bool currentValue;
    private bool shouldToggle;
    
    private void OnEnable()
    {
        musicManager.OnBeat += ToggleSpikes;
    }

    private void OnDisable()
    {
        musicManager.OnBeat -= ToggleSpikes;
    }

    private void Start()
    {
        currentValue = startingValue;
        SetSpikes();
    }

    private void SetSpikes()
    {
        spikesOff.SetActive(!currentValue);
        spikesOn.SetActive(currentValue);
    }

    private void ToggleSpikes()
    {
        if (!shouldToggle)
        {
            shouldToggle = true;
        }
        else
        {
            shouldToggle = false;
            currentValue = !currentValue;
            SetSpikes();
        }
        
        if (currentValue)
        {
            var position = transform.position;
            var playerHit = Physics2D.Linecast(position, position, playerLayerMask);
            if (playerHit)
            {
                player.Die();
            }
        }
    }
}
