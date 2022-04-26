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
                if(!CanPlaceChecker(posicao)){
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

    bool CanPlaceChecker(int posicao){
        if(board.Contains(posicao)) return false;
        int count = 0;
        int linha = posicao / boardSize;
        int coluna = posicao % boardSize;

        //percorre linha da posicao, iniciando no inicio da linha, esquerda para direita
        for (int i = 0; i < boardSize; i++)
        {
            int boardPos = linha * boardSize + i;
            if(board.Contains(boardPos)){
                count++;
                if(count >= checkersPerLine)
                    return false;
                }
        }
        
        count = 0;
        //percorre coluna da posicao, iniciando no inicio da coluna, cima para baixo
        for (int i = 0; i < boardSize; i++) 
        {
            int boardPos = i * boardSize + coluna;
            if(board.Contains(boardPos))
                count++;
            if(count >= checkersPerLine)
                return false;
        }

        return true && CheckMinorDiagonalOfIndex(posicao) && CheckMajorDiagonalOfIndex(posicao);
    }
    bool CheckMinorDiagonalOfIndex(int posicao){
    // Verificamos se podemos meter na diagonal:
    // Cima para baixo, esquerda para direita
        int count = 0;
        int coluna = posicao % boardSize;
        int index = posicao;
        while(coluna < boardSize && index < boardSize * boardSize)
        {
            if(board.Contains(index))
                count++;
            if(count >= checkersPerLine)
                {
                    return false;
                }
            index += boardSize + 1;
            coluna++;
        }

    // baixo para cima, direita para a esquerda
        coluna = (posicao % boardSize) - 1;
        index = posicao - (boardSize + 1);
        while(coluna >= 0 && index >= 0)
        {
            if(board.Contains(index))
                count++;
            if(count >= checkersPerLine)
                {
                    return false;
                }
            index -= boardSize + 1;
            coluna--;
        }

        return true;
    }
    bool CheckMajorDiagonalOfIndex(int posicao){

    // cima para baixo, direita para a esquerda

        int count = 0;
        int coluna = posicao % boardSize;
        int linha = posicao / boardSize;

        int index = posicao;
        while(coluna >= 0 && index >= 0 && linha < boardSize)
        {
            if(board.Contains(index))
                count++;
            if(count >= checkersPerLine){
                return false;
            }
            index += boardSize - 1;
            linha++;
            coluna--;
        }
    //  baixo para cima, esquerda para a direita

    // Como não quero repetir a mesma casa do array, tento ir para a posição seguinte na diagonal inversa
        int linhaInicial = posicao / boardSize;
        int colunaInicial = posicao % boardSize; 
        
            index = posicao - (boardSize - 1);
            coluna = index % boardSize;
            linha = index / boardSize;
        if(colunaInicial + 1 == coluna && linhaInicial - 1 >= linha)   
        {   
            while(linha >= 0 && coluna < boardSize && index >= 0)
            {
                if(board.Contains(index))
                    count++;
                if(count >= checkersPerLine){
                    return false;
                }
                index -= boardSize - 1;
                coluna++;
                linha--;
            }}
        return true;
    }

    public override void SolucaoVazia()
    {
        board.Clear();
    }

    public override bool SolucaoCompleta() {
        if(board.Count() != boardSize * checkersPerLine) return false;
        Console.WriteLine("Número de damas no tabuleiro: " + board.Count());
        return (
            VerificarLinhas() &&
            VerificarColunas() &&
            VerificarDiagonais()
        );
    }

    bool VerificarLinhas() {
        int count = 0;
        for (int linha = 0; linha < boardSize; linha++)
        {
            count = 0;
            for (int coluna = 0; coluna < boardSize; coluna++)
            {
                int boardPos = linha * boardSize + coluna;
                if(board.Contains(boardPos))
                {
                    count++;
                }
                if(count > checkersPerLine)
                    {
                        return false;
                    }
            }
            Console.WriteLine("{0} ", count);
        }

        return true;
    }

    bool VerificarColunas() {
        Console.WriteLine("Colunas: ");
        int count = 0;
        for (int coluna = 0; coluna < boardSize; coluna++)
        {
            count = 0;
            for (int linha = 0; linha < boardSize; linha++)
            {
                int boardPos = linha * boardSize + coluna;
                if(board.Contains(boardPos))
                {
                    count++;
                }
                if(count > checkersPerLine)
                {
                    return false;
                }
            }
            Console.WriteLine("{0} ", count);
        }

        return true;

    }

    bool VerificarDiagonais() {

        return (
            MinorDiagonal() && MajorDiagonal()
        );
    }

    bool MinorDiagonal(){
        //  Da direita para a esquerda, primeira linha ate ultima linha
            Console.Write("Minor Diagonal: ");

        for(int i = 0; i <= boardSize -1; i++)
        {
            int linha = i;
            int coluna = boardSize - 1;
            int numberOfCheckersInDiagonal = 0;
            while( linha >= 0)
            {
                int boardPos = linha * boardSize + coluna;
                if(board.Contains(boardPos))
                    {                   
                        numberOfCheckersInDiagonal++;
                    }
                if(numberOfCheckersInDiagonal > checkersPerLine)
                    {
                        return false;
                    }
                linha -= 1;
                coluna -= 1;
            }
            Console.Write("{0} ", numberOfCheckersInDiagonal);
        }

        // Da direita para a esquerda, ultima linha, ultima coluna ate primeira coluna


        for (int i = 1; i < boardSize - 1; i++)
        {
            int numberOfCheckersInDiagonal = 0;
            int linha = boardSize - 1;
            int coluna = (boardSize - 1) - i; 
            while(coluna >= 0)
            {
            int index = boardSize * linha + coluna;
                if(board.Contains(index))
                    numberOfCheckersInDiagonal++;
                if(numberOfCheckersInDiagonal > checkersPerLine)
                    {
                        return false;
                    }
                coluna--;
                linha--;
            }
            Console.Write("{0} ", numberOfCheckersInDiagonal);
        }
        
        return true;
    }
    
    bool MajorDiagonal()
    {
        //  Da esquerda para a direita, primeira linha ate ultima linha
        Console.Write("Major Diagonal: ");
        for(int i = 0; i <= boardSize -1; i++)
        {
            int linha = i;
            int coluna = 0;
            int numberOfCheckersInDiagonal = 0;
            while( linha >= 0)
            {
                int boardPos = linha * boardSize + coluna;
                if(board.Contains(boardPos))
                    numberOfCheckersInDiagonal++;
                if(numberOfCheckersInDiagonal > checkersPerLine)
                    return false;
                linha -= 1;
                coluna += 1;
            }
            Console.Write("{0} ", numberOfCheckersInDiagonal);
        }
        // Da esquerda para a direita, ultima linha, primeira coluna ate ultima coluna
        for(int i = 1; i <= boardSize - 1; i++)
        {
            int linha = boardSize - 1;
            int coluna = i;
            int numberOfCheckersInDiagonal = 0;
            while(coluna <= boardSize - 1)
            {
                int boardPos = linha * boardSize + coluna;
                if(board.Contains(boardPos))
                    numberOfCheckersInDiagonal++;
                if(numberOfCheckersInDiagonal > checkersPerLine)
                    return false;
                linha -= 1;
                coluna += 1;
            }
            Console.Write("{0} ", numberOfCheckersInDiagonal);
        }
        return true;
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