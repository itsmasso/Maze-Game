using UnityEngine;

public class PlayerLineRenderer : MonoBehaviour
{
	[SerializeField] private LineRenderer line;
	private Transform[] points;
	void Start()
	{
		
	}

	private void SetUpLine(Transform[] points){
		line.positionCount = points.Length;
		this.points = points;
	}
	void Update()
	{
		
	}
}
