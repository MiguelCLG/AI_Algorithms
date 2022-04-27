/*
    Author: Miguel Gonçalves
    Class: Procura Construtiva
    Description: Contains search algorithms like:
        - Breadth First Search
        - Depth First Search
        - Uniform cost
        - AStar *
    Notes: * has no implementation yet
*/

//TODO: Results array not returning number of queens

using System;
using System.Timers;

class ProcuraConstrutiva {
#region Estrutura de dados
    const int TIMER_LIMIT = 60000;
    public static int expansoes = 0;
    public static int geracoes = 0;
    public static List<int> results = new List<int>();
    private static int debug = 0;
    private static System.Timers.Timer aTimer;
    private int cost = 0;

    public List<ProcuraConstrutiva> visitados = new List<ProcuraConstrutiva>();

    //Em termos de definição, o BFS usa uma fila (queue) para correr o algoritmos
    public Queue<ProcuraConstrutiva> queue = new Queue<ProcuraConstrutiva>();

    //Em termos de definição, o UCS usa uma fila prioritaria (queue) para correr o algoritmos, usa o custo para ordernar por prioridade
    public PriorityQueue<ProcuraConstrutiva, int> priorityQueue = new PriorityQueue<ProcuraConstrutiva, int>();

    public virtual List<int> Board { get; set; }

    public virtual int BoardSize { get; set; }
    public virtual int CheckersPerLine { get; set; }

    private bool time = false;
    // Em termos de definição, o DFS usa uma pilha (stack) para correr o seu algoritmo de recursão. Como Stack tem uma função de Pop (retira o ultimo elemento da pilha), usamos este em vez de lista
    public Stack<ProcuraConstrutiva> stack = new Stack<ProcuraConstrutiva>();
#endregion
#region Algorithms
    public int LarguraPrimeiro()
    {
        queue.Enqueue(this);
        while(queue.Count() > 0){
            //if(time >= TIMER_LIMIT) return -1;
            ProcuraConstrutiva currentQueueItem = queue.Dequeue();
            List<ProcuraConstrutiva> duplicados = visitados.Where<ProcuraConstrutiva>(x => 
            x.Board.SequenceEqual(currentQueueItem.Board) || x.Board.SequenceEqual(Utils.TranspostaMatrix(currentQueueItem.Board, BoardSize))
            ).ToList<ProcuraConstrutiva>();
            if(duplicados.Count() == 0){
                if(currentQueueItem.SolucaoCompleta())
                {
                    currentQueueItem.Debug();
                    Console.WriteLine("expansoes: {0} geracoes: {1}", expansoes, geracoes);
                    return currentQueueItem.Board.Count();
                }
                else
                {
                    List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                    sucessores = currentQueueItem.Sucessores(sucessores, cost);

                    if(debug > 0) currentQueueItem.Debug();
                    visitados.Add(currentQueueItem);
                    foreach (ProcuraConstrutiva sucessor in sucessores)
                    {
                        queue.Enqueue(sucessor);                
                    }
                    
                }
            }
        }
        return -1;
    }

    public int ProfundidadePrimeiro(Stack<ProcuraConstrutiva> stack, List<ProcuraConstrutiva> visitados)
    {

        while (stack.Count() > 0)
        {
            // Verifica se o node atual tem a solução
            ProcuraConstrutiva currentNode = stack.Pop();
            if(currentNode.SolucaoCompleta())
            {
                currentNode.Debug();
                return currentNode.Board.Count();
            }
            else{
                // Verifica se o node não foi marcado como visitado
                // Se não, então percorre o algoritmo e adiciona os filhos ao stack
                // se foi visitado, então vai descartar esse node e vai ao seguinte no stack
                if(!visitados.Contains(currentNode))
                {
                    List<ProcuraConstrutiva> nodes = new List<ProcuraConstrutiva>();
                    nodes = currentNode.Sucessores(nodes, cost);
                    visitados.Add(currentNode);
                    if(debug > 0) currentNode.Debug();

                    foreach (ProcuraConstrutiva sucessor in nodes)
                    {
                        stack.Push(sucessor);
                    }
                    int result = ProfundidadePrimeiro(stack, visitados);
                    if(result > 0)
                        return results.Last();
                }
            }
        }
        return -1; // nao encontrou solução
    }
    
    public int CustoUniforme(PriorityQueue<ProcuraConstrutiva, int> priorityQueue, List<ProcuraConstrutiva> visitados)
    {
        priorityQueue.Enqueue(this, 0);
        while (priorityQueue.Count > 0) {
            ProcuraConstrutiva currentElement = priorityQueue.Dequeue();
            if(currentElement.SolucaoCompleta())
            {
                currentElement.Debug();
                Console.WriteLine("Expansoes: {0} Geracoes: {1}", expansoes, geracoes);
                return results.Last();
            }
            else
            {
                List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                sucessores = currentElement.Sucessores(sucessores, geracoes);

                foreach (ProcuraConstrutiva sucessor in sucessores)
                {
                    priorityQueue.Enqueue(sucessor, sucessor.cost);
                }
            }
        }
        return -1;
    }
#endregion
#region Utils
    public void SetCost(int custo)
    {
        cost = custo;
    }
    
    public void Expand(List<ProcuraConstrutiva> sucessores){
        geracoes++;
        expansoes += sucessores.Count();
    }
    
    public void AddResult(int value){
        results.Add(value);
    }

    virtual public List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores, int custo) { return sucessores; }

    public virtual void SolucaoVazia() {}

	public virtual bool SolucaoCompleta() { return false; }    

    virtual public void Debug(){}

    void LimpaTudo(){
        stack.Clear();
        queue.Clear();
        stack.Push(this);
        priorityQueue.Clear();
        visitados.Clear();
        expansoes = 0;
        geracoes = 0;
    }
#endregion

   /* void Tick(int signalTime)
    {
        time = signalTime;
    }*/
    private void SetTimer()
    {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(1000);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);
        //if(e.SignalTime. > TIMER_LIMIT) time = true;
    }
    public void Teste(){
        Console.Clear();
        BoardSize = 4;
        CheckersPerLine = 2;
        while(true){
            SolucaoVazia();
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
            switch(op) {
                case "1": 
                    //SetTimer();
                    Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
                    //aTimer.Stop();
                    break;
                case "2": 
                    //SetTimer();
                    Console.WriteLine("Profundidade Primeiro: " + ProfundidadePrimeiro(stack, visitados).ToString());
                    //aTimer.Stop();
                    break;
                case "3": 
                    //SetTimer();
                    Console.WriteLine("Custo Uniforme: " + CustoUniforme(priorityQueue, visitados).ToString());
                    //aTimer.Stop();
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
                    System.Environment.Exit(0);
                    break;
                default: 
                    Console.WriteLine("Opção não válida!");
                    break;
            }
            //aTimer.Dispose();

        }
    }
}