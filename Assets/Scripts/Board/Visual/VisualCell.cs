using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        public void SetBlock(VisualBlock block, bool showSetAnim = true)
        {
            _visualBlock = block;
            var transform1 = block.transform;
            if (showSetAnim == false)
            {
                transform1.parent = transform;
                transform1.localPosition = Vector3.zero;
                return;
            }
            
            transform1.DOMove(transform.position, .2f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    transform1.parent = transform;
                    transform1.localPosition = Vector3.zero;
                });

        }

        public void UnbindBlock()
        {
            _visualBlock = null;
        }

        public void RemoveBlock()
        {
            _visualBlock.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                Destroy(_visualBlock.gameObject);
                UnbindBlock();
            });
            
        }

        private void OnMouseDown()
        {
            Debug.Log($"[OnClick] Player clicked cell{_boardPos}");
            _boardsBridge.LogicActionOnClicked(_boardPos);
        }
    }

}
