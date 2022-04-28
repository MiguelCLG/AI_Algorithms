/*
    Author: Miguel Gonçalves
    Class: NoThreeInLine
    Description: Solves the No Three In Line problem
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

class NoThreeInLine : ProcuraConstrutiva, ICloneable {
    public override List<int> Board {get; set;} = new List<int>();

    public override int BoardSize { get; set; } = 0;
    public override int CheckersPerLine { get; set; } = 0;

    public NoThreeInLine(){
        Board = new List<int>();
    }
    

    public object Clone()
    {
        return new NoThreeInLine();
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores, int custo)
    {
        int boardCount = Board.Count();
        // Console.WriteLine("boardCount: {0} BoardSize: {1} CheckersPerLine: {2}", boardCount, BoardSize, CheckersPerLine);
        for (int posicao=0; posicao < BoardSize*BoardSize; posicao++)
        {   
            int j = 0;
            for (; j < boardCount; j++)
            {   
                if(!Utils.CanPlaceChecker(posicao, Board, BoardSize, CheckersPerLine)){
                    break;
                }
            }

            if(j == boardCount)    //pode adicionar dama       
            {
                NoThreeInLine sucessor = ObjectExtensions.Copy(this);
                sucessor.Board.Add(posicao);
                sucessor.Board.Sort();
                sucessores.Add(sucessor);
            }
            
        }

        AddResult(Board.Count());
        Expand(sucessores);        
        return sucessores;
    }

    public override void SolucaoVazia()
    {
        Board.Clear();
    }

    public override bool SolucaoCompleta() {
        if(Board.Count() != BoardSize * CheckersPerLine) return false;
        Console.WriteLine("Número de damas no tabuleiro: " + Board.Count());
        return (
            Utils.VerificarLinhas(Board, BoardSize, CheckersPerLine) &&
            Utils.VerificarColunas(Board, BoardSize, CheckersPerLine) &&
            Utils.VerificarDiagonais(Board, BoardSize, CheckersPerLine)
        );
    }
    public override void Debug()
    {
        Console.WriteLine();
        for (int i = 0; i < BoardSize; i++)
        {
            
                Console.WriteLine();
                for (var j = 0; j < BoardSize; j++)
                {
                    int cor=((i + j)%2 > 0 ? 43 : 43);  // 176:178  --- jm 03/4/2018
                    int boardPos = i * BoardSize + j;
                    if(Board.Contains(boardPos))
                        Console.Write("{0} ", (char)68);  // 16:17   --- jm 03/4/2018
                    else Console.Write("{0} ",(char)cor);
                    
                }
        }
        Console.WriteLine();
        Console.WriteLine("Número de peças no board: {0}", Board.Count());
    }

    //calcula a transposta da nossa matriz
    // void Transposta(NoThreeInLine estado)
    // {
    //     int novoBoard[boardSize * boardSize];
    //     for (int i = 0; i < boardSize; i++)
    //     {
    //         for (int j = 0; j < boardSize; j++)
    //         {
    //             int indexVelho = i * boardSize + j;
    //             int indexNovo = j * boardSize + i;
    //             novoBoard[indexNovo] = estado->Board[indexVelho];
    //         }
    //     }
    //     for (int i = 0; i < boardSize * boardSize; i++)
    //     {
    //         estado->Board[i] = novoBoard[i];
    //     }
    // }
}