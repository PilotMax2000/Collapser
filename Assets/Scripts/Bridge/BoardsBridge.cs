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

    //public BoardMap BoardMap => _boardMap;

    //public VisualBoard VisualBoard => _visualBoard;

    public void InitLogicBoard(BoardMap boardMap)
    {
        _boardMap = boardMap;
        if (boardMap != null)
        {
            Debug.Log("Logic Board was registered in  bridge");
            _boardMap.Init();
        }
    }

    public void InitVisualBoard(VisualBoard visualBoard)
    {
        _visualBoard = visualBoard;
    }

    public void GenerateVisualMap()
    {
        if (_visualBoard != null && _boardMap != null)
        {
            _visualBoard.GenerateBoard(_boardMap.Map);
        }
    }

    // public void SendVisualBoardAction(Action visualAction)
    // {
    //     visualAction.Invoke();
    // }
    //
    // public void SendLogicBoardAction(Action logicAction)
    // {
    //     logicAction.Invoke();
    // }

    public void LogicActionOnClicked(Vector2Int boardPos)
    {
        _boardMap.OnClickReaction(_boardMap.GetCell(boardPos));
    }


    public void VisualActionSwapBlocks(Vector2Int from, Vector2Int to)
    {
        _visualBoard.SwapBlockFromTo(_visualBoard.GetVisualCell(from), _visualBoard.GetVisualCell(to));
    }

    public void VisualActionSetNewBlock(Vector2Int cellPos, Block block)
    {
        _visualBoard.SetNewBlock(cellPos, block);
    }

    public void VisualActionRemoveBlock(Vector2Int cellPos)
    {
        _visualBoard.GetVisualCell(cellPos).RemoveBlock();
    }


}
