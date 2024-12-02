using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;        // Скорость движения
    [SerializeField] private float jumpForce = 5f;        // Сила прыжка
    [SerializeField] private float gravityScale = -9.81f; // Сила гравитации
    
    private CharacterController controller;                // Компонент CharacterController
    private Vector3 velocity;                             // Вектор скорости
    private bool isGrounded;                              // Проверка на земле ли игрок
    private Animator animator;                            // Добавляем аниматор

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();                // Получаем компонент аниматора
    }

    void Update()
    {
        // Проверяем, на земле ли игрок
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Получаем входные данные
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Создаем вектор движения
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        // Обновляем анимации
        bool isMoving = moveX != 0 || moveZ != 0;
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("Speed", move.magnitude);
        
        // Применяем движение
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravityScale);
        }

        // Применяем гравитацию
        velocity.y += gravityScale * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
