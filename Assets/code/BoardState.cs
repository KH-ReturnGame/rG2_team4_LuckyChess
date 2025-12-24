//board11에서 행마법 데이터만 추출(아마)

public enum PieceType { None, Pawn, Bishop, Knight, Rook, Queen, King }
public enum PieceColor { None, White, Black }

public class BoardState
{
    public PieceType[,] piece = new PieceType[8, 8];
    public PieceColor[,] color = new PieceColor[8, 8];

    public bool whiteTurn;

    public BoardState Clone()
    {
        BoardState copy = new BoardState();
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                copy.piece[r, c] = piece[r, c];
                copy.color[r, c] = color[r, c];
            }
        }
        copy.whiteTurn = whiteTurn;
        return copy;
    }
}
