using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    public float speed = 5f;

    private Animator animator;

    public Transform playerCamera; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        animator.SetFloat("MoveX",h);
        animator.SetFloat("MoveY",v);

        animator.SetFloat("X", h);  
        animator.SetFloat("Y", v);


        // направление камеры
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        // убираем наклон
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // движение относительно камеры
        Vector3 move = camForward * v + camRight * h;

        // движение
        transform.position += move * speed * Time.deltaTime;
    }
}
