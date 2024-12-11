using UnityEngine;

public class WorldCells : MonoBehaviour
{
	public int positionInListX;
	public int positionInListY;
	private void Update() {
		Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f);
		if(hit != null)
		{
			PlayerMovement playerScript = hit.gameObject.GetComponent<PlayerMovement>();

			if(playerScript != null )
			{
				playerScript.UpdateCellPosition(positionInListX, positionInListY);
			
			}
		}
	}
}
