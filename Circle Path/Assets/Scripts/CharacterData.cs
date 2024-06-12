using System;
using Sonity;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterData : MonoBehaviour
{
	public float sinceGroundedMultiplierWhenWallGrab = 0.2f;

	public PlayerActions playerActions;

	public ParticleSystem[] landParts;

	public int jumps = 1;

	public int currentJumps = 1;

	public bool isPlaying;
	public bool dead;
	public bool canMove;
	public bool isStunned;
	
	public AnimationCurve slamCurve;

	public Vector3 wallPos;

	public Vector2 wallNormal;

	public Vector3 groundPos;
	
	public float sinceWallGrab = float.PositiveInfinity;

	public bool isWallGrab;

	public float wallDistance = 1f;

	private bool wasWallGrabLastFrame;

	public float sinceGrounded;

	public bool isGrounded = true;

	private bool wasGroundedLastFrame = true;

	public float sinceJump = 1f;

	public PlayerVelocity playerVel;

	public GeneralInput input;

	public PlayerMovement movement;

	public PlayerJump jump;

	public CharacterStatModifier stats;

	public Collider2D mainCol;

	//public PlayerSounds playerSounds;

	private Transform wobblePos;

	private LayerMask groundMask;

	public Rigidbody2D standOnRig;

	public Animator anim;

	public Action<float, Vector3, Vector3, Transform> TouchGroundAction;

	private void Awake()
	{
		playerActions = PlayerActions.CreateWithKeyboardBindings();
		mainCol = GetComponent<Collider2D>();
		stats = GetComponent<CharacterStatModifier>();
		input = GetComponent<GeneralInput>();
		movement = GetComponent<PlayerMovement>();
		jump = GetComponent<PlayerJump>();
		playerVel = GetComponent<PlayerVelocity>();
		anim = GetComponentInChildren<Animator>();
		//playerSounds = GetComponent<PlayerSounds>();
	}

	private void Start()
	{
		groundMask = LayerMask.GetMask("Default");
	}

	private void Update()
	{
		if (!playerVel.rb.simulated || !canMove)
		{
			sinceGrounded = 0f;
			return; // Если персонаж не может двигаться, прекращаем обработку ввода и других действий
		}
		sinceJump += Time.deltaTime;
	}

	private void FixedUpdate()
	{
		Ground();
	}

	private void Ground()
	{
		if (!isPlaying)
		{
			return;
		}
		if (!isGrounded)
		{
			sinceGrounded += Time.deltaTime * ((isWallGrab && wallDistance < 0.7f) ? sinceGroundedMultiplierWhenWallGrab : 1f);
			if (sinceGrounded < 0f)
			{
				sinceGrounded = Mathf.Lerp(sinceGrounded, 0f, Time.deltaTime * 15f);
			}
		}
		if (!wasGroundedLastFrame)
		{
			isGrounded = false;
		}
		wasGroundedLastFrame = false;
	}

	public void TouchGround(Vector3 pos, Vector3 groundNormal, Rigidbody2D groundRig, Transform groundTransform = null)
	{
		if (sinceJump > 0.2f)
		{
			currentJumps = jumps;
		}
		if (TouchGroundAction != null)
		{
			TouchGroundAction(sinceGrounded, pos, groundNormal, groundTransform);
		}
		standOnRig = groundRig;
		if (playerVel.rb.velocity.y < -10f)
		{
			for (int i = 0; i < landParts.Length; i++)
			{
				landParts[i].transform.localScale = Vector3.one * Mathf.Clamp((0f - playerVel.rb.velocity.y) / 40f, 0.5f, 2f) * 0.5f;
				landParts[i].transform.position = new Vector3(transform.position.x + playerVel.rb.velocity.x * 0.03f, pos.y, 5f);
				landParts[i].transform.rotation = Quaternion.LookRotation(groundNormal);
				landParts[i].Play();
			}
		}
		groundPos = pos;
		wasGroundedLastFrame = true;
		isGrounded = true;
		sinceGrounded = 0f;
	}
 
	public bool ThereIsGroundBelow(Vector3 pos, float range = 5f)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.down, range, groundMask);
		if ((bool)raycastHit2D.transform && raycastHit2D.distance > 0.1f)
		{
			return true;
		}
		return false;
	}

	public void SetWobbleObjectChild(Transform obj)
	{
		obj.transform.SetParent(wobblePos, true);
	}

	public void Die()
	{
		//diePart.transform.position = transform.position;
		//diePart.Play();
		//SoundManager.Instance.Play(dieSound, transform);
	}

	public void ResetVelocity()
	{
		playerVel.rb.velocity = new Vector2(0, 0);
		playerVel.rb.angularVelocity = 0f;
		canMove = true;
	}
}
