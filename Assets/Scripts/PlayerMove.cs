using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(Animator), typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
	CharacterController characterController;
	Animator animator;

	public float walkSpeed;
	public float runSpeed;

	public InputActionAsset controlDefine;
	InputAction moveAction;

	Vector2 inputValue;
	Vector2 smoothValue;
	Vector2 velocity;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		controlDefine = GetComponent<PlayerInput>().actions;
		moveAction = controlDefine.FindAction("Move");
	}

	private void OnEnable()
	{
		moveAction.performed += OnMoveEvent;
		moveAction.canceled += OnMoveEvent;
	}

	private void OnDisable()
	{
		moveAction.performed -= OnMoveEvent;
		moveAction.canceled -= OnMoveEvent;
	}

	public void OnMoveEvent(InputAction.CallbackContext context)
	{
		inputValue = context.ReadValue<Vector2>();
	}

	private void Update()
	{
		float runValue = Input.GetAxis("Fire3");

		float speed = walkSpeed + runValue * (runSpeed - walkSpeed);
		smoothValue = Vector2.SmoothDamp(smoothValue, inputValue, ref velocity, 0.1f);
		Vector3 inputMoveDir = new Vector3(smoothValue.x, 0, smoothValue.y) * speed;

		Vector3 actualMoveDir = transform.TransformDirection(inputMoveDir);

		characterController.Move(actualMoveDir * Time.deltaTime);

		animator.SetFloat("Xdir", smoothValue.x);
		animator.SetFloat("Ydir", smoothValue.y);
		animator.SetFloat("Speed", smoothValue.magnitude + runValue);
	}
}
