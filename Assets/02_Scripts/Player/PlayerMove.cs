using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private Transform tr;
    public Vector3 dir;
    public float h = 0.0f;
    public float v = 0.0f;
    public float rotY = 0.0f;
    private float jumpForce = 250f;
    public bool isJump = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        PlayerRotate();
        Jump();
    }

    void Update()
    {

    }

    private void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float targetV = (Input.GetKey(KeyCode.LeftShift) && v > 0) ? 2f : v;
        float curV = animator.GetFloat("PosY");
        float smoothV = Mathf.Lerp(curV, targetV, Time.deltaTime * 10f);

        animator.SetFloat("PosY", smoothV);
        animator.SetFloat("PosX", h);

        dir = new Vector3(h / 15f, 0, smoothV / 15f);
        tr.Translate(dir, Space.Self);
    }
    private void PlayerRotate()
    {
        rotY = Input.GetAxis("Mouse X");
        tr.Rotate(0f, rotY * 2f, 0f);
    }
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && !isJump)
        {
            isJump = true;
            animator.SetTrigger("JumpTrigger");
            Invoke("JumpNow", 0.7f);
        }
    }
    private void JumpNow()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
            isJump = false;

    }

}