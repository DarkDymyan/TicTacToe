using UnityEngine;
using System.Collections;

public static class Turn
{
    public static bool m_turn = false;
    static bool m_Pause = false;

    public static bool Pause
    {
        get { return m_Pause; }
    }

	public static bool IsTurn
	{
		get { return m_turn; }
		set { m_turn = value; }
	}


	public static IEnumerator SetPouse()
    {
		m_Pause = true;
        yield return new WaitForSeconds(0.6f);
		m_Pause = false;
    }
}
