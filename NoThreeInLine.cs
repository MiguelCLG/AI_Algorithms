/*
    Author: Miguel Gonçalves
    Class: NoThreeInLine
    Description: Solves the No Three In Line problem
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

class NoThreeInLine : ProcuraConstrutiva, ICloneable {
    public override List<int> Board {get; set;}

    int boardSize = 4;
    int checkersPerLine = 2;

    public NoThreeInLine(){
        Board = new List<int>();
        boardSize = 4;
        checkersPerLine = 2;
    }
    
    public NoThreeInLine(List<int> a){
        Board = a;
    }
    public object Clone()
    {
        return new NoThreeInLine(Board);
    }

    void ConfigureSearch(int n, int k)
    {
        boardSize = n;
        checkersPerLine = k;
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores, int custo, string algorithm)
    {
        int boardCount = Board.Count();
        for (int posicao=0; posicao < boardSize * boardSize; posicao++)
        {   
            int j = 0;
            for (; j < boardCount; j++)
            {   
                if(!Utils.CanPlaceChecker(posicao, Board, boardSize, checkersPerLine)){
                    break;
                }
            }

            if(j == boardCount)    //pode adicionar dama       
            {
                NoThreeInLine sucessor = ObjectExtensions.Copy(this);
                sucessor.Board.Add(posicao);
                sucessor.Board.Sort();
                //if(!IsDuplicate((ProcuraConstrutiva)sucessor, algorithm))
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
        if(Board.Count() != boardSize * checkersPerLine) return false;
        Console.WriteLine("Número de damas no tabuleiro: " + Board.Count());
        return (
            Utils.VerificarLinhas(Board, boardSize, checkersPerLine) &&
            Utils.VerificarColunas(Board, boardSize, checkersPerLine) &&
            Utils.VerificarDiagonais(Board, boardSize, checkersPerLine)
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