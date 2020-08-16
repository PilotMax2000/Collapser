using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualBoard : MonoBehaviour
    {
        [SerializeField] private int _sizeX;
        [SerializeField] private int _sizeY;

        [Header("Prefabs")] 
        [SerializeField] private VisualCell _cellPref;
        [SerializeField] private VisualBlock _blockPref;
        
            // Start is called before the first frame update
            void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GenerateBoard(Cell[,] boardCells)
        {
            _sizeX = boardCells.GetLength(0);
            _sizeY = boardCells.GetLength(1);


            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    //Create Cell
                    VisualCell cell = Instantiate(_cellPref, transform);
                    cell.Init(boardCells[x,y]);
                    var cellTransform = cell.transform;
                    cellTransform.position = new Vector3(x,y,0);
                    
                    VisualBlock block = Instantiate(_blockPref, cellTransform);
                    block.Init(boardCells[x,y].Block);
                    
                    cell.SetBlock(block);
                    //Create Block
                }
            }
            
        }

        //TODO: Add to game event system
        public void UpdateBoard()
        {
        
        }

        private void CreateBlock()
        {
        
        }
    }

}

