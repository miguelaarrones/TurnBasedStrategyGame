using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public event EventHandler OnDoorOpened;

    private const string DOOR_IS_OPEN = "IsOpen";

    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);

        if (isOpen)
        {
            OpenDoor();
        } else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractionComplete = onInteractComplete;

        isActive = true;
        timer = .5f;

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool(DOOR_IS_OPEN, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);

        OnDoorOpened?.Invoke(this, EventArgs.Empty);
    }

    private void CloseDoor() 
    {
        isOpen = false;
        animator.SetBool(DOOR_IS_OPEN, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
