using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Random = UnityEngine.Random;
public class PlayerScripts : MonoBehaviour
{   
    public GameObject Crosshair;
    #region Movement
    public float speed = 5f;
    public ParticleSystem dust;
    public bool isRunning;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    public bool canMove = true;
    AudioManager audioManager;
    CameraShake camShaker;
    bool alive = true;
    void MovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        int animLayer = 0;

        if (canMove)
        {
            bool isPlaying(Animator anim, string stateName){
                if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) 
                && anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
                    return true;
                else 
                    return false;
                
            }
            if(x != 0 || y != 0)
            {
                isRunning = true;
                CreateDust();
                if(selectedWeapon == 1){
                    if (!isPlaying(anim ,"attack_enxada") && !isPlaying(anim ,"upattack") && !isPlaying(anim ,"downattack")){

                        anim.Play("running_weapon");
                    }

                }
                else if (selectedWeapon == 2){
                    anim.Play("running_nohand");
                    
                }else {
                    anim.Play("running_seed");

                }
                Walk(dir);
            }
            else
            {
                isRunning = false;
                rb.velocity = Vector2.zero;
                StopDust();
                
                if(selectedWeapon == 1){
                    if (!isPlaying(anim ,"attack_enxada") && !isPlaying(anim ,"upattack") && !isPlaying(anim ,"downattack")){

                        anim.Play("idle");
                }

                }
                else if (selectedWeapon == 2){
                    anim.Play("idle_nohand");
                    
                }else {
                    anim.Play("idle_seed");

                }
            }

            if (x < 0)
            {

                if (!isPlaying(anim ,"attack_enxada") && !isPlaying(anim ,"upattack") && !isPlaying(anim ,"downattack")){

                    sr.flipX = true;
                }
            }
            if (x > 0)
            {
                if (!isPlaying(anim ,"attack_enxada") && !isPlaying(anim ,"upattack") && !isPlaying(anim ,"downattack")){

                    sr.flipX = false;
                }
            }
        }

    }

    //Variables for creating dust
    float lastStep;
    public float stepRate;
    public Vector3 Offset = new Vector3(0, -3f, 0);
    void CreateDust()
    {
        if (Time.time - lastStep > 1 / stepRate)
        {
            lastStep = Time.time;
            dust.transform.position = transform.position + Offset;
            dust.Play();
        }
    }
    void StopDust()
    {
        dust.Stop();
    }

    void Walk(Vector2 dir)
    {
        rb.velocity = dir.normalized * speed;
    }
    #endregion

    #region Attacks
    public float maxStamina = 100;
    float currentStamina;
    
    public float dashSpeed = 2000;
    private bool isDashing = false;
    public float attackRadius;
    public int attackDamage;
    public Transform attackPos;
    public LayerMask enemies;
    float timeSinceLastAttack;

    #region statusbars
    public float staminaTimer = 3f;
    public float staminaRegen = 3f;
    float currentHealth;
    public float maxHealth;
    float currentWater;
    public float maxWater;

    void RegenHealth(){
        if (currentHealth <= 0){
            Die();
        }
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }

        float healthNormalized = currentHealth/maxHealth;
        GameObject.Find("StatusBars").GetComponent<UIScripts>().SetHealth(healthNormalized);
    }
    void RegenWater(){
        if (currentWater <= 0){
            currentWater = 0;
        }
        if (currentWater > maxWater){
            currentWater = maxWater;
        }
        if (isOnWater){
            currentWater += 35 * Time.deltaTime;
        }

        float waterNormalized = currentWater/maxWater;
        GameObject.Find("StatusBars").GetComponent<UIScripts>().SetWater(waterNormalized);
    }
    void RegenStamina(){
        if (Time.time - timeSinceLastAttack > 1/staminaTimer){
            currentStamina += staminaRegen * Time.deltaTime;
        }
        if (currentStamina < 0){
            currentStamina = 0;
        }
        if (currentStamina > maxStamina){
            currentStamina = maxStamina;
        }

        float staminaNormalized = currentStamina/maxStamina;
        GameObject.Find("StatusBars").GetComponent<UIScripts>().SetStamina(staminaNormalized);
        

    }
    #endregion
    public GameObject RestartButton;
    void Die(){
        alive = false;
        RestartButton.SetActive(true);
        audioManager.Play("youdied", 1f, 1f);
        audioManager.Stop("Theme");
        Cursor.visible = true;
        Destroy(gameObject);
    }

    public bool hasSuperDash;
    int selectedWeapon = 1;
    bool isPlanting;
    GameObject thisFarm;
    GameObject pranta;
    public GameObject selecter;
    public GameObject position1, position2, position3, position4, position5;
    [SerializeField] GameObject pranta1;
    [SerializeField] GameObject pranta2;
    [SerializeField] GameObject pranta3;
    [SerializeField] GameObject handSprite;
    public int seeds1 = 1;
    public int seeds2 = 0;
    public int seeds3 = 0;
    void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            selectedWeapon = 1;
            waterParticle.Stop();
            audioManager.Stop("water");
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)){
            selectedWeapon = 3;
            waterParticle.Stop();
            audioManager.Stop("water");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)){
            selectedWeapon = 4;
            waterParticle.Stop();
            audioManager.Stop("water");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)){
            selectedWeapon = 5;
            waterParticle.Stop();
            audioManager.Stop("water");
        }


        if (selectedWeapon == 1){
            isPlanting = false;
            handSprite.SetActive(false);
            selecter.transform.position = position1.transform.position;
            if (Input.GetButtonDown("Fire1")){
                if (currentStamina > 0){
                    timeSinceLastAttack = Time.time;
                    Attack();
                }

            }
            if (Input.GetButtonDown("Fire2")){
                if(hasSuperDash){
                    if(currentStamina > 150 && currentHealth > 10f){
                        timeSinceLastAttack = Time.time;
                        SuperAttack();
                    }
                }
            }
        }   
        if (selectedWeapon == 2){
            isPlanting = false;
            handSprite.SetActive(true);
            selecter.transform.position = position2.transform.position;
            Watering();
        }   
        if (selectedWeapon == 3){
            pranta = pranta1;
            handSprite.SetActive(false);
            selecter.transform.position = position3.transform.position;
            isPlanting = true;
        }   
        if (selectedWeapon == 4){
            pranta = pranta2;
            handSprite.SetActive(false);
            selecter.transform.position = position4.transform.position;
            isPlanting = true;
            
        }   
        if (selectedWeapon == 5){
            pranta = pranta3;
            handSprite.SetActive(false);
            selecter.transform.position = position5.transform.position;
            isPlanting = true;
            
        }   

        CheckForDash();
    }

    public GameObject flash;
    void SuperAttack(){
        flash.SetActive(true);
        currentStamina -= 150;
        audioManager.Play("Attack", Random.Range(.9f, 1.3f), 1f);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDirection = mousePos - transform.position;

        rb.velocity = Vector2.zero;
        rb.velocity += dashDirection.normalized * dashSpeed * 4;

        float dashAngle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
        if (dashAngle < 145f && dashAngle > 45f)
        {
            anim.Play("downattack", -1, 0f);
        }
        else if (dashAngle > -145f && dashAngle < -45f)
        {
            anim.Play("upattack", -1, 0f);
        }
        else
        {
            anim.Play("attack_enxada", -1, 0f);
        }
        if (mousePos.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        
        if (isDashing){
            StopCoroutine("SuperDashWait");
            StartCoroutine("SuperDashWait");
        }else{
            StartCoroutine("SuperDashWait");

        }

    }
    void Attack()
    {
        currentStamina -= 10;
        audioManager.Play("Attack", Random.Range(.9f, 1.3f), 1f);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDirection = mousePos - transform.position;

        rb.velocity = Vector2.zero;
        rb.velocity += dashDirection.normalized * dashSpeed;

        float dashAngle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
        if (dashAngle < 145f && dashAngle > 45f)
        {
            anim.Play("downattack", -1, 0f);
        }
        else if (dashAngle > -145f && dashAngle < -45f)
        {
            anim.Play("upattack", -1, 0f);
        }
        else
        {
            anim.Play("attack_enxada", -1, 0f);
        }
        if (mousePos.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }


        //do damage
        if (isDashing){
            StopCoroutine("DashWait");
            StartCoroutine("DashWait");
        }else{
            StartCoroutine("DashWait");

        }
    }
    
    public GameObject armPivot;
    public GameObject armPivot2;
    public float waterRegen = 10;
    public float waterCost = 10;
    public ParticleSystem waterParticle;
    public LayerMask plants;
    public LayerMask fire;
    public GameObject waterPoint;
    bool audioPlaying = false;
    void Watering(){
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - armPivot.transform.position;
        difference.Normalize();
        float rotation =  Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        armPivot.transform.rotation = Quaternion.Euler(0f,0f, rotation );
        armPivot2.transform.rotation = Quaternion.Euler(0f,0f, rotation );
        if (rotation < -90f || rotation > 90f){
            if (transform.eulerAngles.y == 0){
                armPivot.transform.localRotation = Quaternion.Euler(180f, 0, -rotation );
                armPivot.transform.position = new Vector3(armPivot.transform.position.x, armPivot.transform.position.y, -5f);
            }
        }
        if (Input.GetButton("Fire1") && currentWater > 0){
            currentWater -= waterCost *Time.deltaTime;
            waterParticle.Play();
            if (!audioPlaying){
                audioManager.Play("water", 1.4f, .3f);
                audioPlaying = true;
            }
            Collider2D[] plantsInside = Physics2D.OverlapCircleAll(waterPoint.transform.position, 0.9f, plants);
            for (int i = 0; i < plantsInside.Length; i++){
                plantsInside[i].GetComponent<PlantScript>().TakeWater(50 * Time.deltaTime);
                
            }
            Collider2D[] fireInside = Physics2D.OverlapCircleAll(waterPoint.transform.position, 0.9f, fire);
            for (int i = 0; i < fireInside.Length; i++){
                fireInside[i].GetComponent<bonfire>().unfire();
                
            }

        }else{
            
            audioManager.Stop("water");
            audioPlaying = false;
            waterParticle.Stop();
        }
    }

    [SerializeField] GameObject drawbox;
    bool isOnFarm;
    bool isOnWater;
    Vector3 farmPos;

    void OnTriggerExit2D(Collider2D col){
        isOnFarm = false;
        isOnWater = false;
        waterParticle2.Stop();
    }
    void OnTriggerStay2D(Collider2D col){
        if (col.gameObject.CompareTag("Farmland")){
            isOnFarm = true;
            farmPos = col.gameObject.transform.position;
            thisFarm = col.gameObject;

        }
        if (col.gameObject.CompareTag("water")){
            isOnWater = true;
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("water")){
            waterParticle2.Play(true);
        }
        if (col.gameObject.CompareTag("bullet")){
            TakeDamage(10);
            Destroy(col.gameObject);
            Vector2 dir = transform.position - col.gameObject.transform.position;
            rb.velocity = dir.normalized * 15;
        }
    }

    void CheckForDash()
    {
        if (isDashing)
        {
            Collider2D[] enemiesInside = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, enemies);
            for (int i = 0; i < enemiesInside.Length; i++)
            {
                if (enemiesInside.Length > 0){
                    rb.velocity = Vector2.zero;
                    isDashing = false;
                    canMove = true;
                }
                enemiesInside[0].GetComponent<EnemyScript>().TakeDamage(attackDamage);
            }
        }
        if (isSuperDashing)
        {
            Collider2D[] enemiesInside = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, enemies);
            for (int i = 0; i < enemiesInside.Length; i++)
            {
                enemiesInside[i].GetComponent<EnemyScript>().TakeDamage(10);
            }
        }

    }
    IEnumerator DashWait()
    {

        isDashing = true;
        canMove = false;
        yield return new WaitForSeconds(0.3f);

        isDashing = false;
        canMove = true;
    }
    bool isSuperDashing;
    IEnumerator SuperDashWait()
    {

        isSuperDashing = true;
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        flash.SetActive(false);

        isSuperDashing = false;
        canMove = true;
    }
    #endregion
    
    #region Camera
    
    public Transform cameraCarrier;
    public Transform carrierCarrier;
    public float lerpSpeed;
    public float lerpSpeed2;
    GameObject gameCamera;
    
    void LerpCamera()
    {
        Vector3 lastPosition = carrierCarrier.position;
        Vector3 endPos = transform.position;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Crosshair.transform.position = new Vector3(mousePos.x, mousePos.y, -10f);
        //carrierCarrier.position = Vector3.Lerp(transform.position, mousePos, lerpSpeed);
        cameraCarrier.position = Vector3.Lerp(lastPosition, endPos, lerpSpeed2);
    }
    #endregion

    #region Health
    public void TakeDamage(float damage){
        currentHealth -= damage;
        AudioManager.instance.Play("playerDamage", Random.Range(.7f, 1.3f), 1f);
        DisableMovementForAWhile();
    }
    #endregion
    GameObject devilblabla;
    private void Start()
    {   
        devilblabla = GameObject.Find("devilblabla");
        devilblabla.SetActive(false);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        uiScripts = GameObject.Find("StatusBars").GetComponent<UIScripts>();
        seeds1 = 3;
        camShaker = GetComponent<CameraShake>();
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        currentWater = maxWater;
        gameCamera = GameObject.Find("Camera");
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        matWhite = Resources.Load("WhiteFlash", typeof (Material)) as Material;
        defaultMat = sr.material;
    }

    UIScripts uiScripts;
    void FixedUpdate(){
        LerpCamera();
    }
    private void Update()
    {
        if (isPlanting){
            if (isOnFarm){
                drawbox.SetActive(true);
                drawbox.transform.position = farmPos;
                if (Input.GetButtonDown("Fire1")){
                    if (selectedWeapon == 3 && seeds1 > 0){
                        Instantiate(pranta, farmPos + Random.insideUnitSphere *.2f, Quaternion.identity);
                        seeds1 -=1;
                        Destroy(thisFarm);
                        uiScripts.UpdateText();
                    }
                    if (selectedWeapon == 4 && seeds2 > 0){
                        Instantiate(pranta, farmPos + Random.insideUnitSphere *.2f, Quaternion.identity);
                        seeds2 -=1;
                        Destroy(thisFarm);
                        uiScripts.UpdateText();
                    }
                    if (selectedWeapon == 5 && seeds3 > 0){
                        Instantiate(pranta, farmPos + Random.insideUnitSphere *.2f, Quaternion.identity);
                        seeds3 -=1;
                        Destroy(thisFarm);
                        uiScripts.UpdateText();
                    }
                }
            }else{
                drawbox.SetActive(false);
            }

        }else{
            drawbox.SetActive(false);
        }
        RegenWater();
        RegenHealth();
        RegenStamina();
        if (alive){
            Cursor.visible = false;

        }
        MovementInput();
        AttackInput();
        if (hasSuperDash && !aquired){
            devilblabla.SetActive(true);
            aquired = true;
        }
    }
    bool aquired;
    
    private Material matWhite;
    private Material defaultMat;
    public void DisableMovementForAWhile(){
        StartCoroutine("DisableMovement");
        sr.material = matWhite;
        GameObject.Find("Camera").GetComponent<CameraShake>().shakeDuration += 0.1f;
        Invoke("ResetMaterial", .25f);


    }
    void ResetMaterial(){
        sr.material = defaultMat;
    }

    IEnumerator DisableMovement(){
        canMove = false;
        sr.material = matWhite;
        yield return new WaitForSeconds(.3f);
        canMove = true;
    }

    public ParticleSystem healingParticle;
    public ParticleSystem levelParticle;
    public ParticleSystem waterParticle2;

    public void LevelUp(){
        levelParticle.Play();
        currentHealth += 10;
        maxStamina += 5f;
        maxWater += 2.5f;
    }

    public void Heal(float amount){
        healingParticle.Play();
        currentHealth += amount;
    }

    #region Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
    #endregion

}
