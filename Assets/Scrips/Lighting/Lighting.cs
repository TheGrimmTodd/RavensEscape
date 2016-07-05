using UnityEngine;
using System.Collections;

public class Lighting : MonoBehaviour {

	private Mesh m_Mesh = null;
	public double theta = 30.0 * System.Math.PI / 180.0;
	public double Dtheta = 1.0 * System.Math.PI / 180.0;
	private int factor = 3;
	public float dis = 10.0f;
	private Vector3 lightBorderL;
	private Vector3 lightBorderR;
	private int foreground;
	private Vector3 src;

	// Use this for initialization
	void Start () {

		foreground = 1 << LayerMask.NameToLayer ("foreground");

		m_Mesh = new Mesh();
		Vector3[] vertices = new Vector3[68 * factor];
		int[] tris = new int[3*(vertices.Length - 2)];
		vertices [0] = new Vector3(0,0,0);
		src = this.transform.position;
		lightBorderL = new Vector3((float)(src.x + 0.5f * System.Math.Sin(theta) * dis),
		                             (float)( src.y + System.Math.Cos(0.5f * theta) * dis),
		                             src.z)-src;
		lightBorderR = new Vector3((float)(src.x - 0.5f * System.Math.Sin(theta) * dis),
		                           (float)(src.y + System.Math.Cos(0.5f * theta) * dis),
		                           src.z)-src;

		int t = 1;
		for(int i = 0; i < tris.Length; i++){
			int index;
			switch(i % 3){
				case 0:
					index = 0;
					break;
				case 1:
					index = t;
					break;
				case 2:
				default:
					t++;
					index = t;
					break;
			}
			tris[i] = index;
		}

		m_Mesh.vertices = vertices;
		m_Mesh.triangles = tris;
		MeshFilter filter = GetComponent<MeshFilter>();
		filter.mesh = m_Mesh;
	}

	Vector3 rotate (Vector3 p, Vector3 o, double dtheta){
		float px = (float)(System.Math.Cos (dtheta) * (p.x - o.x) - System.Math.Sin (dtheta) * (p.y - o.y) + o.x);
		float py = (float)(System.Math.Sin (dtheta) * (p.x - o.x) + System.Math.Cos (dtheta) * (p.y - o.y) + o.y);
		return new Vector3 (px, py, p.z);
	}
	
	void swingLight (Vector3[] vertices)
	{
		lightBorderL = rotate (lightBorderL, vertices [0],Dtheta);
		lightBorderR = rotate (lightBorderR, vertices [0],Dtheta);
		if (lightBorderL.y >= 0 && Dtheta < 0) {
			Dtheta = -Dtheta;
		}
		if (lightBorderR.y >= 0 && Dtheta > 0) {
			Dtheta = -Dtheta;
		}
	}
	Vector3 copyConstructor(Vector3 toCopy){
		Vector3 copy = new Vector3 ();
		copy.x = toCopy.x;
		copy.y = toCopy.y;
		copy.z = toCopy.z;
		return copy;
	}
	void UpdateMesh()
	{

		m_Mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = m_Mesh.vertices;
		swingLight (vertices);
		int i;
		Vector3 direction = copyConstructor(lightBorderL);
		bool ravenSeen = false;
		for(i = 1; i < vertices.Length; i ++){
			RaycastHit2D groundHit = Physics2D.Raycast (src, direction,dis-1f,foreground,-1.0f,1.0f);
			RaycastHit2D ravenHit = Physics2D.Raycast (src, direction,dis-1f,1 << LayerMask.NameToLayer("Raven"),-1.0f,1.0f);
			vertices[i] = groundHit.collider != null && groundHit.distance <= dis? new Vector3(groundHit.point.x,groundHit.point.y,src.z) - src : direction;
			float gdis = groundHit.collider != null? groundHit.distance : dis;
			ravenSeen = ravenSeen || (ravenHit.collider != null && gdis > ravenHit.distance);
			direction = rotate(direction,vertices[0], 1.0/(factor) * System.Math.PI / 180.0);
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
		UpdateMesh();
	}

	void ravenHasBeenSeen(){
		//TODO: whap happens here
		LevelSignalSender.Instance.signalLightTouched ();

	}

}
