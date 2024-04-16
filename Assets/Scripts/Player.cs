using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    // ENEMY/CORPSE/MINION TYPE NUMBERS CONSISTENT ACROSS SCRIPTS:
    // 0 = Grunt, 1 = Mage, 2 = Cart, 3 = Shieldbearer

    // STATIC:
    public static Vector2 mousePosition;

    // PREFAB REFERENCE:
    [SerializeField] protected CircleCollider2D col;

    [SerializeField] private List<Minion> minions = new();
    [SerializeField] private List<Sprite> minionSprites = new();

    // SCENE REFERENCE:
        // Read by MageMinion, ShieldbearerMinion
    public AudioManager audioManager;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform zombieParent;

    [SerializeField] private GameObject summonQueue;
    [SerializeField] private Image nextSummonImage;

    private readonly List<int> corpseQueue = new();

    // DYNAMIC:
    private Vector2 moveInput;

    private bool isStunned;

    private Minion minionToActivate;

    public void GameStartEnd(bool start)
    {
        isStunned = !start;

        rb.velocity = Vector2.zero;
    }

    protected override void Update()
    {
        base.Update();

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

        if (isStunned)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (minionToActivate == null)
                Summon();
            else
                ActivateMinion();
        }

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

        StartCoroutine(audioManager.PlayClip(4));

        minionToActivate = Instantiate(minions[corpseQueue[0]], mousePosition, Quaternion.identity, zombieParent);
        minionToActivate.OnSpawn(this);

        corpseQueue.RemoveAt(0);
    }
    private void ActivateMinion()
    {
        minionToActivate.OnActivate();

        minionToActivate = null;
    }

    public void SummonMinionFromCart(Vector2 position, Vector2 minionFaceDirection)
    {
        Minion minion = Instantiate(minions[0], position, Quaternion.identity, zombieParent);

        minion.ChangeFaceDirectionFromVector(minionFaceDirection);

        minion.OnSpawn(this);
        minion.OnActivate();
    }

    public void AddCorpse(int corpseType)
    {
        corpseQueue.Add(corpseType);
    }

    public static Vector2 GetPositivePerpendicular(Vector2 vector)
    {
        vector = Vector2.Perpendicular(vector);
        return new(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }
}