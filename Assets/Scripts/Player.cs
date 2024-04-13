using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
        // 0 = standard, 1 = explode, 2 = sniper, 3 = spawner, 4 = shieldbearer
    [SerializeField] private List<Minion> minions = new();

    // SCENE REFERENCE:
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform zombieParent;

    // CONSTANT:
    [SerializeField] private float teleportDuration = .3f;

    private readonly List<int> corpseQueue = new();

    // DYNAMIC:
    private Vector2 mousePosition;

    private Vector2 moveInput;

    private bool isStunned;

    private Minion minionToActivate;

    private Vector2 teleportDestination;
    private Coroutine teleportRoutine;

    public void PhaseStart()
    {
        if (teleportRoutine != null)
        {
            StopCoroutine(teleportRoutine);
            col.enabled = true;
        }

        isStunned = false;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 tempMousePosition = Input.mousePosition;
        tempMousePosition.z = -mainCamera.transform.position.z;
        mousePosition = mainCamera.ScreenToWorldPoint(tempMousePosition);

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (minionToActivate != null)
        {
            Vector2 direction = mousePosition - (Vector2)minionToActivate.transform.position;
            if (direction != Vector2.zero)
                minionToActivate.ChangeFaceDirectionFromVector(direction.normalized);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (minionToActivate == null)
                Summon();
            else
                ActivateZombie();
        }

        if (isStunned)
            return;

        if (Input.GetMouseButtonDown(1))
            Teleport();
    }

    protected override void FixedUpdate()
    {
        // No base

        if (isStunned)
            return;

        Move();
    }

    private void Move() // Run in Fixed Update
    {
        if (moveInput != Vector2.zero)
            ChangeFaceDirectionFromVector(moveInput.normalized);

        // Snappy horizontal movement:
        // (This movement method will prevent the player from slowing completely in a frictionless environment. To prevent this,
        // ensure that either at least a tiny bit of friction is present or the player's velocity is rounded to 0 when low enough)
        // (In this project, the player is frictionless, but a small amount of linear drag has been added)
        Vector2 desiredVelocity = moveInput * moveSpeed;
        Vector2 velocityChange = desiredVelocity - rb.velocity;
        Vector2 acceleration = velocityChange / .05f;
        Vector2 force = rb.mass * acceleration;
        rb.AddForce(force);
    }

    private void Summon()
    {
        if (corpseQueue.Count == 0)
            return;

        minionToActivate = Instantiate(minions[corpseQueue[0]], mousePosition, Quaternion.identity, zombieParent);

        corpseQueue.RemoveAt(0);
    }
    private void ActivateZombie()
    {
        minionToActivate.OnActivate();

        minionToActivate = null;
    }

    public void Teleport()
    {
        col.enabled = false;
        isStunned = true;

        teleportDestination = mousePosition;
        teleportRoutine = StartCoroutine(TeleportRoutine(teleportDuration));

        float teleportSpeed = Vector2.Distance(teleportDestination, transform.position) / teleportDuration;
        rb.velocity = teleportSpeed * ((Vector3)teleportDestination - transform.position).normalized;
    }
    private IEnumerator TeleportRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        rb.velocity = Vector3.zero;
        transform.position = (Vector3)teleportDestination;

        isStunned = false;

        col.enabled = true;
    }
}