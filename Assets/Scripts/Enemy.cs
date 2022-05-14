using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool m_Enable = true;
    public Sprite m_Round;
    public string m_NamePlayer = "Round";
    public Transform m_CellsParent;
    private string m_HuPlayer = "X";
    private string m_AiPlayer = "O";
    private int m_FieldSize = 3;


	private struct Move
    {
        private int m_Score;
		private int m_Row;
		private int m_Coll;
        public Move(int _row, int _coll, int _score = 0)
        {
			m_Row = _row;
			m_Coll = _coll;
			m_Score = _score;
        }

		public int Score
		{
			get { return m_Score; }
			set { m_Score = value; }
		}

		public int Row
		{
			get { return m_Row; }
			set { m_Row  = value; }
		}

		public int Coll
		{
			get { return m_Coll; }
			set { m_Coll = value; }
		}
	};

    void Update()
    {
		if (!ScenesManager.Inst.IsOnePlayer)
		{
			return;
		}

		if (m_Enable && Turn.IsTurn && !Turn.Pause)
		{
			TurnEnemy();
		}
    }

    public void ReInitEnemy()
    {
		m_Enable = true;
    }

    void TurnEnemy()
    {
        string[,] CellsBoard = new string[m_FieldSize, m_FieldSize];

        for (int i = 0; i < m_CellsParent.childCount; i++)
        {
            Transform cell = m_CellsParent.GetChild(i);
            int indI = i / m_FieldSize;
            int indJ = i % m_FieldSize;
            if (!cell.GetComponent<SpriteRenderer>().sprite)
            {
                CellsBoard[indI, indJ] = i.ToString();
            }
            else if (cell.GetComponent<SpriteRenderer>().sprite == m_Round)
            {
                CellsBoard[indI, indJ] = m_AiPlayer;
            }
            else
            {
                CellsBoard[indI, indJ] = m_HuPlayer;
            }

        }

		if (EmptyIndicesCount(CellsBoard) == 0)
		{
			m_Enable = false;
			GameObject.Find("GamePlay").GetComponent<GamePlayController>().CheckWin(null, null);

			return;
		}
		
		Move move = Minimax(CellsBoard, m_AiPlayer);

		Transform moveCell = m_CellsParent.GetChild(move.Row * m_FieldSize + move.Coll);

        if (!moveCell.GetComponent<SpriteRenderer>().sprite)
        {
            moveCell.GetComponent<SpriteRenderer>().sprite = m_Round;

            if (!GameObject.Find("GamePlay").GetComponent<GamePlayController>().CheckWin(moveCell, m_NamePlayer))
            {
                Turn.IsTurn = false;
                StartCoroutine(Turn.SetPouse());
            }
            else
            {
                m_Enable = false;
            }
        }
    }

    Move Minimax(string[,] _cellsBoard, string _player)
    {
		List<Move> emptyIndices = EmptyIndices(_cellsBoard);

		if (IsWinning(_cellsBoard, m_HuPlayer))
		{
			return new Move(-1, -1, -10);
		}
		else if (IsWinning(_cellsBoard, m_AiPlayer))
		{
			return new Move(-1, -1, 10);
		}
		else if (emptyIndices.Count == 0)
		{
			return new Move(-1, -1, 0);
		}

		List<Move> moves = new List<Move>();

		for (int i = 0; i < emptyIndices.Count; i++)
		{
			Move move = emptyIndices[i];
			string prewValue = _cellsBoard[move.Row, move.Coll];

			_cellsBoard[move.Row, move.Coll] = _player;

			if (_player == m_AiPlayer)
			{
				Move result = Minimax(_cellsBoard, m_HuPlayer);
				move.Score = result.Score;
			}
			else
			{
				Move result = Minimax(_cellsBoard, m_AiPlayer);
				move.Score = result.Score;
			}

			_cellsBoard[move.Row, move.Coll] = prewValue;

			moves.Add(move);
		}

		int bestMove = 0;
		if (_player == m_AiPlayer)
		{
			var bestScore = -10000;
			for (var i = 0; i < moves.Count; i++)
			{
				if (moves[i].Score > bestScore)
				{
					bestScore = moves[i].Score;
					bestMove = i;
				}
			}
		}
		else
		{
			var bestScore = 10000;
			for (var i = 0; i < moves.Count; i++)
			{
				if (moves[i].Score < bestScore)
				{
					bestScore = moves[i].Score;
					bestMove = i;
				}
			}
		}
		
		return moves[bestMove];
	}

    List<Move> EmptyIndices(string[,] _cellsBoard)
    {
        List<Move> resultIndx = new List<Move>();
        for (int i = 0; i < 3; i++)
        {
            for (int j= 0; j < 3; j++)
            {
                string cell = _cellsBoard[i, j];
                if (!cell.Equals(m_HuPlayer) && !cell.Equals(m_AiPlayer))
                {
                    resultIndx.Add(new Move(i, j));
                }
            }
        }

        return resultIndx;
    }

    int EmptyIndicesCount(string[,] _cellsBoard)
    {
        return EmptyIndices(_cellsBoard).Count;
    }

    bool IsWinning(string[,] _cellsBoard, string _player)
    {
       if (
         (_cellsBoard[0, 0] == _player && _cellsBoard[1, 0] == _player && _cellsBoard[2, 0] == _player) ||
          (_cellsBoard[0, 1] == _player && _cellsBoard[1, 1] == _player && _cellsBoard[2, 1] == _player) ||
          (_cellsBoard[0, 2] == _player && _cellsBoard[1, 2] == _player && _cellsBoard[2, 2] == _player) ||
          (_cellsBoard[0, 0] == _player && _cellsBoard[0, 1] == _player && _cellsBoard[0, 2] == _player) ||
          (_cellsBoard[1, 0] == _player && _cellsBoard[1, 1] == _player && _cellsBoard[1, 2] == _player) ||
          (_cellsBoard[2, 0] == _player && _cellsBoard[2, 1] == _player && _cellsBoard[2, 2] == _player) ||
          (_cellsBoard[0, 0] == _player && _cellsBoard[1, 1] == _player && _cellsBoard[2, 2] == _player) ||
          (_cellsBoard[0, 2] == _player && _cellsBoard[1, 1] == _player && _cellsBoard[2, 0] == _player)
          )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    string[,] Clone(string[,] board)
    {
        var newboard = (string[,])board.Clone();
        return newboard;
    }
}
