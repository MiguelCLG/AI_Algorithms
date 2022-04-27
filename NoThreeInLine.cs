/*
    Author: Miguel Gonçalves
    Class: NoThreeInLine
    Description: Solves the No Three In Line problem
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

class NoThreeInLine : ProcuraConstrutiva, ICloneable {
    List<int> board = new List<int>();
    int boardSize = 4;
    int checkersPerLine = 2;

    public NoThreeInLine(){
        board = new List<int>();
        boardSize = 4;
        checkersPerLine = 2;
    }
    
    public NoThreeInLine(List<int> a){
        board = a;
    }
    public object Clone()
    {
        return new NoThreeInLine(board);
    }

    void ConfigureSearch(int n, int k)
    {
        boardSize = n;
        checkersPerLine = k;
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores, int custo)
    {
        int boardCount = board.Count();
        for (int posicao=0; posicao < boardSize * boardSize; posicao++)
        {   
            int j = 0;
            for (; j < boardCount; j++)
            {   
                if(!Utils.CanPlaceChecker(posicao, board, boardSize, checkersPerLine)){
                    break;
                }
            }

            if(j == boardCount)    //pode adicionar dama       
            {
                NoThreeInLine sucessor = ObjectExtensions.Copy(this);
                sucessor.board.Add(posicao);
                sucessores.Add(sucessor);
            }
            
        }

        AddResult(board.Count());
        Expand(sucessores);        
        return sucessores;
    }

    public override void SolucaoVazia()
    {
        board.Clear();
    }

    public override bool SolucaoCompleta() {
        if(board.Count() != boardSize * checkersPerLine) return false;
        Console.WriteLine("Número de damas no tabuleiro: " + board.Count());
        return (
            Utils.VerificarLinhas(board, boardSize, checkersPerLine) &&
            Utils.VerificarColunas(board, boardSize, checkersPerLine) &&
            Utils.VerificarDiagonais(board, boardSize, checkersPerLine)
        );
    }
    public override void Debug()
    {
        Console.WriteLine();
        for (int i = 0; i < boardSize; i++)
        {
            
                Console.WriteLine();
                for (var j = 0; j < boardSize; j++)
                {
                    int cor=((i + j)%2 > 0 ? 43 : 43);  // 176:178  --- jm 03/4/2018
                    int boardPos = i * boardSize + j;
                    if(board.Count()>i && board.Contains(boardPos))
                        Console.Write("{0} ", (char)68);  // 16:17   --- jm 03/4/2018
                    else Console.Write("{0} ",(char)cor);
                    
                }
        }
        Console.WriteLine();
        Console.WriteLine("Número de peças no board: {0}", board.Count());
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
    //             novoBoard[indexNovo] = estado->board[indexVelho];
    //         }
    //     }
    //     for (int i = 0; i < boardSize * boardSize; i++)
    //     {
    //         estado->board[i] = novoBoard[i];
    //     }
    // }
}