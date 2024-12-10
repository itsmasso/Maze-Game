using UnityEngine;

public class PathCell : MonoBehaviour
{
	[SerializeField] private bool isPitStop = false;
	[SerializeField] private float pointRadius = 0.5f;
	private bool collidedWithPlayer = false;
	public bool visited;
	public void ChangeToPitstop()
	{
		isPitStop = true;
		visited = false;
		
	}
	void Update()
	{
		if(isPitStop){
			Collider2D hit = Physics2D.OverlapCircle(transform.position, pointRadius);
			if(hit != null)
			{
				PlayerMovement playerScript = hit.gameObject.GetComponent<PlayerMovement>();

				if(!collidedWithPlayer && playerScript != null && Vector2.Distance(hit.gameObject.transform.position, transform.position) < 0.1f)
				{
					playerScript.StopPlayerMovement();
					
					if(!visited)
					{
						playerScript.AddPointToLine(this);
						visited = true;
					}else
					{
						playerScript.DeletePointToLine();
					}
					
					
					collidedWithPlayer = true;
				}
			}
			else
			{
				collidedWithPlayer = false;
			}
		}
	}
}
