using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainmove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 2f;
    public float jumpHeight = 15;
    public float gravity = 200f;
    public float airControl = 0.5f;
    public float smoothValue = 0.1f;
    float turnVelocity;

    Vector3 jumpDirection;
    float highJump = 1.0f;
    // public GameObject playerCamera;
    CharacterController controller;
    Animator animator;
    public GameObject b1;
    public GameObject b2;
    public GameObject b3;

    // public AudioSource glyphSound;
    // public AudioSource lifeSound;
    // public AudioSource hitSound;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        jumpDirection = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;
        // dropcage = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction.Normalize();

        if (direction.magnitude >= 0.1f){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, smoothValue);
            transform.rotation = Quaternion.Euler(0, targetAngle, 0); //Updates character rotation
            direction = transform.forward; //changes direction of chara after rotated

            //moving resets the superjump
            highJump = 1.0f;
        }
        
        direction *= speed;
        animator.SetFloat("speed", direction.magnitude);

        if(controller.isGrounded){
            
            //did i just fall?
            if(animator.GetBool("jumpend") == true){
                animator.SetBool("jumpbeginning", false);
            }
            //I have started jumping, but already on the ground -> proceed
            else if (animator.GetBool("jumpbeginning")){
                animator.SetBool("jumpmid", true);
                animator.SetBool("jumpbeginning", false);
            }
            //about to jump
            //if I am not moving,superjump
            if(Input.GetButtonDown("Jump") && (direction.magnitude < 0.1f)){
                animator.SetBool("jumpend", false);
                animator.SetBool("jumpbeginning", true);
                highJump = 1.1f;
            }          

            jumpDirection = direction;

            //release spacebar
            //start jumping
            if(Input.GetButtonUp("Jump")){
                //move the character up
                jumpDirection.y = jumpHeight * highJump; //* Time.deltaTime;
                highJump = 1.0f;

                //unparent the character from the platform
                controller.gameObject.transform.parent = null;

                //start jump animation
                animator.SetBool("jumpbeginning", true);
                // animator.SetBool("jumpprep", false);
                animator.SetBool("jumpmid", false);
                animator.SetBool("jumpend", false);

            }else{
                jumpDirection.y = 0;
            }

        }else{//I am in the air
            //if started falling
            //print("does it get here?");
            if(jumpDirection.y < 0){
                animator.SetBool("jumpend", true);

                animator.SetBool("jumpbeginning", false);
                // animator.SetBool("jumpprep", false);
                // animator.SetBool("jumpend", false);
            }
            direction.y = jumpDirection.y;
            jumpDirection = Vector3.Slerp(jumpDirection, direction, airControl * Time.deltaTime);
            
        }
        
        //apply gravity
        jumpDirection.y -= gravity * Time.deltaTime;
        //controller.Move(jumpDirection * Time.deltaTime); // transform.position += direction * Time.deltaTime;

        CollisionFlags fl = controller.Move(jumpDirection * Time.deltaTime);

        if (!controller.isGrounded && (fl & CollisionFlags.Sides) > 0){
            jumpDirection.Normalize();
            jumpDirection = new Vector3(-jumpDirection.x*1f, -gravity * Time.deltaTime, -jumpDirection.z*1f);
            controller.Move(jumpDirection);
        }

        // if (dropcage){
        //     cage.gameObject.transform.position = new Vector3(cage.transform.position.x, cage.transform.position.y - 0.02f, cage.transform.position.z);
        //     //cage.SetActive(false);
        // }

        
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            controller.gameObject.transform.parent = null;
            //if heart, just disappears
            // if(hit.gameObject.CompareTag("Heart"))
            // {
                
            //     if(health < 5){
            //         hit.gameObject.GetComponent<pickup>().Pick();//SetActive(false);
            //         health = health + 1;
            //         lifeSound.Play();
            //     }
            // }

            if(hit.gameObject.CompareTag("b1"))
            {
                Debug.Log("hello?");
                b1.SetActive(true);
            }

            if(hit.gameObject.CompareTag("b2"))
            {
                b2.SetActive(true);
            }

            if(hit.gameObject.CompareTag("b3"))
            {
                b3.SetActive(true);
            }

            //if platform, parent
            if(hit.gameObject.CompareTag("Platform"))
            {
                controller.gameObject.transform.parent = hit.transform;
            }
        }
}
