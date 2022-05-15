/*
    Author: Miguel Gonçalves
    Class: Torres
    Description: Tries to place as many towers (of different colours, marked as A B or C) as it can as long as they arent attacking
    the same colour or 2 or more of different colours.
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

//TODO: Verificar sucessores, o que esta a acontecer na verificação?

using System.Runtime.CompilerServices;

class Torres : ProcuraConstrutiva {

    public List<PlaceInBoard> Board { get; set; }
    public List<int> AvailableSpaces { get; set; }
    public int BoardSize { get; set; }
    public int ConsistentCost {get; set;}
    public override int Cost { get; set;}

    public List<int> SpacesToRemove { get; set; }
    public Torres(){
        Board = new List<PlaceInBoard>();
        BoardSize = 4;
        AvailableSpaces = new List<int>();
        SpacesToRemove = new List<int>();
        Cost = 0;
        ConsistentCost = 0;
    }
    public Torres(int n){
        Board = new List<PlaceInBoard>();
        BoardSize = n;
        AvailableSpaces = new List<int>();
        SpacesToRemove = new List<int>();
        Cost = 0;
        ConsistentCost = 0;
    }
    


    public override int Heuristica()
{
	base.Heuristica();
	return AvailableSpaces.Count();
}

    public override int Distancia()
    {
        return ConsistentCost;
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
                        if(canNotPlaceCount >=3)
                        {
                            AvailableSpaces.Remove(pos);
                            //Se esta foi a ultima casa livre, entao re-adicionamos este node aos sucessores para reavaliar
                            if(SolucaoCompleta())
                                {
                                    sucessores.Add(this);
                                    return sucessores;
                                }
                        }
                        break;
                    }
                }

                if(j == boardCount)    //pode adicionar torre       
                {

                    //Torre a ser adicionada
                    PlaceInBoard torre = new PlaceInBoard();
                    torre.cor = currentTower;
                    torre.posicao = pos;

                    //setup do sucessor
                    Torres sucessor = (Torres)Extensions.Clone(this);
                    sucessor.AvailableSpaces.Remove(pos);
                    sucessor.Board.Add(torre);
                    sucessor.Board = sucessor.Board.OrderBy(o => o.posicao).ToList();
                    sucessor.Cost += AvailableSpaces.Count();
                    sucessor.ConsistentCost += 1;

                    //verifica se o sucessor já existe
                    if(!IsDuplicate(sucessor, visitados)){
                        allNodesBoardPieces.Add(sucessor.Board.Count());
                        sucessores.Add(sucessor);
                    }
                }
                currentTower = currentTower.Next();
            }

            // If we cant place any tower in this position, remove it from the AvailableSpaces
            /*  if(canNotPlaceCount >= 3)
            {
                AvailableSpaces.Remove(pos);
                if(SolucaoCompleta()) // se true temos de re-adicionar o sucessor pois este é uma solução completa
                { 
                    sucessores.Add(this);
                    base.AddResult(Board.Count());
                }
            } */
        }

       /*  if(SolucaoCompleta())
            AddResult(Board.Count()); */
        DebugAvailableSpaces.Add(AvailableSpaces.Count());
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

        int aTowerCheck = 0;
        int bTowerCheck = 0;
        int cTowerCheck = 0;
        //percorre linha da posicao, iniciando no inicio da linha, esquerda para direita
        for (int i = 0; i < boardSize; i++)
        {
            int boardPos = linha * boardSize + i;
            board.Contains(new PlaceInBoard(tower, boardPos));
            if (boardPos != posicao)
            {
                if(board.Contains(new PlaceInBoard(tower, boardPos)))
                    return false;
                                
                if (
                    (
                        board.Contains(new PlaceInBoard(Towers.A, boardPos)) ||
                        board.Contains(new PlaceInBoard(Towers.B, boardPos)) ||
                        board.Contains(new PlaceInBoard(Towers.C, boardPos))
                    )   && nivel == 0
                )
                {    
                    if(!CheckBoundaries(board, tower, boardPos, boardSize, 1)) return false;
                }
                    aTowerCheck += (board.FindAll(fa => fa.posicao == boardPos && fa.cor == Towers.A).Count());
                    bTowerCheck += (board.FindAll(fa => fa.posicao == boardPos && fa.cor == Towers.A).Count());
                    cTowerCheck += (board.FindAll(fa => fa.posicao == boardPos && fa.cor == Towers.A).Count());
                if(
                    aTowerCheck > 1 ||
                    bTowerCheck > 1 || 
                    cTowerCheck > 1
                )
                    return false;
            }
        }

        for (int j = 0; j < boardSize; j++)
        {
            int boardPos = j * boardSize + coluna;
            if(boardPos != posicao){
                if(board.Contains(new PlaceInBoard(tower, boardPos)))
                    return false;
                                
                if (
                    (
                        board.Contains(new PlaceInBoard(Towers.A, boardPos)) ||
                        board.Contains(new PlaceInBoard(Towers.B, boardPos)) ||
                        board.Contains(new PlaceInBoard(Towers.C, boardPos))
                    )   && nivel == 0
                )
                {    
                    if(!CheckBoundaries(board, tower, boardPos, boardSize, 1)) return false;
                }
                    aTowerCheck += (board.FindAll(fa => fa.posicao == boardPos && fa.cor == Towers.A).Count());
                    bTowerCheck += (board.FindAll(fa => fa.posicao == boardPos && fa.cor == Towers.A).Count());
                    cTowerCheck += (board.FindAll(fa => fa.posicao == boardPos && fa.cor == Towers.A).Count());
                if(
                    aTowerCheck > 1 ||
                    bTowerCheck > 1 || 
                    cTowerCheck > 1
                )
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
        if(debug == 3) Console.ReadKey();
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
            ).ToList<ProcuraConstrutiva>();
    }

    public override int GetResult(){ return Board.Count(); }
    public override void Teste(){
        Console.Clear();
        while(true){
            base.Teste();
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme   [Procuras Cegas]");
            Console.WriteLine("[4] - A Star           | [5] - Sofrega    | [6] - Melhor Primeiro        [Procuras Informadas]");
            Console.WriteLine("[8] - Definir N({0})   | [9] - Debug ({1}) | [10] - Limite Avaliacoes({2})     [Configurações]", BoardSize, debug, limiteAvaliações);
            Console.WriteLine("[11] - Limite Nível({0})                                                       [Configurações]", limiteNivel);
            Console.WriteLine("[0] - Sair                                                                           [Sistema]");
            Console.WriteLine("---------------------------------------  Estatísticas  --------------------------------------");
            Console.WriteLine("Expansoes: {0}                         Geracoes: {1}                        Avaliacoes: {2}", expansoes, geracoes, avaliacoes);
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.Write("Opcao: ");
            
            string op = Console.ReadLine();
            LimpaTudo();
            SolucaoVazia();
            AvailableSpaces = Utils.FillCleanBoard(BoardSize);
            switch(op) {
                case "1": 
                    SetTimer();
                    Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "2": 
                    SetTimer();
                    Console.WriteLine("Profundidade Primeiro: " + ProfundidadePrimeiro(stack, visitados).ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "3": 
                    SetTimer();
                    Console.WriteLine("Custo Uniforme: " + CustoUniforme(priorityQueue, visitados).ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "4": 
                    SetTimer();
                    Console.WriteLine("A Star: " + AStar(priorityQueue, visitados).ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "5": 
                    SetTimer();
                    Console.WriteLine("Sofrega: " + Sofrega(priorityQueue, visitados).ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "6": 
                    SetTimer();
                    Console.WriteLine("Melhor Primeiro: " + MelhorPrimeiro(limiteNivel).ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "20": 
                    SetTimer();
                    Console.WriteLine("Best First: " + BestFirst(limiteNivel).ToString());
                    aTimer.Stop();
                    DebugTimer();
                    break;
                case "8": 
                    Console.Write("Definir N: ");
                    if(!int.TryParse(Console.ReadLine(), out int n))
                        Console.WriteLine("Invalid value entered");
                    else
                        {
                            BoardSize = n;
                            AvailableSpaces = Utils.FillCleanBoard(BoardSize);
                        }
                    break;
                case "9": 
                    Console.Write("Definir debug: ");
                    if(!int.TryParse(Console.ReadLine(), out int d))
                        Console.WriteLine("Invalid value entered");
                    else
                        debug = d;
                    break;
                case "10":
                    Console.Write("Definir Limite de Avaliações: ");
                    if(!int.TryParse(Console.ReadLine(), out int limite))
                            Console.WriteLine("Invalid value entered");
                        else
                            {
                                limiteAvaliações = limite;
                            }
                        break;
                case "11":
                    Console.Write("Definir Limite de Nivel: ");
                    if(!int.TryParse(Console.ReadLine(), out int limNivel))
                            Console.WriteLine("Invalid value entered");
                        else
                            {
                                limiteNivel = limNivel;
                            }
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