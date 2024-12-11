using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
	public Vector2 moveDirection;
	[SerializeField] private Rigidbody2D rb;
	public float speed;
	private Vector2 smoothedPosition;
	[SerializeField] private float smoothTime = 0.9f;

	[SerializeField] private ContactFilter2D movementFilter = new ContactFilter2D();
	private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

	public Vector2Int cellPosition;
	[SerializeField] private float collisionOffset = 0.1f;

	void Start()
	{
		
	
	}
	
	public void UpdateCellPosition(int x, int y)
	{
		cellPosition = new Vector2Int(x, y);
	}
	
	
	public void OnMove(InputAction.CallbackContext ctx)
	{
		moveDirection = ctx.ReadValue<Vector2>();


	}
	
	

	void Update()
	{
		
	

		if(moveDirection.x != 0)
			moveDirection.y = 0;
		
	}

	private void FixedUpdate() {
		

		int count = rb.Cast
		(
			moveDirection, 
			movementFilter,
			castCollisions,
			speed * Time.fixedDeltaTime + collisionOffset
			
		);
		if(count == 0)
		{
			smoothedPosition = Vector2.Lerp(rb.position, rb.position + moveDirection.normalized * speed, smoothTime * Time.deltaTime);
			rb.MovePosition(smoothedPosition);
		
		}
		
	
	}
	
	
	
	
}
