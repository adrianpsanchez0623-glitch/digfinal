using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;

    public InputAction MoveAction;
    public InputAction RunAction;
    
    public float stamina = 100f;
    public bool isRunning;
    public bool isExhausted;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;

    public TextMeshProUGUI coinText;

    public GameObject audioPlayer;
    public AudioClip coinSound;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public int coins = 0;
    public int maxCoins = 7;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        RunAction.Enable();
        coinText.text = "Coins: " + coins + "/" + maxCoins;
    }

    void Update()
    {
        isRunning = RunAction.IsPressed() && stamina > 0f && !isExhausted;
        stamina = Mathf.Clamp(stamina, 0f, 100f);

        if (isRunning)
        {
            walkSpeed = 3f;
            stamina -= 50f * Time.deltaTime; //stamina drain
            if(stamina < 1f)
            {
                isExhausted = true;
            }
        }
        else
        {
            walkSpeed = 1f;
            stamina += 35f * Time.deltaTime; //stamina gain
            if (isExhausted == true && stamina > 75f)
            {
                isExhausted = false;
            }
            
        }
    }

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins += 1;
            coinText.text = "Coins: " + coins + "/" + maxCoins;
            audioPlayer.GetComponent<AudioSource>().PlayOneShot(coinSound);

            Destroy(other.gameObject); 
        }
    }

}