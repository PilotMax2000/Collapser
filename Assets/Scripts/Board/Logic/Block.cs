
namespace Collapser
{
    public class Block
    {
        public BlockParams BlockParams { get; private set; }
        public Cell Cell { get; private set; }

        public Block(BlockParams blockParams, Cell parentCell)
        {
            SetBlockParams(blockParams, parentCell);
        }

        public void SetBlockParams(BlockParams blockParams, Cell parentCell)
        {
            BlockParams = blockParams;
            Cell = parentCell;
        }

        public void BindBlockToNewCell(Cell parentCell)
        {
            Cell = parentCell;
        }

    }
}
