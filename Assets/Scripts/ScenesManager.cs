using UnityEngine;


public class ScenesManager
{
	public static ScenesManager Inst = new ScenesManager();


	private bool m_isOnePlayer = true;

	public bool IsOnePlayer
	{
		get { return m_isOnePlayer; }
		set { m_isOnePlayer = value; }
	}
}
