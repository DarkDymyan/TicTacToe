using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public Image m_Win, m_Fail, m_Draw;
    public Transform m_Line;
    public Transform m_Cell;
    public Transform m_CellsParent;
    private Transform[,] m_Sprites;
    private float m_PosX = -150.0f;
    private float m_PosY = 100.0f;
    
    void Awake ()
    {
        InitField();
    }

    void InitField()
    {
        int arrColl = 0, arrRow = 0;
		Vector2 size = m_Cell.GetComponent<BoxCollider2D>().size * m_Cell.localScale;
		m_Sprites = new Transform[3, 3];
        for (int i = 1; i <= 9; i++)
        {
            Transform tempObj = Instantiate(m_Cell, new Vector3(m_PosX, m_PosY, 0), Quaternion.identity) as Transform;
			m_Sprites[arrColl, arrRow] = tempObj;
            tempObj.transform.SetParent(m_CellsParent.transform);
            tempObj.GetComponent<CellClass>().Row = arrRow;
            tempObj.GetComponent<CellClass>().Coll = arrColl;
            
            arrColl++;
			m_PosX += size.x;
            
            if (i % 3 == 0)
            {
                arrColl = 0;
				arrRow++;
				m_PosX = -size.x;
				m_PosY -= size.y;
            }
        }
    }

    public bool CheckWin(Transform callerCell, string callerFigure)
    {
        bool isWin = false;
        if (callerCell)
        {
            Vector2 currenPos = new Vector2();
            currenPos.x = callerCell.GetComponent<CellClass>().Coll;
            currenPos.y = callerCell.GetComponent<CellClass>().Row;

            if (StepByField(new Vector2(0, -1), currenPos) == 2)
            {
                DrawLine(m_Sprites[(int)currenPos.x, (int)currenPos.y].position, "vertical");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
            else if (StepByField(new Vector2(1, -1), currenPos) == 2)
            {
                DrawLine(m_Sprites[(int)currenPos.x, (int)currenPos.y].position, "left_diagonal");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
            else if (StepByField(new Vector2(1, 0), currenPos) == 2)
            {
                DrawLine(m_Sprites[(int)currenPos.x, (int)currenPos.y].position, "horizontal");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
            else if (StepByField(new Vector2(1, 1), currenPos) == 2)
            {
                DrawLine(m_Sprites[(int)currenPos.x, (int)currenPos.y].position, "right_diagonal");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
        }
        else
        {
            StartCoroutine(ShowMessage(null));
        }

        return isWin;
    }

    int StepByField(Vector2 vector, Vector2 currentPosition)
    {  
        int coincide = 0;
        
        Vector2 step = currentPosition + vector;
        string figure = m_Sprites[(int)currentPosition.x, (int)currentPosition.y].GetComponent<SpriteRenderer>().sprite.name;

        int stop = 0;
        while (stop < 2)
        {
                                                                                                                  
            if ((step.x > -1 && step.y > -1 && step.x < 3 && step.y < 3)                                        
                && (m_Sprites[(int) step.x, (int) step.y].GetComponent<SpriteRenderer>().sprite)                     
                && m_Sprites[(int)step.x, (int)step.y].GetComponent<SpriteRenderer>().sprite.name == figure)         
                                                                                                                  
            {
                coincide++;
                step += vector;
            }
            else
            {
                vector *= -1;
                step = currentPosition + vector;
                stop++;
            }
        }
        return coincide;
    }

    IEnumerator ShowMessage(string winner)
    {
        yield return new WaitForSeconds(1.0f);

        GameObject.Find("field_spr").GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GameObject.Find("line(Clone)"));
        foreach (Transform sprite in m_Sprites)
            sprite.GetComponent<SpriteRenderer>().sprite = null;

        if (winner != null)
        {   
            if (GameObject.Find("Player").GetComponent<Player>().Name == winner)
            {
                m_Win.enabled = true;
            }
            else
            {
                m_Fail.enabled = true;
            }
        }
        else
        {
			m_Draw.enabled = true;
        }
    }

    void DrawLine(Vector2 position, string orientation)
    {
        Vector3 posLine = position;
        Transform tr;
        switch (orientation)
        {
            case "vertical":
                posLine.y = 0; 
                Instantiate(m_Line, posLine, Quaternion.identity);
                break;
            case "horizontal":
                posLine.x = 0;
                Instantiate(m_Line, posLine, Quaternion.Euler(0, 0, 90));
                break;
            case "left_diagonal": 
                tr = (Transform)Instantiate(m_Line, new Vector3(0, -50, 0), Quaternion.Euler(0,0,-45));
                tr.localScale += new Vector3(0, 45.0f, 0);
                break;
            case "right_diagonal":
                tr = (Transform)Instantiate(m_Line, new Vector3(0, -50, 0), Quaternion.Euler(0, 0, 45));
                tr.localScale += new Vector3(0, 45.0f, 0);
                break;
        }
    }

    public void Restart()
    {
        Turn.IsTurn = false;
        GameObject.Find("field_spr").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("Enemy").GetComponent<Enemy>().ReInitEnemy();
        GameObject.Find("Player").GetComponent<Player>().ReinitPlayer();

        if (!m_Win.enabled && !m_Fail.enabled && !m_Draw.enabled)
        {
            foreach (Transform sprite in m_Sprites)
                sprite.GetComponent<SpriteRenderer>().sprite = null;
        }

        m_Win.enabled = false;
        m_Fail.enabled = false;
		m_Draw.enabled = false;
    }
}
