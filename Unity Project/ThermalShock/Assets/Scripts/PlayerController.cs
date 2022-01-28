using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0f;

    [Header("Health")]
    public float heat;
    public float heatMax;
    public float heatMin;
    public float health;

    [Header("Attack - Gun")]
    public bool gunAttacking;
    public float gunHeatGenerated;
    public float gunAttackDelay;
    public float gunAttackCooldown;
    public GameObject projectilePrefab;

    private float gunClock;
    private int gunState;

    [Header("Attack - Sword")]
    public bool swordAttacking;
    public float swordHeatGenerated;
    public float swordAttackDelay;
    public float swordAttackCutTime;
    public float swordAttackCooldown;
    public GameObject swordObject;

    private float swordClock;
    private int swordState;

    [Space]
    [Header("Internal Stuff")]
    public GameObject helperObject;
    public GameObject otherHelperObject;
    private CharacterController charControl;
    Vector3 pos;
    Quaternion rot;

    // Start is called before the first frame update
    void Start()
    {
        charControl = gameObject.GetComponent<CharacterController>();
        
        gunAttacking = false;
        gunState = 0;

        swordAttacking = false;
        swordState = 0;
    }

    // Update is called once per frame
    void Update()
    {

        (pos, rot) = GetDirection();

        if (Input.GetButtonDown("Fire1")) { gunAttacking = true; }
        if (Input.GetButtonDown("Fire2")) { swordAttacking = true; }
        GetDirection();
        GunAttack();
        SwordAttack();
        Movement();
    }

    // Check and do movement based on inputs.
    void Movement()
    {
        Vector3 moveInput = Vector2.zero;
        moveInput.z = Input.GetAxisRaw("Vertical");
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.Normalize();
        //Debug.Log("Movement direction magnitude is " + moveInput.magnitude);
        charControl.Move(moveInput * moveSpeed * Time.deltaTime);
    }

    // calculates the angle/ direction the mouse is from the player character, to be fed into attacks.
    (Vector3, Quaternion) GetDirection()
    {
        Plane aimPlane = new Plane(Vector3.up, Vector3.up);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 mouseHitPoint = Vector3.zero;
        float rayHitDist;
        if (aimPlane.Raycast(mouseRay, out rayHitDist) )
        {
            mouseHitPoint = mouseRay.GetPoint(rayHitDist);
        }
        
        mouseHitPoint = gameObject.transform.InverseTransformPoint(mouseHitPoint);
        mouseHitPoint.y = 0f;
        mouseHitPoint.Normalize();
        mouseHitPoint = mouseHitPoint * 2f;
        Quaternion lookAt = Quaternion.LookRotation(mouseHitPoint, Vector3.up);

        otherHelperObject.transform.SetPositionAndRotation(otherHelperObject.transform.position, lookAt);
        helperObject.transform.SetPositionAndRotation(mouseHitPoint + gameObject.transform.position, helperObject.transform.rotation);

        return (mouseHitPoint, lookAt);
    }


    // gun attack logic
    void GunAttack() {
        if (gunAttacking)
        {
            gunClock += Time.deltaTime;
            //Debug.Log("Gun's clock is running. Gun is attacking.");
            switch (gunState)
            {
                case 0:
                    if (gunClock >= gunAttackDelay) { gunState = 1; }
                    //Debug.Log("Gun is in state 0, the pre attack state.");
                    break;

                case 1:
                    //Debug.Log("Gun is in state 1, the projectile spawn/attack state.");
                    SpawnProjectile();
                    gunState = 2;
                    break;

                case 2:
                    //Debug.Log("Gun is in state 2, the cooldown state.");
                    if (gunClock >= gunAttackDelay + gunAttackCooldown) {
                        //Debug.Log("Gun is at the end of state 2, state, clock and attacking bool are reset."); 
                        gunState = 0; gunClock = 0f; gunAttacking = false; 
                    }
                    break;

                default:
                    //Debug.Log("Gun is in error state. This should not be happening.");
                    break;
            }


        }
    }

    void SwordAttack()
    {
        if (swordAttacking)
        {
            swordClock += Time.deltaTime;
            switch (swordState)
            {
                case 0:
                    if (swordClock >= swordAttackDelay) { swordState = 1; }
                    break;

                case 1:
                    swordState = 2;
                    //Turn on sword
                    break;
                case 2:
                    if (swordClock >= swordAttackDelay + swordAttackCutTime)
                    {
                        //Turn off sword
                        swordState = 3;
                    }
                        break;
                case 3:
                    if (swordClock >= swordAttackDelay + swordAttackCutTime + swordAttackCooldown)
                    {
                        swordState = 0; swordClock = 0f; swordAttacking = false;
                    }
                    break;

                default:
                    break;
            }


        }
    }

    // code for the player to fire a projectile.
    void SpawnProjectile()
    {
        //get Direction of mouse/second stick and create a projectile prefab moving in that direction.
        
        Instantiate<GameObject>(projectilePrefab, gameObject.transform.position + pos, rot);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player has collided with " + collision.gameObject.name);
    }

}
