using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DragObjecController))]
public class RayLighting : MonoBehaviour {

    public LayerMask playerMask;
    public float accuracy;
    [Range(0, 360)]
    public float Direction = 180;
    [Range(5, 165)]
    public int lightAngle = 45;
    [Range(4, 20)]
    public float distance = 10;
    [Range(0, 5)]
    public float RotationSpeed = 0;
    [Range(0, 360)]
    public float rotationRangeRight = 135;
    [Range(0, 360)]
    public float rotationRangeLeft = 225;
    public bool debugLogs;
    public bool debugLines;
    public bool dragable;

    [HideInInspector]
    public DragObjecController dragController;

    private Vector3[] endpoints;
	private Vector3[] m_Vertices;
	private int[] m_Tris;
    private Ray centerRay;
    private float tempDirection;
    private float tempAccuracy;
	private int tempLightAngle;
	private Vector3 tempUp;
	private Vector3 tempPosition;
	private MeshFilter mf;
	private Ray[] rays;
	Vector3 startDirection;

	void Start () {
		initializeRayValues ();
		tempDirection = Direction;
		tempAccuracy = accuracy;
		tempLightAngle = lightAngle;
		tempUp = transform.up;
		tempPosition = transform.position;

		mf = GetComponent<MeshFilter>();
        dragController = GetComponent<DragObjecController>();
	}

	void initializeRayValues()
	{
		centerRay = new Ray(transform.position, Quaternion.Euler(0, 0,  -Direction ) * Vector3.up);
		tempDirection = Direction;
		int numRays = Mathf.CeilToInt(lightAngle / accuracy);

		rays = new Ray[numRays];
		float startAngle = -lightAngle / 2;
		startDirection = Quaternion.AngleAxis (startAngle, Vector3.back) * centerRay.direction;

		Vector3 curDirection = startDirection;
		for(int i = 0; i < rays.Length; i++)
		{
			rays[i] = new Ray(transform.position, curDirection);
			curDirection = Quaternion.AngleAxis(accuracy, Vector3.back) * curDirection;
		}

		//initialize mesh arrays
		m_Vertices = new Vector3[rays.Length + 1];
		m_Tris = new int[3 * (rays.Length - 1)];
	}
	
	void Update () {
        if (dragable)
        {
            dragController.Move();
        }
		if (tempAccuracy != accuracy || tempLightAngle != lightAngle) 
		{
			if(accuracy > 0 && lightAngle > 0)
			{
				tempAccuracy = accuracy;
				tempLightAngle = lightAngle;
				initializeRayValues ();
			}
		}
		if (tempPosition != transform.position)
		{
			OnPositionChange ();
		}
		UpdateRays();
	}

	void FixedUpdate()
	{
		SwingLight ();
		if (tempDirection != Direction) 
		{
			Quaternion rotation = Quaternion.Euler(0, 0, tempDirection - Direction);
			OnRotationChange (rotation);
			tempDirection = Direction;
		}
	}

	private void SwingLight()
	{
		if (rotationRangeRight != rotationRangeLeft && RotationSpeed != 0) {

			if(RotationSpeed != 0)
			{
				Direction += RotationSpeed;
				if (RotationSpeed > 0) {
					if (Direction > rotationRangeLeft) {
						Direction = rotationRangeLeft;
						RotationSpeed = -RotationSpeed;
					}
				} else if (RotationSpeed < 0) {
					if (Direction < rotationRangeRight) {
						Direction = rotationRangeRight;
						RotationSpeed = -RotationSpeed;
					}
				}
			}
		}
	}

	void UpdateRays(){
		Vector3 curDirection = startDirection;
		m_Vertices [0] = new Vector3(0,0,0);
		Vector3 endpoint;
		for(int i = 0; i < rays.Length; i++)
		{
			RaycastHit2D hit = Physics2D.Raycast (rays[i].origin, rays[i].direction, distance);
			RaycastHit2D playerHit = Physics2D.Raycast (rays[i].origin, rays[i].direction, distance, playerMask);
            if (playerHit && (!hit || hit.distance >= playerHit.distance))
            {
                Spawn spawner = playerHit.collider.transform.GetComponent<Spawn>();
                spawner.PlayerSeen();
            }
            if (hit.collider != null)
			{
				if(debugLines)
					Debug.DrawLine(rays[i].origin, hit.point, Color.green);
                
				endpoint = hit.point;
				m_Vertices[i+1] = endpoint - transform.position;
			}else
			{
				Vector3 tempVec = rays[i].direction;
				tempVec.Set(rays[i].direction.x * distance, rays[i].direction.y * distance, 0);
				endpoint = rays[i].origin + tempVec;

				if(debugLines)
					Debug.DrawLine(rays[i].origin, endpoint, Color.red);

				m_Vertices[i+1] = endpoint - transform.position;
			}

			if(i > 0)
			{
				int startTriangleIndex = 3*(i-1);
				for(int j = 0; j < 3; j++)
				{
					m_Tris[startTriangleIndex] = i;
					m_Tris[startTriangleIndex+1] = i+1;
					m_Tris[startTriangleIndex+2] = 0;
				}
			}
		}
		mf.mesh.Clear();
		mf.mesh.vertices = m_Vertices;
		mf.mesh.triangles = m_Tris;
	}

	void PrintMeshInfo()
	{
		if (debugLogs) 
		{
			Debug.Log ("Vertices ( " + m_Vertices.Length + "): ");
			for(int i = 0; i < m_Vertices.Length; i++)
			{
				Debug.Log("\t Vertex " + i + ": (" + m_Vertices[i].x + ", " + m_Vertices[i].y + ", " + m_Vertices[i].z + ")");
			}

			Debug.Log ("Triangles (" + m_Tris.Length + "): ");
			for(int i = 0; i < m_Tris.Length; i=i+3)
			{
				Debug.Log("\t " + m_Tris[i] + ", " + m_Tris[i+1] + ", " + m_Tris[i+2]);
			}
		}
	}

	void OnPositionChange()
	{
		centerRay.origin = transform.position;
		for (int i = 0; i < rays.Length; i++)
		{
			rays[i].origin = transform.position;
		}
	}

	void OnRotationChange(Quaternion rotationDif)
	{
		centerRay.direction = rotationDif * centerRay.direction;
		for (int i = 0; i < rays.Length; i++)
		{
			rays[i].direction = rotationDif * rays[i].direction;
		}
	}

	void printRayLog(Ray ray, Vector3 endpoint, string title){
		Debug.Log (title + " Ray, origin: " + ray.origin + ", endpoint: " + endpoint.x + ", " + endpoint.y + ", " + endpoint.z);
	}

    void OnDrawGizmos()
    {
        float startAngle = -lightAngle / 2;
        Vector3 center = Quaternion.Euler(0, 0, -Direction) * new Vector3(0f, distance) + transform.position;
        Vector3 right = Quaternion.Euler(0, 0, -Direction + startAngle) * new Vector3(0f, distance) + transform.position;
        Vector3 left = Quaternion.Euler(0, 0, -Direction - startAngle) * new Vector3(0f, distance) + transform.position;
        Vector3 rightRange = Quaternion.Euler(0, 0, -rotationRangeRight - startAngle) * new Vector3(0f, distance) + transform.position;
        Vector3 leftRange = Quaternion.Euler(0, 0, -rotationRangeLeft + startAngle) * new Vector3(0f, distance) + transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, center);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, right);
        Gizmos.DrawLine(transform.position, left);
        Gizmos.color = ValidRange() ? Color.blue : Color.red;
        Gizmos.DrawLine(transform.position, rightRange);
        Gizmos.DrawLine(transform.position, leftRange);
    }
    private bool ValidRange()
    {
        return !(rotationRangeRight > rotationRangeLeft)
            && !(Direction > rotationRangeLeft || Direction < rotationRangeRight);
    }
}
