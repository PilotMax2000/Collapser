using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "BoardsBridge", menuName = "Other/BoardBridge")]
    public class BoardsBridge : ScriptableObject
    {
        private Board _board;
        private VisualBoard _visualBoard;
        private VisualActionsHandler _actionsHandler;

        private Queue<Action> _removeActions = new Queue<Action>();
        private Queue<Action> _swapActions = new Queue<Action>();
        private Queue<Action> _spawnNewBlockActions = new Queue<Action>();
        
        [SerializeField] private FloatVariable _globalAnimationDuration;

        public void InitLogicBoard(Board board)
        {
            _board = board;
            if (board != null)
            {
                Debug.Log("Logic Board was registered in  bridge");
                _board.Init();
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
            if (_visualBoard != null && _board != null)
            {
                _visualBoard.GenerateBoard(_board.Map);
            }
        }

        public void LogicActionOnClicked(Vector2Int boardPos)
        {
            _board.OnClickReaction(_board.GetCell(boardPos));
        }

        public void VisualActionSwapBlocks(Vector2Int from, Vector2Int to)
        {
            _swapActions.Enqueue(() =>
                _visualBoard.SwapBlockFromTo(_visualBoard.GetVisualCell(from), _visualBoard.GetVisualCell(to)));
        }

        public void VisualActionSetNewBlock(Vector2Int cellPos, Block block)
        {
            _spawnNewBlockActions.Enqueue(() => _visualBoard.SetNewBlock(cellPos, block));
        }

        public void VisualActionRemoveBlock(Vector2Int cellPos)
        {
            _removeActions.Enqueue(() => _visualBoard.GetVisualCell(cellPos).RemoveBlock());
        }

        public void RunVisualActions()
        {
            _actionsHandler.DoWithoutWaiting(() => _visualBoard.BlockPlayersInput(true));
            _actionsHandler.DoAndWait(_removeActions, _globalAnimationDuration.Value);
            _actionsHandler.DoAndWait(_swapActions, _globalAnimationDuration.Value);
            _actionsHandler.DoAndWait(_spawnNewBlockActions, _globalAnimationDuration.Value);
            _actionsHandler.DoWithoutWaiting(() => _visualBoard.BlockPlayersInput(false));
        }
    }
}