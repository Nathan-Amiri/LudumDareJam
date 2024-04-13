using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    // PREFAB REFERENCE:
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D col;

        // 0 = standard, 1 = explode, 2 = sniper, 3 = spawner, 4 = shieldbearer
    [SerializeField] private List<Zombie> zombies = new();

    // SCENE REFERENCE:
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform zombieParent;

    // CONSTANT:
    private readonly float moveSpeed = 3;

    private readonly float teleportDuration = .3f;

    private readonly List<int> corpseQueue = new();

    // DYNAMIC:
    private Vector2 mousePosition;

    private Vector2 moveInput;

    private bool isStunned;

    private Zombie zombieToActivate;

    private Vector2 teleportDestination;
    private Coroutine teleportRoutine;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
            corpseQueue.Add(0);
    }

    public void PhaseStart()
    {
        if (teleportRoutine != null)
        {
            StopCoroutine(teleportRoutine);
            col.enabled = true;
        }

        isStunned = false;
    }

    private void Update()
    {
        Vector3 tempMousePosition = Input.mousePosition;
        tempMousePosition.z = -mainCamera.transform.position.z;
        mousePosition = mainCamera.ScreenToWorldPoint(tempMousePosition);

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (zombieToActivate != null)
        {
            Vector2Int newFaceDirection = Vector2Int.RoundToInt(mousePosition - (Vector2)zombieToActivate.transform.position);
            zombieToActivate.ChangeFaceDirection(newFaceDirection);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (zombieToActivate == null)
                Summon();
            else
                ActivateZombie();
        }

        if (Input.GetMouseButtonDown(1))
            Teleport();
    }

    private void FixedUpdate()
    {
        if (isStunned)
            return;

        Move();
    }

    private void Move() // Run in Fixed Update
    {
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

        zombieToActivate = Instantiate(zombies[corpseQueue[0]], mousePosition, Quaternion.identity, zombieParent);

        corpseQueue.RemoveAt(0);
    }
    private void ActivateZombie()
    {
        zombieToActivate.OnActivate();

        zombieToActivate = null;
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