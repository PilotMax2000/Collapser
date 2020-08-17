using System;
using System.Collections;
using System.Collections.Generic;
using Collapser;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardsBridge", menuName = "Other/BoardBridge")]
public class BoardsBridge : ScriptableObject
{
    private BoardMap _boardMap;
    private VisualBoard _visualBoard;

    public BoardMap BoardMap => _boardMap;

    public VisualBoard VisualBoard => _visualBoard;

    public void InitLogicBoard(BoardMap boardMap)
    {
        _boardMap = boardMap;
    }

    public void InitVisualBoard(VisualBoard visualBoard)
    {
        _visualBoard = visualBoard;
    }

    public void SendVisualBoardAction(Action visualAction)
    {
        BuildCellsConnecctions();
        visualAction.Invoke();
    }

    public void SendLogicBoardAction(Action logicAction)
    {
        BuildCellsConnecctions();
        logicAction.Invoke();
    }

    public VisualCell GetVisualCell(Cell cell)
    {
        return _visualBoard.GetCell(cell.BoardPos);
    }

    public Cell GetLogicCell(VisualCell visualCell)
    {
        return _boardMap.GetCell(visualCell.BoardPos);
    }

    private void BuildCellsConnecctions()
    {
        //throw new System.NotImplementedException();
    }
}
