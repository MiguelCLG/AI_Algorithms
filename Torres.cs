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
    public override int Cost { get; set;}
    public Torres(int n = 4){
        Board = new List<PlaceInBoard>();
        BoardSize = n;
        AvailableSpaces = new List<int>(Utils.FillCleanBoard(BoardSize));
        Cost = 0;
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
                    if(!Utils.CanPlaceTower(Board, currentTower, pos, BoardSize)){
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
                    if(!sucessor.SolucaoCompleta())
                        sucessores.Add(sucessor);
                    else
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

    public override void SolucaoVazia(){ Board.Clear(); }

    public override bool SolucaoCompleta() { return AvailableSpaces.Count() == 1; }
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

    public override int GetResult(){ return Board.Count(); }
    public override void Teste(){
        Console.Clear();
        while(true){
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme  [Procuras Cegas]");
            Console.WriteLine("[4] - Definir N({0})   | [5] - Debug ({1})                                   [Configurações]", BoardSize, debug);
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