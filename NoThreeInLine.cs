/*
    Author: Miguel Gonçalves
    Class: NoThreeInLine
    Description: Solves the No Three In Line problem
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

class NoThreeInLine : ProcuraConstrutiva, ICloneable {
    public List<int> Board {get; set;} = new List<int>();

    public int BoardSize { get; set; }
    public int CheckersPerLine { get; set; }
    public override int Cost { get; set; }
    public NoThreeInLine(int n = 4, int k = 2){
        Board = new List<int>();
        BoardSize = n;
        CheckersPerLine = k;
    }
    

    public object Clone()
    {
        return new NoThreeInLine();
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores)
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
                NoThreeInLine sucessor = Extensions.Clone(this);
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

    public override bool IsDuplicate(ProcuraConstrutiva currentNode, List<ProcuraConstrutiva> visitados)
    {
        NoThreeInLine node = (NoThreeInLine)currentNode;
        List<int> transposta = Utils.TranspostaMatrix(node.Board, node.BoardSize);

        List<NoThreeInLine> marked = new List<NoThreeInLine>();
        foreach (var item in visitados)
        {
            marked.Add((NoThreeInLine) item);
        }
        return VerificaDuplicados(node, transposta, marked).Count() > 0;
    }

    public List<ProcuraConstrutiva> VerificaDuplicados(NoThreeInLine currentNode, List<int> transposta, List<NoThreeInLine> visitados){
        
        return visitados.Where<NoThreeInLine>(x => 
                x.Board.SequenceEqual(currentNode.Board) || 
                x.Board.SequenceEqual(transposta)
                //x.Board.SequenceEqual(Utils.SymmetricMatrix(currentNode.Board, transposta, BoardSize))
            ).ToList<ProcuraConstrutiva>();
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

    public override int GetResult(){ return Board.Count(); }
    public override void Teste(){
        Console.Clear();
        while(true){
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme  [Procuras Cegas]");
            Console.WriteLine("[4] - Definir N({0})   | [5] - Definir K ({1})       | [6] - Debug ({2})      [Configurações]", BoardSize, CheckersPerLine, debug);
            Console.WriteLine("[0] - Sair                                                                          [Sistema]");
            Console.WriteLine("---------------------------------------  Estatísticas  --------------------------------------");
            Console.WriteLine("Expansoes: {0}                Geracoes: {1}", expansoes, geracoes);
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.Write("Opcao: ");
            
            string op = Console.ReadLine();
            LimpaTudo();
            SolucaoVazia();
            switch(op) {
                case "1": 
                    SetTimer();
                    Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
                    aTimer.Stop();
                    break;
                case "2": 
                    SetTimer();
                    Console.WriteLine("Profundidade Primeiro: " + ProfundidadePrimeiro(stack, visitados).ToString());
                    aTimer.Stop();
                    break;
                case "3": 
                    SetTimer();
                    Console.WriteLine("Custo Uniforme: " + CustoUniforme(priorityQueue, visitados).ToString());
                    aTimer.Stop();
                    break;
                case "4": 
                    Console.Write("Definir N: ");
                    if(!int.TryParse(Console.ReadLine(), out int n))
                        Console.WriteLine("Invalid value entered");
                    else
                        BoardSize = n;
                    break;
                case "5": 
                    Console.Write("Definir K: ");
                    if(!int.TryParse(Console.ReadLine(), out int k))
                        Console.WriteLine("Invalid value entered");
                    else
                        CheckersPerLine = k;
                    break;
                case "6": 
                    Console.Write("Definir debug: ");
                    if(!int.TryParse(Console.ReadLine(), out int d))
                        Console.WriteLine("Invalid value entered");
                    else
                        debug = d;
                    break;
                case "0": 
                    aTimer.Dispose();
                    System.Environment.Exit(0);
                    break;
                default: 
                    Console.WriteLine("Opção não válida!");
                    break;
            }

        }
    }
}