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
//TODO: Create distinct function so we free this class from the inside of the successors

using System;
using System.Timers;

class ProcuraConstrutiva {
#region Estrutura de dados
    const int TIMER_LIMIT = 60000;
    private static DateTime timerStart;
    public static int expansoes = 0;
    public static int geracoes = 0;
    public static List<int> results = new List<int>();
    public static int debug = 0;
    public static System.Timers.Timer aTimer;
    private int cost = 0;

    public List<ProcuraConstrutiva> visitados = new List<ProcuraConstrutiva>();

    private static bool time = false;
    
    //Em termos de definição, o BFS usa uma fila (queue) para correr o algoritmos
    public Queue<ProcuraConstrutiva> queue = new Queue<ProcuraConstrutiva>();

    //Em termos de definição, o UCS usa uma fila prioritaria (queue) para correr o algoritmos, usa o custo para ordernar por prioridade
    public PriorityQueue<ProcuraConstrutiva, int> priorityQueue = new PriorityQueue<ProcuraConstrutiva, int>();

    // Em termos de definição, o DFS usa uma pilha (stack) para correr o seu algoritmo de recursão. Como Stack tem uma função de Pop (retira o ultimo elemento da pilha), usamos este em vez de lista
    public Stack<ProcuraConstrutiva> stack = new Stack<ProcuraConstrutiva>();
#endregion

#region Algorithms
    public int LarguraPrimeiro()
    {
        queue.Enqueue(this);
        while(queue.Count() > 0){
            if(time) return -1;
            ProcuraConstrutiva currentNode = queue.Dequeue();
            if(!IsDuplicate(currentNode, visitados)){
                if(currentNode.SolucaoCompleta())
                {
                    currentNode.Debug();
                    Console.WriteLine("expansoes: {0} geracoes: {1}", expansoes, geracoes);
                    return currentNode.GetResult();
                }
                else
                {
                    List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                    sucessores = currentNode.Sucessores(sucessores, cost);

                    if(debug > 0) currentNode.Debug();
                    visitados.Add(currentNode);
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
            if(time) return -1;
            ProcuraConstrutiva currentNode = stack.Pop();
            // Verifica se o node atual tem a solução
            if(currentNode.SolucaoCompleta())
            {
                currentNode.Debug();
                return currentNode.GetResult();
            }
            else{
                // Verifica se o node não foi marcado como visitado
                // Se não, então percorre o algoritmo e adiciona os filhos ao stack
                // se foi visitado, então vai descartar esse node e vai ao seguinte no stack

                if(!IsDuplicate(currentNode, visitados)){
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
                        return currentNode.GetResult();
                }
            }
        }
        return -1; // nao encontrou solução
    }
    
    public int CustoUniforme(PriorityQueue<ProcuraConstrutiva, int> priorityQueue, List<ProcuraConstrutiva> visitados)
    {
        priorityQueue.Enqueue(this, 0);
        while (priorityQueue.Count > 0) {
            if(time) return -1;
            ProcuraConstrutiva currentNode = priorityQueue.Dequeue();
            if(!IsDuplicate(currentNode, visitados)){
                if(currentNode.SolucaoCompleta())
                {
                    currentNode.Debug();
                    Console.WriteLine("Expansoes: {0} Geracoes: {1}", expansoes, geracoes);
                    return currentNode.GetResult();
                }
                else
                {
                    List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                    sucessores = currentNode.Sucessores(sucessores, geracoes);

                    foreach (ProcuraConstrutiva sucessor in sucessores)
                    {
                        priorityQueue.Enqueue(sucessor, (sucessor.cost + visitados.Count()) * -1);
                    }
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

    public virtual List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores, int custo) { return sucessores; }

    public virtual void SolucaoVazia() {}

	public virtual bool SolucaoCompleta() { return false; }    

    public virtual void Debug(){}

    public virtual int GetResult(){ return 2; }
    public virtual bool IsDuplicate(ProcuraConstrutiva currentNode, List<ProcuraConstrutiva> visitados){ return false; }

    public void LimpaTudo(){
        stack.Clear();
        queue.Clear();
        stack.Push(this);
        priorityQueue.Clear();
        visitados.Clear();
        expansoes = 0;
        time = false;
        geracoes = 0;
    }
#endregion

#region Timer
    public static void SetTimer()
    {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(10000);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += CreateTimer;
        timerStart = DateTime.Now;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    public static void CreateTimer(Object source, ElapsedEventArgs e) {
        TimeSpan currentTimer = DateTime.Now - timerStart;
        Console.WriteLine("Time Elapsed: {0}s", Math.Abs(currentTimer.TotalSeconds));
        if(currentTimer.TotalMilliseconds > TIMER_LIMIT)
            time = true;
    }
#endregion

    public virtual void Teste(){}
}