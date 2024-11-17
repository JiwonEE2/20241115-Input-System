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

	public InputActionAsset controlDefine;
	InputAction fireAction;
	InputAction reloadAction;
	InputAction grenadeAction;

	private bool isReload = false;
	private bool isGrenade = false;
	private bool isFire = false;

	private bool reloading = false;
	private bool grenadeing = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		controlDefine = GetComponent<PlayerInput>().actions;
		fireAction = controlDefine.FindAction("Fire");
		reloadAction = controlDefine.FindAction("Reload");
		grenadeAction = controlDefine.FindAction("Grenade");
	}

	private void OnEnable()
	{
		fireAction.performed += OnFireEvent;
		fireAction.canceled += OnFireEvent;
		reloadAction.performed += OnReloadEvent;
		reloadAction.canceled += OnReloadEvent;
		grenadeAction.performed += OnGrenadeEvent;
		grenadeAction.canceled += OnGrenadeEvent;
	}

	private void OnDisable()
	{
		fireAction.performed -= OnFireEvent;
		fireAction.canceled -= OnFireEvent;
		reloadAction.performed -= OnReloadEvent;
		reloadAction.canceled -= OnReloadEvent;
		grenadeAction.performed -= OnGrenadeEvent;
		grenadeAction.canceled -= OnGrenadeEvent;
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

	public void OnReloadEvent(InputAction.CallbackContext context)
	{
		if (context.ReadValue<float>() > 0)
		{
			isReload = true;
		}
		else
		{
			isReload = false;
		}
	}

	public void OnGrenadeEvent(InputAction.CallbackContext context)
	{
		if (context.ReadValue<float>() > 0)
		{
			isGrenade = true;
		}
		else
		{
			isGrenade = false;
		}
	}


	private void Start()
	{
		untilReload = new WaitUntil(() => reloading);
		untilGrenade = new WaitUntil(() => grenadeing);
		StartCoroutine(ReloadCoroutine());
		StartCoroutine(GrenadeCoroutine());
	}

	private void Update()
	{
		if (isReload && isGrenade == false && isFire == false && reloading == false)
		{
			reloading = true;
			animator.SetTrigger("Reload");
		}
		if (isReload == false && isGrenade && isFire == false && grenadeing == false)
		{
			grenadeing = true;
			animator.SetTrigger("Grenade");
		}
		if (isReload == false && isGrenade == false && isFire)
		{
			animator.SetTrigger("Fire");
		}
	}

	IEnumerator ReloadCoroutine()
	{
		while (true)
		{
			yield return untilReload;
			yield return new WaitForSeconds(reloadClip.length);
			reloading = false;
		}
	}

	IEnumerator GrenadeCoroutine()
	{
		while (true)
		{
			yield return untilGrenade;
			yield return new WaitForSeconds(grenadeClip.length);
			grenadeing = false;
		}
	}
}
