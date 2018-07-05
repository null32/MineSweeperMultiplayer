namespace MineSweeperMulitplayer
{
    public class Tile
    {
        public Tile(uint row, uint col)
        {
            Visible = false;
            IsMine = false;
            Mines = 0;
            Row = row;
            Col = col;
        }

        public bool Visible { get; set; }
        public bool IsMine { get; set; }
        public uint Mines { get; set; }
        public uint Row { get; set; }
        public uint Col { get; set; }
    }
}
