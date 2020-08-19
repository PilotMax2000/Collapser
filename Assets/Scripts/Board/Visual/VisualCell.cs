using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualCell : MonoBehaviour
    {
        private Vector2Int _boardPos;
        [SerializeField] private VisualBlock _visualBlock;
        public VisualBlock VisualBlock => _visualBlock;

        public Vector2Int BoardPos => _boardPos;
        public int BoardX => _boardPos.x;
        public int BoardY => _boardPos.y;
        private BoardsBridge _boardsBridge;

        public void Init(Cell cell, BoardsBridge bridge)
        {
            _boardPos = cell.BoardPos;
            _boardsBridge = bridge;
        }

        public void SetBlock(VisualBlock block)
        {
            _visualBlock = block;
            var transform1 = block.transform;
            transform1.parent = transform;
            transform1.localPosition = Vector3.zero;
        }

        public void UnbindBlock()
        {
            _visualBlock = null;
        }

        public void RemoveBlock()
        {
            Destroy(_visualBlock.gameObject);
            UnbindBlock();
        }

        private void OnMouseDown()
        {
            Debug.Log($"[OnClick] Player clicked cell{_boardPos}");
            _boardsBridge.LogicActionOnClicked(_boardPos);
        }
    }

}
