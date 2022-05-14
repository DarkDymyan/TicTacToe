using UnityEngine;

public class CellClass : MonoBehaviour
{
    private int m_Row, m_Coll;

	public int Row
	{
		get { return m_Row; }
		set { m_Row = value; }
	}

	public int Coll
	{
		get { return m_Coll; }
		set { m_Coll = value; }
	}
}
