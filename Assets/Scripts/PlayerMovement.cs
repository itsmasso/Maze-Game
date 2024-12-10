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
	private bool isMoving;
	[SerializeField] private LayerMask wallLayer;
	private List<Vector3> points = new List<Vector3>();
	[SerializeField] private LineRenderer lineRenderer;

	private List<PathCell> paths = new List<PathCell>();

	private bool canPressMove;
	void Start()
	{

		points.Add(transform.position);

		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(points.ToArray());
		
		canPressMove = true;
	}
	
	public void AddPointToLine(PathCell path)
	{
		paths.Add(path);
		points.Add(path.gameObject.transform.position);
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(points.ToArray());
		
	}
	
	public void DeletePointToLine()
	{
		PathCell pathToDelete = paths[paths.Count-1];
		pathToDelete.visited = false;
		points.Remove(pathToDelete.gameObject.transform.position);
		
		paths.Remove(pathToDelete);
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(points.ToArray());
	}
	
	public void StopPlayerMovement()
	{
		
		moveDirection = Vector2.zero;
	}
	
	private IEnumerator MoveCooldown()
	{
		canPressMove = false;
		yield return new WaitForSeconds(0.1f);
		canPressMove = true;
	}
	
	public void OnMove(InputAction.CallbackContext ctx)
	{
		if(ctx.performed && !isMoving && canPressMove)
		{
			RaycastHit2D hit = Physics2D.Raycast(rb.position, ctx.ReadValue<Vector2>(), 1.5f, wallLayer);
			if(hit.collider == null)
			{
				moveDirection = ctx.ReadValue<Vector2>();
			}else
			{
				//Debug.Log("Blocked by: " + hit.collider.name);
			}
			StartCoroutine(MoveCooldown());

		}

	}
	
	

	void Update()
	{
		if(moveDirection.x != 0)
			moveDirection.y = 0;
		
		if(moveDirection != Vector2.zero)
			isMoving = true;
		else
			isMoving = false;
		
	}

	private void FixedUpdate() {
		
		smoothedPosition = Vector2.Lerp(rb.position, rb.position + moveDirection.normalized * speed, smoothTime * Time.deltaTime);
		rb.MovePosition(smoothedPosition);
		
	
	}
	
	
	
	
}
