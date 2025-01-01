using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;        // Скорость движения
    [SerializeField] private float jumpForce = 5f;        // Сила прыжка
    [SerializeField] private float gravityScale = -9.81f; // Сила гравитации
    [SerializeField] private float runSpeedMultiplier = 2f;  // Множитель скорости бега
    
    private CharacterController controller;                // Компонент CharacterController
    private Vector3 velocity;                             // Вектор скорости
    private bool isGrounded;                              // Проверка на земле ли игрок
    private Animator animator;                            // Добавляем аниматор
    private bool isRunning;                               // Переменная для бега
    public Camera mainCamera;                            // Добавляем ссылку на камеру

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Проверяем, на земле ли игрок
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("IsFalling", false);  // Отключаем анимацию падения при приземлении
        }

        // Получаем входные данные
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Проверяем бег (например, при нажатии левого Shift)
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Создаем вектор движения
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        // Добавляем поворот персонажа в направлении движения
        if (moveX != 0 || moveZ != 0)
        {
            // Получаем целевое направление движения относительно камеры
            Vector3 targetDirection = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(moveX, 0, moveZ);
            // Поворачиваем персонажа плавно к целевому направлению
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime); // Изменено с 10f на 5f
            }
        }

        // Обновляем анимации движения
        bool isMoving = moveX != 0 || moveZ != 0;
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsRunning", isMoving && isRunning);
        
        // Применяем движение с учетом бега
        float currentSpeed = isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravityScale);
            animator.SetTrigger("Jump");  // Активируем триггер прыжка
        }

        // Проверяем падение
        if (!isGrounded && velocity.y < 0)
        {
            animator.SetBool("IsFalling", true);  // Активируем анимацию падения
        }

        // Применяем гравитацию
        velocity.y += gravityScale * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    

}
