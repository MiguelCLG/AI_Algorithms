/*
    Author: Miguel Gonçalves
    Class: Static Utils
    Description: Has any calculations or verifications for matrix
*/
static class Utils{
    
    public static bool CanPlaceChecker(int posicao, List<int> board, int boardSize, int checkersPerLine){
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

        return true && CheckMinorDiagonalOfIndex(posicao, board, boardSize, checkersPerLine) && CheckMajorDiagonalOfIndex(posicao, board, boardSize, checkersPerLine);
    }

    public static bool VerificarLinhas(List<int> board, int boardSize, int checkersPerLine){
        Console.Write("Linhas: ");
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
            Console.Write("{0} ", count);
        }

        return true;
    }

    public static bool VerificarColunas(List<int> board, int boardSize, int checkersPerLine) {
        Console.Write("Colunas: ");
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
            Console.Write("{0} ", count);
        }

        return true;
    }

    public static bool VerificarDiagonais(List<int> board, int boardSize, int checkersPerLine) {

        return (
            MinorDiagonal(board, boardSize, checkersPerLine) && MajorDiagonal(board, boardSize, checkersPerLine)
        );
    }

    public static bool MinorDiagonal(List<int> board, int boardSize, int checkersPerLine){
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
    
    public static bool MajorDiagonal(List<int> board, int boardSize, int checkersPerLine)
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

    public static bool CheckMinorDiagonalOfIndex(int posicao, List<int> board, int boardSize, int checkersPerLine){
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

    public static bool CheckMajorDiagonalOfIndex(int posicao, List<int> board, int boardSize, int checkersPerLine){
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

    public static List<int> SymmetricMatrix (List<int> origList, List<int> transposedList, int boardSize){
    int[] origMatrix = ConvertListToMatrix(origList, boardSize);
    int[] transposedMatrix = ConvertListToMatrix(transposedList, boardSize);
    int[] result = new int[boardSize * boardSize];
    for(int i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
        {
           if (i<=j) result[i * boardSize + j] = i*boardSize-i*(i+1)/2+j;              //above the diagonal
           else if( i>j ) result[i * boardSize + j] = j*boardSize-j*(j+1)/2+i;              //below the diagonal
        }

    List<int> resultado = result.ToList();
    resultado.RemoveAll(i => i == -1);
    resultado.Sort();
    return resultado;
}
    public static List<int> TranspostaMatrix(List<int> list, int boardSize){
        int[] matrix = ConvertListToMatrix(list, boardSize);
        int [] aux = matrix.Copy();
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int indexVelho = i * boardSize + j;
                int indexNovo = j * boardSize + i;
                if(matrix[indexVelho] == -1) 
                    aux[indexNovo] = matrix[indexVelho];
                else
                    aux[indexNovo] = indexNovo;
            }
        }
        
        // DebugMatrixes(aux, boardSize);
        // DebugMatrixes(matrix, boardSize);

        List<int> resultado = aux.ToList();
        resultado.RemoveAll(i => i == -1);
        resultado.Sort();
        return resultado;
    }

    public static void DebugMatrixes(int[] matrix, int boardSize)
    {
        Console.WriteLine();
        for (var i = 0; i < matrix.Length; i++)
        {
            if(i % boardSize == 0) Console.WriteLine();
            Console.Write("{0} ", matrix[i] == -1? "." : matrix[i]);
        }
        Console.WriteLine();
    }

    public static int[] ConvertListToMatrix(List<int> list, int boardSize)
    {
        int[] matrix = new int[boardSize * boardSize];
        for (int i = 0; i < boardSize * boardSize; i++)
        {
            if(list.Contains(i))
            {
                matrix[i] = i;
            }
            else{
                matrix[i] = -1;
            }
        }
        return matrix;
    }

    public static List<int> FillCleanBoard(int boardSize)
    {
        List<int> result = new List<int>();
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                result.Add(0);
            }
        }

        return result;
    }
}
