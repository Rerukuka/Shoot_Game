using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_NewInput : MonoBehaviour
{
    [Header("Движение")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;

    [Header("Вращение мышью")]
    public Transform playerBody;
    public Transform cameraHolder;
    public float mouseSensitivity = 0.2f;
    public float mouseSmoothing = 5f;

    [Header("Стрельба")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private AudioSource gunAudio;
    [SerializeField] private float burstRate = 6f;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    [Header("Патроны и перезарядка")]
    [SerializeField] private int maxAmmo = 50;
    [SerializeField] private AudioSource reloadAudio;
    [SerializeField] private AudioSource emptyClickAudio;
    [SerializeField] private float reloadDelayEmpty = 3f;
    [SerializeField] private float reloadDelayNotEmpty = 1f;

    private int currentAmmo;
    private bool isReloading = false;
    private bool isDead = false;
    private bool alreadyClickedEmpty = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Анимация")]
    public Animator animator;
    public GameObject hitEffectPrefab; // ← Назначишь в инспекторе


    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 smoothedLook;
    private float postReloadBlockTime = 0.2f;
    private float reloadEndTime = -1f;

    private float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;
    private float shootHoldTime = 0f;
    private bool isHoldingShoot = false;
    private float burstTimer = 0f;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Reload.performed += ctx => Reload();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void SetDead()
    {
        isDead = true;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        Look();
        Move();
        HandleShooting();
        UpdateAmmoUI();
        if (isDead) return; // ← блокируем поворот мыши

    }

    void Look()
    {
        Vector2 targetLook = lookInput * mouseSensitivity;
        smoothedLook = Vector2.Lerp(smoothedLook, targetLook, Time.deltaTime * mouseSmoothing);

        playerBody.Rotate(Vector3.up * smoothedLook.x); // Только по горизонтали
    }

    void Move()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        bool isRunning = Keyboard.current.leftShiftKey.isPressed && moveInput.y > 0;
        float speed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveZ", moveInput.y);
        animator.SetBool("IsRunning", isRunning);
    }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    void Reload()
    {
        // ✅ если уже перезаряжается — не трогаем
        if (isReloading || currentAmmo == maxAmmo) return;

        isReloading = true;
        animator.SetTrigger("Reload");

        if (reloadAudio != null && reloadAudio.clip != null)
        {
            reloadAudio.PlayOneShot(reloadAudio.clip);
        }

        // ✅ всегда проверяем на патроны
        float delay = (currentAmmo <= 0) ? reloadDelayEmpty : reloadDelayNotEmpty; // ННННННННННННННННЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕ ММММММММММММММММММЕЕЕЕЕЕЕЕЕЕННННННННҐҐҐҐҐҐҐҐҐҐЙЙЙЙЙЙЙЙ
        CancelInvoke(nameof(FinishReload)); // на всякий случай отменим предыдущий
        Invoke(nameof(FinishReload), delay);
    }



    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;

        reloadEndTime = Time.time;

        // Запускаем автострельбу, если мышь зажата
        if (Mouse.current.leftButton.isPressed)
        {
            shootHoldTime = 0f;
            isHoldingShoot = true;
        }
    }



    void HandleShooting()
    {
        // ❌ Блокировка стрельбы во время перезарядки
        if (isReloading)
        {
            animator.SetBool("ShootLoop", false);
            return;
        }

        // 🧱 Пустой магазин
        if (currentAmmo <= 0)
        {
            animator.SetBool("ShootLoop", false);

            // 🔊 Звук щелчка — только 1 раз за нажатие
            if (Mouse.current.leftButton.wasPressedThisFrame && !alreadyClickedEmpty)
            {
                if (emptyClickAudio != null && emptyClickAudio.clip != null)
                {
                    emptyClickAudio.PlayOneShot(emptyClickAudio.clip);
                }
                alreadyClickedEmpty = true;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                alreadyClickedEmpty = false;
            }

            return;
        }

        // 👆 Начало нажатия ЛКМ
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            shootHoldTime = 0f;
            isHoldingShoot = true;
        }

        // 👊 Удержание ЛКМ
        if (Mouse.current.leftButton.isPressed && isHoldingShoot)
        {
            shootHoldTime += Time.deltaTime;

            if (shootHoldTime >= 0.5f)
            {
                if (!animator.GetBool("ShootLoop"))
                    animator.SetBool("ShootLoop", true);

                burstTimer += Time.deltaTime;
                float interval = 1f / burstRate;

                if (burstTimer >= interval && currentAmmo > 0)
                {
                    currentAmmo--;
                    PlayShotEffects();

                    // 💥 Если патроны закончились после выстрела
                    if (currentAmmo == 0)
                    {
                        Invoke(nameof(Reload), reloadDelayEmpty); // 3 сек
                    }

                    burstTimer = 0f;
                }
            }
        }

        // 🖐 Отпускание ЛКМ
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            // 👆 Одиночный выстрел
            if (shootHoldTime < 0.5f && currentAmmo > 0)
            {
                // 🚫 Если прошло <0.2 сек с конца перезарядки — НЕ стрелять одиночным
                if (Time.time - reloadEndTime >= postReloadBlockTime)
                {
                    animator.SetTrigger("Shoot");
                    currentAmmo--;
                    PlayShotEffects();
                }
            }


            animator.SetBool("ShootLoop", false);
            burstTimer = 0f;
            isHoldingShoot = false;
        }
    }


    void PlayShotEffects()
    {
        // 🔊 Звук выстрела
        if (gunAudio != null && shotSound != null)
        {
            gunAudio.PlayOneShot(shotSound);
        }

        // 🔥 Вспышка на дуле
        if (muzzleFlashPrefab != null && muzzlePoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
            ParticleSystem ps = flash.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
            Destroy(flash, 0.5f);
        }

        // 🎯 Raycast для попадания
        Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 💥 Кровь / партикл на месте попадания
                if (hitEffectPrefab != null)
                {
                    GameObject blood = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    blood.transform.SetParent(hit.collider.transform);
                }

                // 💥 Урон врагу
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeHit();
                }
            }
        }
    }




    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"AMMO: {currentAmmo} / {maxAmmo}";
        }
    }
}
