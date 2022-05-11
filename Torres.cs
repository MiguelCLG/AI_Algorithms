/*
    Author: Miguel Gonçalves
    Class: Torres
    Description: Tries to place as many towers (of different colours, marked as A B or C) as it can as long as they arent attacking
    the same colour or 2 or more of different colours.
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

//TODO: Verificar sucessores, o que esta a acontecer na verificação?
class Torres : ProcuraConstrutiva, ICloneable {

    public List<PlaceInBoard> Board { get; set; }
    public List<int> AvailableSpaces { get; set; }
    public int BoardSize { get; set; }
    public int NumeroDeAtaques {get; set;}
    public override int Cost { get; set;}
    public Torres(int n = 4){
        Board = new List<PlaceInBoard>();
        BoardSize = n;
        AvailableSpaces = new List<int>(Utils.FillCleanBoard(BoardSize));
        Cost = 0;
        NumeroDeAtaques = 0;
    }
    
    public object Clone()
    {
        return new Torres();
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores) { 
        int boardCount = Board.Count();
        Towers currentTower = Towers.A;
        for (int posicao=0; posicao < AvailableSpaces.Count(); posicao++)
        {
            int pos = AvailableSpaces.ElementAt(posicao);
            //canNotPlaceCount: Counts the number of tower types we can not put in this position
            int canNotPlaceCount = 0;
            for(int k = 0; k < 3; k ++){
                int j = 0;
                for (; j < boardCount; j++)
                {   
                    if(!CanPlaceTower(Board, currentTower, pos, BoardSize)){
                        canNotPlaceCount++;
                        break;
                    }
                }

                if(j == boardCount)    //pode adicionar dama       
                {
                    Torres sucessor = ObjectExtensions.Copy(this);
                    PlaceInBoard torre = new PlaceInBoard();
                    torre.cor = currentTower;
                    torre.posicao = pos;
                    sucessor.AvailableSpaces.Remove(pos);
                    sucessor.Board.Add(torre);
                    sucessor.Board = sucessor.Board.OrderBy(o => o.posicao).ToList();
                    sucessor.Cost += AvailableSpaces.Count() + posicao + k;
                    sucessor.NumeroDeAtaques = 0;
                    if(!sucessor.SolucaoCompleta())
                        {
                            allNodesBoardPieces.Add(sucessor.Board.Count());
                            sucessores.Add(sucessor);
                        }
                    else
                        allNodesBoardPieces.Add(sucessor.Board.Count());
                        AddResult(sucessor.Board.Count());
                }
                currentTower = currentTower.Next();
            }

            // If we cant place any tower in this position, remove it from the AvailableSpaces
            if(canNotPlaceCount >= 3)
            {
                AvailableSpaces.Remove(pos);
            }
        }

        // AddResult(Board.Count());
        base.Sucessores(sucessores);
        return sucessores;
        }


    public bool CanPlaceTower(List<PlaceInBoard> board, Towers tower, int posicao, int boardSize){
        if(board.Where(w => w.posicao == posicao).Count() > 0) return false; // se existe uma torre nessa posição retorna falso
        return CheckBoundaries(board, tower, posicao, boardSize);
    }

    public bool CheckBoundaries(List<PlaceInBoard> board, Towers tower, int posicao, int boardSize, int nivel = 0)
    {
        //TODO: Check piece if in the same line and column. But only once to avoid infinite loops

        // Procura por cada casa na linha da posicao e por pela torre que está a tentar meter no board
        int linha = posicao / boardSize;
        int coluna = posicao % boardSize;

        List<PlaceInBoard> sameTowerCheck = new List<PlaceInBoard>();

        List<PlaceInBoard> aTowerCheck = new List<PlaceInBoard>();
        List<PlaceInBoard> bTowerCheck = new List<PlaceInBoard>();
        List<PlaceInBoard> cTowerCheck = new List<PlaceInBoard>();
        //percorre linha da posicao, iniciando no inicio da linha, esquerda para direita
        for (int i = 0; i < boardSize; i++)
        {
            int boardPos = linha * boardSize + i;
            if (boardPos != posicao)
            {
                sameTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == tower).ToList());
                if(sameTowerCheck.Count() > 0)
                    return false;
                
                aTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == Towers.A).ToList());
                
                if (aTowerCheck.Count() > 0 && nivel == 0)
                {    
                    NumeroDeAtaques++;
                    if(!CheckBoundaries(board, tower, aTowerCheck.First().posicao, boardSize, 1)) return false;
                }
                bTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == Towers.B).ToList());
                
                if (bTowerCheck.Count() > 0 && nivel == 0)
                {    
                    NumeroDeAtaques++;
                    if(!CheckBoundaries(board, tower, bTowerCheck.First().posicao, boardSize, 1)) return false;
                }
                cTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == Towers.C).ToList());
                
                if (cTowerCheck.Count() > 0 && nivel == 0)
                {    
                    NumeroDeAtaques++;
                    if(!CheckBoundaries(board, tower, cTowerCheck.First().posicao, boardSize, 1)) return false;
                }

                if(aTowerCheck.Count() > 1 || bTowerCheck.Count() > 1 || cTowerCheck.Count() > 1)
                    return false;
            }
        }

        for (int j = 0; j < boardSize; j++)
        {
            int boardPos = j * boardSize + coluna;
            if(boardPos != posicao){
                sameTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == tower).ToList());
                if(sameTowerCheck.Count() > 0)
                    return false;
                
                aTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == Towers.A).ToList());
                if (aTowerCheck.Count() > 0 && nivel == 0)
                {    
                    NumeroDeAtaques++;
                    if(!CheckBoundaries(board, tower, aTowerCheck.First().posicao, boardSize, 1)) return false;
                }
                bTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == Towers.B).ToList());
                if (bTowerCheck.Count() > 0 && nivel == 0)
                {    
                    NumeroDeAtaques++;
                    if(!CheckBoundaries(board, tower, bTowerCheck.First().posicao, boardSize, 1)) return false;
                }
                cTowerCheck.AddRange(board.Where(w => w.posicao == boardPos && w.cor == Towers.C).ToList());
                if (cTowerCheck.Count() > 0 && nivel == 0)
                {    
                    NumeroDeAtaques++;
                    if(!CheckBoundaries(board, tower, cTowerCheck.First().posicao, boardSize, 1)) return false;
                }

                if(aTowerCheck.Count() > 1 || bTowerCheck.Count() > 1 || cTowerCheck.Count() > 1)
                    return false;
            }
        }

        return true;
    }

    public override void SolucaoVazia(){ Board.Clear(); }

    public override bool SolucaoCompleta() { return AvailableSpaces.Count() == 0; }
    public override void Debug() {
        Console.WriteLine();
        for (int i = 0; i < BoardSize; i++)
        {
            
                Console.WriteLine();
                for (var j = 0; j < BoardSize; j++)
                {
                    int cor=43;
                    int boardPos = i * BoardSize + j;                    
                    
                    PlaceInBoard place = Board.Find(f => f.posicao == boardPos);
                    if(place.posicao == boardPos && place.cor != 0)
                        Console.Write("{0} ", (char)place.cor);
                    else Console.Write("{0} ",(char)cor);

                    
                }
        }
        Console.WriteLine();
        Console.WriteLine("Número de peças no board: {0}", Board.Count());
        Console.WriteLine("Available Spaces: {0}", AvailableSpaces.Count());
    }

public override bool IsDuplicate(ProcuraConstrutiva currentNode, List<ProcuraConstrutiva> visitados)
    {
        Torres node = (Torres)currentNode;

        List<Torres> marked = new List<Torres>();
        foreach (var item in visitados)
        {
            marked.Add((Torres) item);
        }
        return VerificaDuplicados(node, marked).Count() > 0;
    }

    public List<ProcuraConstrutiva> VerificaDuplicados(Torres currentNode, List<Torres> visitados){
        
        return visitados.Where<Torres>(x => 
                x.Board.SequenceEqual(currentNode.Board) 
                // x.Board.SequenceEqual(transposta)
                //x.Board.SequenceEqual(Utils.SymmetricMatrix(currentNode.Board, transposta, BoardSize))
            ).ToList<ProcuraConstrutiva>();
    }

public override int Heuristica()
{
	base.Heuristica();   
	return NumeroDeAtaques - Board.Count() + AvailableSpaces.Count();
}
    public override int GetResult(){ return Board.Count(); }
    public override void Teste(){
        Console.Clear();
        while(true){
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme  [Procuras Cegas]");
            Console.WriteLine("[4] - A Star           | [5] - Best First Search*    | [6] - Greedy Search*  [Procuras Informadas]");
            Console.WriteLine("[8] - Definir N({0})   | [9] - Debug ({1})                                   [Configurações]", BoardSize, debug);
            Console.WriteLine("[0] - Sair                                                                          [Sistema]");
            Console.WriteLine("---------------------------------------  Estatísticas  --------------------------------------");
            Console.WriteLine("Expansoes: {0}                           Geracoes: {1}                        Avaliacoes: {2}", expansoes, geracoes, avaliacoes);
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
                    SetTimer();
                    Console.WriteLine("A Star: " + AStar(priorityQueue, visitados).ToString());
                    aTimer.Stop();
                    break;
                case "8": 
                    Console.Write("Definir N: ");
                    if(!int.TryParse(Console.ReadLine(), out int n))
                        Console.WriteLine("Invalid value entered");
                    else
                        BoardSize = n;
                    break;
                case "9": 
                    Console.Write("Definir debug: ");
                    if(!int.TryParse(Console.ReadLine(), out int d))
                        Console.WriteLine("Invalid value entered");
                    else
                        debug = d;
                    break;
                case "0": 
                    if(aTimer != null)
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