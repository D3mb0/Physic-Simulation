using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCloth
{
	public List<CNode> mNodeArray = new List<CNode>();
	public List<CNode> mPreviousNodeArray;
	public List<CSpring> mSpringArray;
	public List<Vector3> mFaceList;
	public int[] triangles;

	public float mOffset;
	public int clothID;
	//public int mSpringCount;
	//public int mNodeCount;

	public float xPos;
	public float yPos;

	//public int Structural_Spring_Count;
	//public int Shear_Spring_Count;
	//public int bend_Spring_Count;
	public int sFlag;   // 1: Reference Model, 2: simulation Model

	public float lineSize = 16.0f;
	public float IntervalX;
	public float IntervalY;

	public float mStrKS = 100f;
	public float mStrKD = 0.1f;

	public float mShearKS = 100f;
	public float mShearKD = 0.1f;

	public float mBendKS = 100f;
	public float mBendKD = 0.1f;

	int totalMass = 100; // Object 전체의 질량
	//float dt = 0.0008f;
	float dt ;

	int curr_Frame = 0;

	//int rendering_mode = 0; // 0: triangle, 1: wireframe

	public CCloth(int n,int m, float offset, int id, int flag,float dt, float lineSize,float ks, float kd)
	{
		mStrKS = ks;
		mShearKS = ks;
		mBendKS = ks;

		mStrKD = kd;
		mShearKD = kd;
		mBendKD = kd;

		Debug.Log("KS :" + ks + " KD :" + kd);

		mPreviousNodeArray = new List<CNode>();
		mPreviousNodeArray.Clear();

		mSpringArray = new List<CSpring>();
		mSpringArray.Clear();

		mFaceList = new List<Vector3>();
		mFaceList.Clear();

		sFlag = flag;
		clothID = id;

		IntervalX = lineSize / ((float)n - 1.0f);
		IntervalY = lineSize / ((float)m - 1.0f);

		mOffset = offset;
		initialize(n, m);
		this.dt = 0.008f;
		this.lineSize = lineSize;
	}
	public void initialize(int n, int m)
	{
		//float newX = -4.0f;
		//float newY = 4.0f;
		//float newZ = 0.0f;

		float newX = 0.0f;
		float newY = 0.0f;
		float newZ = 0.0f;

		// Node position set up: n by m
		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < m; j++)
			{
				CNode node = new CNode();
				node.mPosition.x = newX;
				node.mPosition.y = newY;
				node.mPosition.z = newZ;
				mNodeArray.Add(node);
				//Debug.Log("Node["+i+"],["+j+"] :"+node.mPosition);
				newX += IntervalX;

				
			}
			newZ -= IntervalY;
			newX = 0f;
		}

		for (int i = 0; i < mNodeArray.Count; i++)
		{
			//mNodeArray.at(i).mMass = (float)totalMass / mNodeArray.size();
			mNodeArray[i].mMass = (float) totalMass / mNodeArray.Count;
		}


		//constrain node 
		//mNodeArray[0].SimulationFlag = false;
		//mNodeArray[m-1].SimulationFlag = false;

		Debug.Log("#" + clothID + "Cloth Node Initialization Success... "+mNodeArray.Count+" Node");

		//int Spring_index = 0;
		int index = 0;

		// Structural Spring Connection: horizontal
		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < (n - 1); j++)
			{
				if (i > 0 && j == 0) index++;
				CSpring spring = new CSpring();
				spring.Kd = mStrKD;
				spring.Ks = mStrKS;
				spring.type = 0;
				spring.init(mNodeArray[index], mNodeArray[index+1]);
				index++;
				mSpringArray.Add(spring);
			}
		}
		//Debug.Log("Horizontal Spring count :" + mSpringArray.Count);
		// Structural Spring Connection: vertical
		for (int i = 0; i < (m - 1); i++)
		{
			for (int j = 0; j < n; j++)
			{
				++index;
				CSpring spring = new CSpring();
				spring.Kd = mStrKD;
				spring.Ks = mStrKS;
				spring.type = 0;
				spring.init(mNodeArray[(n) * i + j], mNodeArray[(n) * i + j + n]);
				mSpringArray.Add(spring);
			}
		}
		//Structural_Spring_Count = mSpringArray.Count;
		//shear Spring Connection: Left right
		int pointindex = 0;
		for (int i = 0; i < (n) * (m - 1); i++)
		{
			if (i % n == (n - 1))
			{
				pointindex++;
				continue;
			}

			CSpring spring = new CSpring();
			spring.Kd = mShearKD;
			spring.Ks = mShearKS;
			spring.type = 1;
			spring.init(mNodeArray[pointindex], mNodeArray[pointindex + n + 1]);
			mSpringArray.Add(spring);
			pointindex++;
		}
		//shear Spring Connection:  Top right bottom
		pointindex = 0;
		for (int i = 0; i < (n) * (m - 1); i++)
		{
			if (i % n == (0))
			{
				pointindex++;
				continue;
			}

			CSpring spring = new CSpring();
			spring.Kd = mShearKD;
			spring.Ks = mShearKS;
			spring.type = 1;
			spring.init(mNodeArray[pointindex], mNodeArray[pointindex + n - 1]);
			mSpringArray.Add(spring);
			pointindex++;
		}

		// Bend Spring Connection: horizontal
		pointindex = 0;
		for (int i = 0; i < (n) * m; i++)
		{
			if (i % n > n - 3)
			{
				pointindex++;
				continue;
			}

			CSpring spring = new CSpring();
			spring.Kd = mBendKD;
			spring.Ks = mBendKS;
			spring.type = 2;
			spring.init(mNodeArray[pointindex], mNodeArray[pointindex + 2]);
			mSpringArray.Add(spring);
			pointindex++;
		}

		// Bend Spring Connection: vertical
		pointindex = 0;
		for (int i = 0; i < (n) * (m - 2); i++)
		{
			if (i % n > n - 3)
			{
				pointindex++;
				continue;
			}

			CSpring spring = new CSpring();
			spring.Kd = mBendKD;
			spring.Ks = mBendKS;
			spring.type = 2;
			spring.init(mNodeArray[pointindex], mNodeArray[pointindex + 2 * m]);
			mSpringArray.Add(spring);
			pointindex++;
		}
		
		Debug.Log("#" + clothID + "Cloth Spring Initialization Success...");
		//Debug.Log("Spring Count : " + mSpringArray.Count);


		//int Face_count = 0;
		//// Face List Setup
		//for (int i = 0; i < m -1; ++i)
		//{
		//	for (int j = 0; j < n-1 ; ++j)
		//	{
		//		int v1 = (i * n) + j;
		//		int v2 = (i * n) + j + 1;
		//		int v3 = (i * n) + n + j;
		//		int v4 = (i * n) + n + j + 1;

		//		mFaceList.Add(new Vector3(v1, v3, v2));
		//		mNodeArray[v1].faceCount++;
		//		mNodeArray[v2].faceCount++;
		//		mNodeArray[v3].faceCount++;
		//		mFaceList.Add(new Vector3(v2, v3, v4));
		//		mNodeArray[v2].faceCount++;
		//		mNodeArray[v3].faceCount++;
		//		mNodeArray[v4].faceCount++;
		//		Face_count++;
		//	}
		// Face List Setup
		triangles = new int[m * n * 6];
		for (int ti = 0, vi = 0, y = 0; y < m; y++, vi++)
		{
			for (int x = 0; x < n; x++, ti += 6, vi++)
			{
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + n + 1;
				triangles[ti + 5] = vi + n + 2;
			}
		}
		//Debug.Log(triangles[1535]);
		//for (int i = 0; i < triangles.Length; i++)
		//{
		//	Debug.Log("Triangle vertice :" + triangles[i]);
		//}



	}
	public void update()
	{
		if (curr_Frame < 33000)
		{
			if (clothID == 1)
			{
				// Clear Node Force
				for (int i = 0; i < mNodeArray.Count; i++)
				{
					mNodeArray[i].ClearForce();
				}
				// Compute Spring Force
				for (int i = 0; i < mSpringArray.Count; i++)
				{
					mSpringArray[i].update();
				}
				// Simulation Integration
				for (int i = 0; i < mNodeArray.Count; i++)
				{			
					mNodeArray[i].update(dt);
					//Debug.Log(mNodeArray[i].mPosition);
				}

			}
		}
		curr_Frame++;
	}
	public void calculateNormalVector(Vector3 tmp)
	{
		Vector3 F1 = Vector3.Cross(mNodeArray[(int)tmp.x].mPosition, mNodeArray[(int)tmp.y].mPosition);
		Vector3 F2 = Vector3.Cross(mNodeArray[(int)tmp.x].mPosition, mNodeArray[(int)tmp.z].mPosition);
		Vector3 F3 = Vector3.Cross(mNodeArray[(int)tmp.y].mPosition, mNodeArray[(int)tmp.z].mPosition);

		Vector3 Summation = F1 + F2 + F3;

		mNodeArray[(int)tmp.x].Normal += Summation;
		mNodeArray[(int)tmp.y].Normal += Summation;
		mNodeArray[(int)tmp.z].Normal += Summation;
		//Summation.N

	}
	public void render()
	{
		wireframeRendering();
	}
	private void wireframeRendering()
	{
		Gizmos.color = Color.black;
		for (int i = 0; i < mSpringArray.Count; i++)
		{
			////draw wireframe
			if (mSpringArray[i].type == 2) continue;
			Gizmos.DrawLine(mSpringArray[i].m1.mPosition, mSpringArray[i].m2.mPosition);
		}
	}
	private void NodeRendering()
	{
		Gizmos.color = Color.black;
		for (int i = 0; i < mNodeArray.Count; i++)
		{
			////draw vertices
			if (mSpringArray[i].type == 2) continue;


			//if(i == 0) Gizmos.color = Color.black;
			//else if(i == 1) Gizmos.color = Color.yellow;
			//else if (i == 255) Gizmos.color = Color.blue;
			//else Gizmos.color = Color.white;
			Gizmos.DrawSphere(mNodeArray[i].mPosition,0.05f);
		}
	}
	public List<CNode> getNodeArray()
	{
		return this.mNodeArray;
	}
	public int[] getTriangle()
	{
		return this.triangles;
	}


}
