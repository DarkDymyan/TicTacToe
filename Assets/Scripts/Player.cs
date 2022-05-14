using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite m_Sprite;
    public string m_NamePlayer = "Player";
	public bool m_Enable = true;
	public Transform m_OppositePlayer;

	void Update()
    {
        if (m_Enable && Input.GetMouseButtonDown(0))
            ClickPlayer();
    }

    public string Name
    {
		get { return m_NamePlayer; }
    }

	public bool Enable
	{
		get { return m_Enable; }
		set { m_Enable = value; }
	}

	void ClickPlayer()
    {
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector2.zero);

        if (hit.collider)
        {
            Transform hitCell = hit.transform;

            if (!hitCell.GetComponent<SpriteRenderer>().sprite && !Turn.IsTurn && !Turn.Pause)
            {
                hitCell.GetComponent<SpriteRenderer>().sprite = m_Sprite;

                if (!GameObject.Find("GamePlay").GetComponent<GamePlayController>().CheckWin(hitCell, m_NamePlayer))
                {
					if (ScenesManager.Inst.IsOnePlayer)
					{
						Turn.IsTurn = true;
						StartCoroutine(Turn.SetPouse());
					}
					else
					{
						Enable = false;
						m_OppositePlayer.GetComponent<Player>().Enable = true;
					}
                }
                else
                {
					Enable = false;
                }
            }
        }
    }
    public void ReinitPlayer()
    {
		Enable = true;
    }
}
