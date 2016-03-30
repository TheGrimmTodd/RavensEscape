using UnityEngine;
using System.Collections;

public class SquareLighting : MonoBehaviour {


	private Mesh m_Mesh = null;
	private int factor = 3;
	public float dis = 10.0f;
	public float width = 6f;
	private int density = 50;
	private ArrayList recs;

	private int foreground;
	private Vector3 src;



		
	// Use this for initialization
	void Start () {

		foreground = 1 << LayerMask.NameToLayer ("foreground");
		m_Mesh = new Mesh();

		for (int i =0; i < width * density; i++) {
			int iw = width/density;
			recs.Add(new Square(new Vector3(iw * i - width/2 ,0,0),iw,dis))
		}
		Vector3[] vertices = new Vector3[recs.Count * 6];
		int[] tris = new int[3*(vertices.Length)];



		vertices [0] = ((Square)recs[0]).GetOrigin();
		int t = 1;
		for(int i = 0; i < tris.Length; i++)
			tris[i] = i;
			
		m_Mesh.vertices = vertices;
		m_Mesh.triangles = tris;
		MeshFilter filter = GetComponent<MeshFilter>();
		filter.mesh = m_Mesh;
	}

	static Vector3 copyConstructor(Vector3 toCopy){
		Vector3 copy = new Vector3 ();
		copy.x = toCopy.x;
		copy.y = toCopy.y;
		copy.z = toCopy.z;
		return copy;
	}

	void UpdateMeshNew()
	{
		m_Mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = m_Mesh.vertices;
		int i;
		Vector3 direction,src;
		bool ravenSeen = false;


		for (i = 0; i< recs.Count; i=i++) {
			Square cur = (Square)recs[i];
			direction = copyConstructor(cur.GetDirection());
			src = copyConstructor(cur.GetOrigin());

			RaycastHit2D groundHit = Physics2D.Raycast (src, direction,dis-1f,foreground,-1.0f,1.0f);
			RaycastHit2D ravenHit = Physics2D.Raycast (src, direction,dis-1f,1 << LayerMask.NameToLayer("Raven"),-1.0f,1.0f);
			Vector3 hit = groundHit.collider != null && groundHit.distance <= dis? new Vector3(groundHit.point.x,groundHit.point.y,src.z) - src : direction;
			cur.SetDistance(hit.y);



		}

	}



	void UpdateMesh()
	{

		m_Mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = m_Mesh.vertices;
		int i;
		Vector3 direction = copyConstructor(borders[1]);
		bool ravenSeen = false;
		for(i = 1; i < vertices.Length; i ++){
			RaycastHit2D groundHit = Physics2D.Raycast (src, direction,dis-1f,foreground,-1.0f,1.0f);
			RaycastHit2D ravenHit = Physics2D.Raycast (src, direction,dis-1f,1 << LayerMask.NameToLayer("Raven"),-1.0f,1.0f);
			vertices[i] = groundHit.collider != null && groundHit.distance <= dis? new Vector3(groundHit.point.x,groundHit.point.y,src.z) - src : direction;
			float gdis = groundHit.collider != null? groundHit.distance : dis;
			ravenSeen = ravenSeen || (ravenHit.collider != null && gdis > ravenHit.distance);
//			direction = rotate(direction,vertices[0], 1.0/(factor) * System.Math.PI / 180.0);
		}
			
		if (ravenSeen) {
			ravenHasBeenSeen();
		}else{
			//todo: may want to do something here
		}
			
		m_Mesh.vertices = vertices;
		m_Mesh.RecalculateBounds();
		m_Mesh.RecalculateNormals();
	}
		
		
		
	void Update ()
	{
		UpdateMeshNew();
	}
		
	void ravenHasBeenSeen(){
		//TODO: whap happens here
		LevelSignalSender.Instance.signalLightTouched ();
			
	}

	private class Square{
		Vector3 topLeft;
		Vector3 topRight;
		Vector3 bottomLeft;
		Vector4 bottomRight;

		Square(Vector3 topLeft, float width, float hight){
			this.topLeft = topLeft;
			this.topRight = new Vector3(topLeft.x + width,topLeft.y,topLeft.z);
			this.bottomLeft = new Vector3(topLeft.x,topLeft.y + hight,topLeft.z);
			this.bottomRight = new Vector3(topLeft.x + width,topLeft.y + hight,topLeft.z);
		}

//		0, 1, 2,
//		2, 3, 0

		public Vector3 [] drawVerts(){
			return new Vector3[]{
				copyConstructor(topLeft),
				copyConstructor(bottomLeft),
				copyConstructor(bottomRight),

				copyConstructor(bottomRight),
				copyConstructor(topRight),
				copyConstructor(topLeft)
			};

		}
		public Vector3 GetOrigin(){
			return topLeft;
		}
		public Vector3 GetDirection(){
			return bottomLeft;
		}

		public void SetDistance(int y){
			bottomLeft.y = y;
			bottomRight.y = y;
		}


	}
}


