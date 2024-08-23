using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public float moveDirectionX;
    public float moveDirectionY;
    public float attackDirectionY;
    public float walkSpeedX;
    public float flySpeedX;
    public float flySpeedY;
    public float dashSpeedX;
    public float jumpForce;
    public float fallSpeedMultiplier;
    public float lowJumpMultiplier;
    public float minJumpVelocity;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    public float dashCooldownTime;
    private float nextDashTime = 0;
    public float attackRate = 2f;
    private float nextAttackTime = 0;
    public float attackRangeLR;
    public float attackRangeUP;
    public float attackRangeDOWN;
    public float knockbackForce;
    public float knockdownForce;
    public float knockupForce;
    private float touchingLeftOrRightWall;
    public float wallSlideSpeed;
    public float countdown = 60;
    public float maxHealth = 4;
    public float currentHealth = 4;
    private float timeBtwShots;
    public float startTimeBtwShots;

    public bool canMove;
    public bool canJump;
    public bool canDash;
    public bool dashing;
    public bool canAttack;
    public bool attacking;
    public bool wallJump;
    public bool isWallSliding;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;
    public bool isTouchingSemiSolid;
    public bool isGrounded;
    public bool isDoubleJumping;
    public bool facingRight;
    public bool isFalling;
    public bool isAttacking;
    public bool canTakeDamage;
    public bool isDead;
    public bool phoenixForm;
    public bool travellerForm;
    public bool mushroomBounce;
    public bool inTrigger;
    public bool transformed;

    private int extraJumps = 0;
    public int extraJumpsValue;
    public int attackDamage;
    public int spawnLocation = 0;
    public int enemiesDefeated = 0;
    public int numOfHearts;

    public Rigidbody2D rb;
    public Animator anim; 
    public Transform attackPointLR;
    public Transform attackPointUP;
    public Transform attackPointDOWN;
    public Transform firePoint;
    public LayerMask whatIsGround;
    public LayerMask enemyLayers;
    public LayerMask mushroomLayers;
    public LayerMask projectileLayers;
    public LayerMask bossLayers;
    public LayerMask iceLayers;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject boss;
    public GameObject healUI;
    public GameObject resurrectUI;
    public GameObject healthBarUI;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject projectile;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public HealthBar healthBar;

    AudioSource audioSource;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);

        currentHealth = PlayerPrefs.GetFloat("currenthealth");

        phoenixForm = (PlayerPrefs.GetInt("phoenixvalue") != 0);

        isDead = (PlayerPrefs.GetInt("isdeadvalue") != 0);

        spawnLocation = (PlayerPrefs.GetInt("spawnlocationvalue"));

        enemiesDefeated = (PlayerPrefs.GetInt("enemiesdefeatedvalue"));

        if (spawnLocation == 0)
        {
            facingRight = true;
        }
        if (spawnLocation == 1)
        {
            Flip();
            facingRight = false;
        }

        canMove = true;

        canTakeDamage = true;
        
        if(phoenixForm == false)
        {
            travellerForm = true;
        }
        else if(phoenixForm == true)
        {
            anim.SetBool("IsPhoenix", true);
            anim.SetBool("IsDead", true);
        }

        if(travellerForm == true)
        {
            canJump = true;

            canDash = true;

            canAttack = true;
        }

        extraJumps = extraJumpsValue;

        dashTime = startDashTime;

        rb = GetComponent<Rigidbody2D>();

        audioSource = gameObject.GetComponent<AudioSource>();

        transformed = false;

        if(phoenixForm == true)
        {
            healthBarUI.SetActive(true);
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
        }
        if(travellerForm == true)
        {
            healthBarUI.SetActive(false);
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(true);
        }

        timeBtwShots = startTimeBtwShots;
    }
    void Update()
    {
        DoubleJump();

        DashCooldown();

        CheckDirection();

        CheckIfWallJumped();

        CheckIfTouchingWall();

        CheckIfWallSliding();

        Attack();

        AttackUpwards();

        AttackDownwards();

        PlayerPrefs.SetFloat("currenthealth", currentHealth);
        
        PlayerPrefs.SetInt("enemiesdefeatedvalue", enemiesDefeated);

        PlayerPrefs.SetInt("phoenixvalue", (phoenixForm ? 1 : 0));

        PlayerPrefs.SetInt("isdeadvalue", (isDead ? 1 : 0));

        SetCurrentHealthToMaxHealth();

        DamageOverTime();

        GameOver();

        HealthUI();

        Resurrect();

        Shoot();

        if (currentHealth == maxHealth && travellerForm == true)
        {
            healUI.SetActive(false);
            resurrectUI.SetActive(false);
        }

        healthBar.SetHealth(currentHealth);

    }
    void FixedUpdate()
    {
        Move();

        WallJump();

        FallSpeed();

        WallSlideFriction();

        CheckIfGrounded();

        CheckIfFalling();

    }
    //allows horizontal movement
    void Move()
    {
        if (canMove == true && isDead == false && travellerForm == true) 
        {
            moveDirectionX = Input.GetAxis("Horizontal");
            attackDirectionY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(moveDirectionX * walkSpeedX, rb.velocity.y);

            if (canMove == true && isGrounded == true)
            {
                anim.SetFloat("Speed", Mathf.Abs(moveDirectionX));   
            }
        }

        if (canMove == true && isDead == true && phoenixForm == true)
        {
            moveDirectionX = Input.GetAxis("Horizontal");
            moveDirectionY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(moveDirectionY * flySpeedX, rb.velocity.y);
            rb.velocity = new Vector2(moveDirectionX * flySpeedY, rb.velocity.x);

            if (canMove == true)
            {
                anim.SetFloat("Speed", Mathf.Abs(moveDirectionX));
            }
        }

        if (dashing == true && facingRight && isDead == false && travellerForm == true)
        {
            moveDirectionX = 1f;
            rb.velocity = new Vector2(moveDirectionX * dashSpeedX, rb.velocity.y);
        }

        else if (dashing == true && !facingRight && isDead == false && travellerForm == true)
        {
            moveDirectionX = -1f;
            rb.velocity = new Vector2(moveDirectionX * dashSpeedX, rb.velocity.y);
        }
    }
    //flips sprite based on direction
    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate (0f, 180f, 0f);

    }
    //checks if wallsliding
    void CheckIfWallSliding()
    {
        if (isTouchingLeftWall | isTouchingRightWall && !isGrounded && rb.velocity.y < 0 && isDead == false)
        {
            isWallSliding = true;
            FindObjectOfType<AudioManager>().Play("Wallslide");
            anim.SetBool("IsWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("IsWallSliding", false);
        }
    }
    //double jump
    void DoubleJump()
    {
        if (isGrounded == true && isDead == false)
        {
            extraJumps = extraJumpsValue;
        }
        if (canJump == true)
        {
            if (Input.GetButtonDown("Jump") && extraJumps > 0 && isDead == false)
            {
                anim.SetBool("Jumped", true);
                rb.velocity = Vector2.up * jumpForce;
                Invoke("SetJumpToFalse", 0.05f);
                extraJumps--;
                canMove = true;
                Debug.Log("Double Jump enabled");
                
                if (Input.GetButtonDown("Jump") && !isGrounded && !isWallSliding && isDead == false)
                {
                    anim.SetBool("Jumped", true);
                    isDoubleJumping = true;
                    Invoke("SetDoubleJumpToFalse", 0.05f);
                    Debug.Log("Double Jump Used");
                }
            }
            else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true && isDead == false)
            {
                anim.SetBool("Jumped", true);
                rb.velocity = Vector2.up * jumpForce;
                FindObjectOfType<AudioManager>().Play("Jump");
                Invoke("SetJumpToFalse", 0.05f);
                Debug.Log("Double Jump disabled");
            }
        }
    }

    void SetDoubleJumpToFalse()
    {
        anim.SetBool("Jumped", false);
        isDoubleJumping = false;
        anim.SetBool("IsDoubleJumping", false);
    }
    //allows control over fall, low jump and rise multipliers for better jumping
    void FallSpeed()
    {
        if (rb.velocity.y < minJumpVelocity)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeedMultiplier - 1) * Time.deltaTime;
            anim.SetBool("Jumped", false);
            anim.SetBool("IsFalling", true);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeedMultiplier - 1) * Time.deltaTime;
        }
    }
    //defines wallJump
    void CheckIfWallJumped()
    {
        if (Input.GetButtonDown("Jump") && (isTouchingLeftWall || isTouchingRightWall) && !isGrounded)
        {
            wallJump = true;
            Invoke("SetWallJumpToFalse", 0.08f);
        }
    }
    //reduces velocity when wallsliding
    void WallSlideFriction()
    {
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }
    //defines isGrounded
    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(0.1f, 0.2f), 0f, whatIsGround);
    }
    //defines isTouchingWall
    void CheckIfTouchingWall()
    {
        isTouchingLeftWall = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.2f, gameObject.transform.position.y),
        new Vector2(0.2f, 0.2f), 0f, whatIsGround);

        isTouchingRightWall = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.2f, gameObject.transform.position.y),
        new Vector2(0.2f, 0.2f), 0f, whatIsGround);

        if (isTouchingLeftWall)
        {
            touchingLeftOrRightWall = 1;     
        }
        else if (isTouchingRightWall)
        {
            touchingLeftOrRightWall = -1;
        }
    }
    //wall jumping
    void WallJump()
    {
        if (wallJump == true)
        {
            extraJumps = extraJumpsValue;
            canMove = false;
            canJump = true;
        }
        else if (isTouchingLeftWall || isTouchingRightWall == true || isWallSliding == true || isGrounded == true)
        {
            canMove = true;
        }
        if (isGrounded)
        {
            canJump = true;
        }
        if (wallJump && travellerForm == true)
        {
            rb.velocity = new Vector2(walkSpeedX * touchingLeftOrRightWall, jumpForce);
            Invoke("SetWallJumpToFalse", 0.08f);
            Invoke("SetCanMoveToTrue", 0.25f);
        }
    }
    void SetWallJumpToFalse()
    {
        wallJump = false;
    }
    //dash

    void DashCooldown()
    {
        if (Time.time > nextDashTime)
        {
            if (dashTime <= 0)
            {
                dashTime = startDashTime;
            }
            else
            {
                dashTime -= Time.deltaTime;
                Dash();
            }
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Fire1r") && facingRight && canDash == true)
        {
            anim.SetBool("IsDashing", true);
            dashing = true;
            FindObjectOfType<AudioManager>().Play("Dash");
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            nextDashTime = Time.time + dashCooldownTime;
            Invoke("SetDashToFalse", 0.4f);
            Debug.Log("Dash ability used, cooldown started");
        }
        else if (Input.GetButtonDown("Fire1l") && !facingRight && canDash == true)
        {
            anim.SetBool("IsDashing", true);
            dashing = true;
            FindObjectOfType<AudioManager>().Play("Dash");
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            nextDashTime = Time.time + dashCooldownTime;
            Invoke("SetDashToFalse", 0.4f);
            Debug.Log("Dash ability used, cooldown started");
        }
    }

    void SetDashToFalse()
    {
        anim.SetBool("IsDashing", false);
        dashing = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }
    void CheckDirection()
    {
        if (facingRight == false && moveDirectionX > 0 && isTouchingLeftWall == false && isTouchingRightWall == false)
        {
            Flip();
            Debug.Log("facing right");
        }
        if (facingRight == true && moveDirectionX < 0 && isTouchingLeftWall == false && isTouchingRightWall == false)
        {
            Flip();
            Debug.Log("facing left");
        }
        if (isTouchingSemiSolid == true)
        {
            Debug.Log("noflip");
        }
        if (isTouchingLeftWall == true && facingRight == false && dashing == false && isGrounded == false)
        {
            Flip();
            Debug.Log("leftwallflip");
        }
        if (isTouchingRightWall == true && facingRight == true && dashing == false && isGrounded == false)
        {
            Flip();
            Debug.Log("leftrightflip");
        }
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {

            //Attack LR
            if (Input.GetButtonDown("Attack") && attackDirectionY == 0 && canAttack == true)
            {
                FindObjectOfType<AudioManager>().Play("AttackLR");

                isAttacking = true;

                anim.SetBool("IsAttacking", true);

                Invoke("SetIsAttackingToFalse", 0.3f);

                nextAttackTime = Time.time + 1f / attackRate;

                //Detect enemies in range of LR attack
                Collider2D[] hitEnemiesLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, enemyLayers);

                //Detect ground in range of LR attack
                Collider2D[] hitGroundLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, whatIsGround);

                Collider2D[] hitMushroomLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, mushroomLayers);

                Collider2D[] hitProjectileLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, projectileLayers);

                Collider2D[] hitIceLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, iceLayers);

                Collider2D[] hitBossLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, bossLayers);

                anim.SetTrigger("Attack");

                //LR Damage enemies
                foreach (Collider2D enemy in hitEnemiesLR)
                {
                    Invoke("DelayAttackEffectsLR", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                //LR ground detection
                foreach (Collider2D ground in hitGroundLR)
                {
                    Invoke("DelayAttackEffectsLR", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D mushroom in hitMushroomLR)
                {
                    Invoke("DelayAttackEffectsLR", 0.2f);
                }

                foreach (Collider2D projectile in hitProjectileLR)
                {
                    Invoke("DelayAttackEffectsLR", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D ice in hitIceLR)
                {
                    Invoke("DelayAttackEffectsLR", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D boss in hitBossLR)
                {
                    Invoke("DelayAttackEffectsLR", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

            }
        }
    }

    void AttackUpwards()
    {
        if (Time.time >= nextAttackTime)
        {
            //Attack Upwards
            if (Input.GetButtonDown("Attack") && attackDirectionY == 1 && canAttack == true)
            {
                FindObjectOfType<AudioManager>().Play("AttackUP");

                isAttacking = true;

                anim.SetBool("IsAttacking", true);

                Invoke("SetIsAttackingToFalse", 0.3f);

                nextAttackTime = Time.time + 1f / attackRate;

                //Detect enemies in range of UP attack
                Collider2D[] hitEnemiesUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, enemyLayers);

                //Detect ground in range of UP attack
                Collider2D[] hitGroundUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, whatIsGround);

                Collider2D[] hitMushroomUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, mushroomLayers);

                Collider2D[] hitProjectileUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, projectileLayers);

                Collider2D[] hitIceUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, iceLayers);

                Collider2D[] hitBossUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, bossLayers);

                anim.SetTrigger("AttackUp");

                foreach (Collider2D enemy in hitEnemiesUP)
                {
                    Invoke("DelayAttackEffectsUP", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D ground in hitGroundUP)
                {
                    Invoke("DelayAttackEffectsUP", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D mushroom in hitMushroomUP)
                {
                    Invoke("DelayAttackEffectsUP", 0.2f);
                }

                foreach (Collider2D projectile in hitProjectileUP)
                {
                    Invoke("DelayAttackEffectsUP", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D ice in hitIceUP)
                {
                    Invoke("DelayAttackEffectsUP", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D boss in hitBossUP)
                {
                    Invoke("DelayAttackEffectsUP", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }
            }
        }
    }

    void AttackDownwards()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack") && attackDirectionY == -1 && canAttack == true)
            {
                FindObjectOfType<AudioManager>().Play("AttackDOWN");

                isAttacking = true;

                anim.SetBool("IsAttacking", true);

                Invoke("SetIsAttackingToFalse", 0.3f);

                //Detect enemies in range of DOWN attack
                Collider2D[] hitEnemiesDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, enemyLayers);

                Collider2D[] hitProjectileDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, projectileLayers);

                Collider2D[] hitIceDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, iceLayers);

                Collider2D[] hitBossDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, bossLayers);

                anim.SetTrigger("AttackDown");

                foreach (Collider2D enemy in hitEnemiesDOWN)
                {
                    Invoke("DelayAttackEffectsDOWN", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D projectile in hitProjectileDOWN)
                {
                    Invoke("DelayAttackEffectsDOWN", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D ice in hitIceDOWN)
                {
                    Invoke("DelayAttackEffectsDOWN", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }

                foreach (Collider2D boss in hitBossDOWN)
                {
                    Invoke("DelayAttackEffectsDOWN", 0.2f);
                    FindObjectOfType<AudioManager>().Play("DamageEnemy");
                }
            }
        }
    }

    void DelayAttackEffectsLR()
    {
        //Detect enemies in range of LR attack
        Collider2D[] hitEnemiesLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, enemyLayers);

        //Detect ground in range of LR attack
        Collider2D[] hitGroundLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, whatIsGround);

        //Detect mushroom in range of LR attack
        Collider2D[] hitMushroomLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, mushroomLayers);

        Collider2D[] hitProjectileLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, projectileLayers);

        Collider2D[] hitIceLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, iceLayers);

        Collider2D[] hitBossLR = Physics2D.OverlapCircleAll(attackPointLR.position, attackRangeLR, bossLayers);

        foreach (Collider2D enemy in hitEnemiesLR)
        {
            //LR Damage enemies
            enemy.GetComponent<EnemyStats>().TakeDamage(attackDamage);

            //Knockback if facing right
            if (facingRight == true)
            {
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 2f);
                Debug.Log("Attack cooldown started");
            }

            //Knockback if facing left
            if (facingRight == false)
            {
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 2f);
                Debug.Log("Attack cooldown started");
            }
        }
        //LR ground detection
        foreach (Collider2D ground in hitGroundLR)
        {
            Debug.Log("We hit " + ground.name);

            //Knockback if facing right
            if (facingRight == true)
            {
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
            }

            //Knockback if facing left
            if (facingRight == false)
            {
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
            }
        }

        foreach (Collider2D mushroom in hitMushroomLR)
        {
            //Knockback if facing right
            if (facingRight == true)
            {
                FindObjectOfType<AudioManager>().Play("Boing");
                mushroomBounce = true;
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -2.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }

            //Knockback if facing left
            if (facingRight == false)
            {
                FindObjectOfType<AudioManager>().Play("Boing");
                mushroomBounce = true;
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 2.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }
        }

        foreach (Collider2D projectile in hitProjectileLR)
        {
            if(facingRight == true)
            {
                Destroy(projectile.gameObject);
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }
            if(facingRight == false)
            {
                Destroy(projectile.gameObject);
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }

        }

        foreach (Collider2D ice in hitIceLR)
        {
            if (facingRight == true)
            {
                Destroy(ice.gameObject);
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }
            if (facingRight == false)
            {
                Destroy(ice.gameObject);
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }
        }

        foreach (Collider2D boss in hitBossLR)
        {
            //LR Damage enemies
            boss.GetComponent<BossHealth>().TakeDamage(attackDamage);

            //Knockback if facing right
            if (facingRight == true)
            {
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 2f);
                Debug.Log("Attack cooldown started");
            }

            //Knockback if facing left
            if (facingRight == false)
            {
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 1.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 2f);
                Debug.Log("Attack cooldown started");
            }
        }
    }
    void DelayAttackEffectsUP()
    {
        //Detect enemies in range of UP attack
        Collider2D[] hitEnemiesUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, enemyLayers);

        //Detect ground in range of UP attack
        Collider2D[] hitGroundUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, whatIsGround);

        Collider2D[] hitMushroomUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, mushroomLayers);

        Collider2D[] hitProjectileUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, projectileLayers);

        Collider2D[] hitIceUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, iceLayers);

        Collider2D[] hitBossUP = Physics2D.OverlapCircleAll(attackPointUP.position, attackRangeUP, bossLayers);

        //Up damage enemy
        foreach (Collider2D enemy in hitEnemiesUP)
        {
            enemy.GetComponent<EnemyStats>().TakeDamage(attackDamage);

            //Knockback
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockdownForce * -1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        //Up ground detection
        foreach (Collider2D ground in hitGroundUP)
        {
            Debug.Log("We hit " + ground.name);

            //Knockback
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockdownForce * -1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        foreach (Collider2D mushroom in hitMushroomUP)
        {
            //Knockback if facing right
            if (facingRight == true)
            {
                FindObjectOfType<AudioManager>().Play("Boing");
                mushroomBounce = true;
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -2.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }

            //Knockback if facing left
            if (facingRight == false)
            {
                FindObjectOfType<AudioManager>().Play("Boing");
                mushroomBounce = true;
                attacking = true;
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 2.0f, 0f);
                Invoke("SetCanMoveToTrue", 0.1f);
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("SetCanAttackToFalse", 0.3f);
                Debug.Log("Attack cooldown started");
                Invoke("SetMushroomBounceToFalse", 0.01f);
            }
        }

        foreach (Collider2D projectile in hitProjectileUP)
        {
            Destroy(projectile.gameObject);
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockdownForce * -1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        foreach(Collider2D ice in hitIceUP)
        {
            Destroy(ice.gameObject);
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockdownForce * -1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        foreach (Collider2D boss in hitBossUP)
        {
            boss.GetComponent<BossHealth>().TakeDamage(attackDamage);

            //Knockback
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockdownForce * -1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }
    }

    void DelayAttackEffectsDOWN()
    {
        //Detect enemies in range of DOWN attack
        Collider2D[] hitEnemiesDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, enemyLayers);

        Collider2D[] hitProjectileDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, projectileLayers);

        Collider2D[] hiticeDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, iceLayers);

        Collider2D[] hitbossDOWN = Physics2D.OverlapCircleAll(attackPointDOWN.position, attackRangeDOWN, bossLayers);

        //Down damage enemy
        foreach (Collider2D enemy in hitEnemiesDOWN)
        {
            enemy.GetComponent<EnemyStats>().TakeDamage(attackDamage);

            //Knockback
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockupForce * 1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        foreach (Collider2D projectile in hitProjectileDOWN)
        {
            Destroy(projectile.gameObject);
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockupForce * 1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        foreach (Collider2D ice in hiticeDOWN)
        {
            Destroy(ice.gameObject);
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockupForce * 1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }

        foreach (Collider2D boss in hitbossDOWN)
        {
            boss.GetComponent<BossHealth>().TakeDamage(attackDamage);

            //Knockback
            attacking = true;
            canMove = false;
            rb.velocity = new Vector2(0f, knockupForce * 1.0f);
            Invoke("SetCanMoveToTrue", 0.1f);
            nextAttackTime = Time.time + 1f / attackRate;
            Invoke("SetCanAttackToFalse", 0.3f);
            Debug.Log("Attack cooldown started");
        }
    }

    void Shoot()
    {
        if(timeBtwShots <= 0 && phoenixForm == true && Input.GetButtonDown("Attack"))
        {
            print("shoot");
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            FindObjectOfType<AudioManager>().Play("PlayerFire");
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void SetCanMoveToTrue()
    {
        canMove = true;
    }

    void SetCanAttackToFalse()
    {
        attacking = false;
    }

    void SetIsAttackingToFalse()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", false);
    }

    void SetCanTakeDamageToTrue()
    {
        canTakeDamage = true;
        anim.SetBool("IsHit", false);
    }

    void SetMushroomBounceToFalse()
    {
        mushroomBounce = false;
    }

    void SetJumpToFalse()
    {
        anim.SetBool("Jumped", false);
    }
    void CheckIfFalling()
    {
        if (isGrounded == false && isWallSliding == false && rb.velocity.y < 0)
        {
            isFalling = true;
            anim.SetBool("IsFalling", true);
        }

        else if (isGrounded == true)
        {
            isFalling = false;
            anim.SetBool("IsFalling", false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage == true)
        {
            currentHealth -= damage;
            FindObjectOfType<AudioManager>().Play("PlayerDamage");

            if (travellerForm == true)
            {
                anim.SetTrigger("Hit");
                print("hit");
            }
            else if (phoenixForm == true)
            {
                anim.SetTrigger("HitPhoenix");
                print("hitphoenix");
            }

            anim.SetBool("IsHit", true);
            canTakeDamage = false;

            if (isGrounded == true && facingRight == true)
            {
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -10f, 10f);
                Invoke("SetCanMoveToTrue", 0.1f);
                Invoke("SetCanTakeDamageToTrue", 0.5f);
            }
            else if (isGrounded == false && facingRight == true)
            {
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * -2f, 2f);
                Invoke("SetCanMoveToTrue", 0.1f);
                Invoke("SetCanTakeDamageToTrue", 0.5f);
            }
            else if (isGrounded == true && facingRight == false)
            {
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 10f, 10f);
                Invoke("SetCanMoveToTrue", 0.1f);
                Invoke("SetCanTakeDamageToTrue", 0.5f);
            }
            else if (isGrounded == false && facingRight == false)
            {
                canMove = false;
                rb.velocity = new Vector2(knockbackForce * 2f, 2f);
                Invoke("SetCanMoveToTrue", 0.1f);
                Invoke("SetCanTakeDamageToTrue", 0.5f);
            }
            if (currentHealth <= 0 && isDead == false)
            {
                canTakeDamage = false;
                isDead = true;
                Die();
            }
            else
            {
                return;
            }
        }
    }

    public void Die()
    {
        {
            if(isDead == true)
            {
                FindObjectOfType<AudioManager>().Play("Burn");
                canAttack = false;
                canDash = false;
                canJump = false;
                canTakeDamage = false;
                rb.velocity = new Vector2(0f * 0f, 0f);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                anim.SetBool("IsDead", true);
                Invoke("ChangeForm", 1.5f);
            }
        }
    }

    public void ChangeForm()
    {
        if (isDead == true)
        {
           FindObjectOfType<AudioManager>().Play("PhoenixCry");
           phoenixForm = true;
           travellerForm = false;
           anim.SetBool("IsPhoenix", true);
           currentHealth = maxHealth;
           rb.constraints = RigidbodyConstraints2D.None;
           rb.constraints = RigidbodyConstraints2D.FreezeRotation;
           healthBarUI.SetActive(true);
           heart1.SetActive(false);
           heart2.SetActive(false);
           heart3.SetActive(false);
           heart4.SetActive(false);

        }
        else if (isDead == false)
        {
           phoenixForm = false;
           travellerForm = true;
           currentHealth = maxHealth;
           anim.SetBool("IsPhoenix", false);
           anim.SetBool("IsDead", false);
           healthBarUI.SetActive(false);
           heart1.SetActive(true);
           heart2.SetActive(true);
           heart3.SetActive(true);
           heart4.SetActive(true);
        }
    }

    public void DamageOverTime()
    {
        if(phoenixForm == true)
        {
            currentHealth -= 0.15f * Time.deltaTime;
        }
        if(currentHealth <= 0)
        {
            currentHealth = 0;
        }

    }

    public void GameOver()
    {
        if (phoenixForm == true && currentHealth <= 0)
        {
            canMove = false;
            print("Game Over");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("enemiesdefeatedvalue", 0);
            PlayerPrefs.SetFloat("currenthealth", 100);
            SceneManager.LoadScene("GameOver");
            Destroy(GameObject.Find("BGM"));
        }
    }

    public void SetCurrentHealthToMaxHealth()
    {
        if(currentHealth > 4)
        {
            currentHealth = maxHealth;
        }
        if(currentHealth <= 0 && isDead == false) 
        {
            currentHealth = maxHealth;
        }
    }

    private void Resurrect()
    {
        if (inTrigger == true && Input.GetButtonDown("Resurrect") && phoenixForm == true)
        {
            FindObjectOfType<AudioManager>().Play("Burn2");
            FindObjectOfType<AudioManager>().Play("Health");
            Debug.Log("Resurrect");
            isDead = false;
            canAttack = true;
            canDash = true;
            canJump = true;
            canTakeDamage = true;
            ChangeForm();
        }

        if(inTrigger == true && Input.GetButtonDown("Resurrect") && travellerForm == true && currentHealth <= 3)
        {
            FindObjectOfType<AudioManager>().Play("Burn2");
            FindObjectOfType<AudioManager>().Play("Health");
            currentHealth = maxHealth;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("SceneTriggerNext"))
        {
            spawnLocation = 0;
            PlayerPrefs.SetInt("spawnlocationvalue", spawnLocation);
        }

        if (collision.tag == ("SceneTriggerPrevious"))
        {
            spawnLocation = 1;
            PlayerPrefs.SetInt("spawnlocationvalue", spawnLocation);
        }

        if (collision.tag == ("Resurrect") && phoenixForm == true)
        {
            inTrigger = true;
            resurrectUI.SetActive(true);
        }

        if (collision.tag == ("Resurrect") && travellerForm == true && currentHealth < maxHealth)
        {
            inTrigger = true;
            healUI.SetActive(true);
        }
        if (collision.tag == ("Transform") && transformed == false)
        {
            isDead = true;
            canDash = false;
            canAttack = false;
            ChangeForm();
            transformed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Resurrect"))
        {
            inTrigger = false;
            resurrectUI.SetActive(false);
            healUI.SetActive(false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Mushroom") && facingRight == true && isGrounded == false)
        {
            FindObjectOfType<AudioManager>().Play("Boing");
            mushroomBounce = true;
            canMove = false;
            rb.velocity = new Vector2(knockbackForce * 0f, 15f);
            Invoke("SetCanMoveToTrue", 0.1f);
            Invoke("SetMushroomBounceToFalse", 0.01f);
        }
        if (collision.gameObject.tag == ("Mushroom") && facingRight == false && isGrounded == false)
        {
            FindObjectOfType<AudioManager>().Play("Boing");
            mushroomBounce = true;
            canMove = false;
            rb.velocity = new Vector2(knockbackForce * 0f, 15f);
            Invoke("SetCanMoveToTrue", 0.1f);
            Invoke("SetMushroomBounceToFalse", 0.01f);
        }
        if (collision.gameObject.tag == ("IcicleDamage"))
        {
            print("hit");
            TakeDamage(1);
        }
        if(collision.gameObject.tag == ("Ground") && isDead == false)
        {
            FindObjectOfType<AudioManager>().Play("Land");
        }
    }

    private void OnDrawGizmosSelected()
    {
        //ground check
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.45f), new Vector2(0.1f, 0.1f));

        //wall check
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - 0.11f, gameObject.transform.position.y), new Vector2(0.1f, 0.7f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + 0.20f, gameObject.transform.position.y), new Vector2(0.1f, 0.7f));

        //Attack Point
        if (attackPointLR == null)
            return;

            Gizmos.DrawWireSphere(attackPointLR.position, attackRangeLR);


        if (attackPointUP == null)
            return;

            Gizmos.DrawWireSphere(attackPointUP.position, attackRangeUP);

        if (attackPointDOWN == null)
            return;

        Gizmos.DrawWireSphere(attackPointDOWN.position, attackRangeDOWN);

    }

    public void HealthUI()
    {
        if(currentHealth > numOfHearts)
        {
            currentHealth = numOfHearts;
        }

        for (int i =0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }        
    }

    public void FootSteps()
    {
        audioSource.Play();
    }
}
