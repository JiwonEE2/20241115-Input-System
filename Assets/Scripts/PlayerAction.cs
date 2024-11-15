using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAction : MonoBehaviour
{
	Animator animator;

	private WaitUntil untilReload;
	private WaitUntil untilGrenade;

	public AnimationClip reloadClip;
	public AnimationClip grenadeClip;

	private bool isReloading = false;
	private bool isGrenade = false;

	public InputActionAsset controlDefine;
	InputAction fireAction;
	bool isFire;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		controlDefine = GetComponent<PlayerInput>().actions;
		fireAction = controlDefine.FindAction("Fire");
	}

	private void OnEnable()
	{
		fireAction.performed += OnFireEvent;
		fireAction.canceled += OnFireEvent;
	}

	private void OnDisable()
	{
		fireAction.performed -= OnFireEvent;
		fireAction.canceled -= OnFireEvent;
	}

	public void OnFireEvent(InputAction.CallbackContext context)
	{
		if (context.ReadValue<float>() > 0)
		{
			isFire = true;
		}
		else
		{
			isFire = false;
		}
	}

	private void Start()
	{
		untilReload = new WaitUntil(() => isReloading);
		untilGrenade = new WaitUntil(() => isGrenade);
		StartCoroutine(ReloadCoroutine());
		StartCoroutine(GrenadeCoroutine());
	}

	private void Update()
	{
		if (isReloading == false && isGrenade == false && Input.GetKeyDown(KeyCode.R))
		{
			isReloading = true;
			animator.SetTrigger("Reload");
		}
		if (isReloading == false && isGrenade == false && Input.GetKeyDown(KeyCode.F))
		{
			isGrenade = true;
			animator.SetTrigger("Grenade");
		}
		if (isFire && isReloading == false && isGrenade == false) animator.SetTrigger("Fire");
	}

	IEnumerator ReloadCoroutine()
	{
		while (true)
		{
			yield return untilReload;
			yield return new WaitForSeconds(reloadClip.length);
			isReloading = false;
		}
	}

	IEnumerator GrenadeCoroutine()
	{
		while (true)
		{
			yield return untilGrenade;
			yield return new WaitForSeconds(grenadeClip.length);
			isGrenade = false;
		}
	}
}
