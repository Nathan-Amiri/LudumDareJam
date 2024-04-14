using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    // ENEMY/CORPSE/MINION TYPE NUMBERS CONSISTENT ACROSS SCRIPTS:
    // 0 = Grunt, 1 = Mage, 2 = Priest, 3 = Cart, 4 = Shieldbearer

    // STATIC:
        // Set by Corpse
    public static Transform corpseMouseOver;

    // PREFAB REFERENCE:
    [SerializeField] private List<Minion> minions = new();
    [SerializeField] private List<Sprite> minionSprites = new();

    // SCENE REFERENCE:
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform zombieParent;

    [SerializeField] private GameObject summonQueue;
    [SerializeField] private Image nextSummonImage;

    // CONSTANT:
    [SerializeField] private float teleportDuration;

    private readonly List<int> corpseQueue = new();

    // DYNAMIC:
    private Vector2 mousePosition;

    private Vector2 moveInput;

    private bool isStunned;

    private Minion minionToActivate;

    private Vector2 teleportDestination;
    private Coroutine teleportRoutine;

    public void GameStartEnd(bool start)
    {
        if (!start && teleportRoutine != null)
        {
            StopCoroutine(teleportRoutine);
            col.enabled = true;
        }

        isStunned = !start;
        if (!start)
            rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (corpseQueue.Count > 0)
            nextSummonImage.sprite = minionSprites[corpseQueue[0]];
        if (corpseQueue.Count > 0 && !summonQueue.activeSelf)

            summonQueue.SetActive(true);
        else if (corpseQueue.Count == 0 && summonQueue.activeSelf)
            summonQueue.SetActive(false);

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

    protected void FixedUpdate()
    {
        if (isStunned)
            return;

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
        if (corpseMouseOver == null)
            return;

        col.enabled = false;
        isStunned = true;

        teleportDestination = corpseMouseOver.transform.position;
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

    public override void AuraTrigger(Collider2D col) // Called by Aura
    {
        base.AuraTrigger(col);

        if (col.CompareTag("Corpse"))
        {
            Corpse corpse = col.GetComponent<Corpse>();

            corpseQueue.Add(corpse.corpseType);

            Destroy(corpse.gameObject);
        }
    }
}