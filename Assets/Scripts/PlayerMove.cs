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
		Vector3 inputMoveDir = new Vector3(inputValue.x, 0, inputValue.y) * (walkSpeed + runValue * (runSpeed - walkSpeed));
		Vector3 actualMoveDir = transform.TransformDirection(inputMoveDir);

		characterController.Move(actualMoveDir * Time.deltaTime);

		animator.SetFloat("Xdir", inputValue.x);
		animator.SetFloat("Ydir", inputValue.y);
		animator.SetFloat("Speed", inputValue.magnitude + runValue);
	}
}
