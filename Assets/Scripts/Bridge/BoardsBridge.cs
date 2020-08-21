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
    private VisualActionsHandler _actionsHandler;

    private Queue<Action> _removeActions = new Queue<Action>();
    private Queue<Action> _swapActions = new Queue<Action>();
    private Queue<Action> _spawnNewBlockActions = new Queue<Action>();

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

    public void InitVisualActionHandler(VisualActionsHandler actionsHandler)
    {
        _actionsHandler = actionsHandler;
    }

    public void GenerateVisualMap()
    {
        if (_visualBoard != null && _boardMap != null)
        {
            _visualBoard.GenerateBoard(_boardMap.Map);
        }
    }

    public void LogicActionOnClicked(Vector2Int boardPos)
    {
        _boardMap.OnClickReaction(_boardMap.GetCell(boardPos));
    }

    public void VisualActionSwapBlocks(Vector2Int from, Vector2Int to)
    {
        _swapActions.Enqueue(() => _visualBoard.SwapBlockFromTo(_visualBoard.GetVisualCell(from), _visualBoard.GetVisualCell(to))); 
    }

    public void VisualActionSetNewBlock(Vector2Int cellPos, Block block)
    {
        _spawnNewBlockActions.Enqueue(() => _visualBoard.SetNewBlock(cellPos, block));
    }

    public void VisualActionRemoveBlock(Vector2Int cellPos)
    {
        _removeActions.Enqueue(()=> _visualBoard.GetVisualCell(cellPos).RemoveBlock());
    }

    public void RunVisualActions()
    {
        _actionsHandler.DoWithoutWaiting(() => _visualBoard.BlockPlayersInput(true));
        _actionsHandler.DoAndWait(_removeActions, .2f);
        _actionsHandler.DoAndWait(_swapActions, .2f);
        _actionsHandler.DoAndWait(_spawnNewBlockActions, .2f);
        _actionsHandler.DoWithoutWaiting(() => _visualBoard.BlockPlayersInput(false));
    }


}
