using UnityEngine;

public class setSizeWindow : MonoBehaviour
{

	public int m_Width = 1024;
	public int m_Height = 768;
    
	void Awake ()
	{
		Screen.SetResolution(m_Width, m_Height, false);
	}
}
