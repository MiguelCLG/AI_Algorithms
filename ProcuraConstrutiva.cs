/*
    Author: Miguel Gonçalves
    Class: Procura Construtiva
    Description: Contains search algorithms like:
        - Breadth First Search
        - Depth First Search
        - Uniform cost
        - AStar
        - Sofrega
        - Melhor Primeiro
    Notes: * has no implementation yet
*/

//TODO: Review Profundidade Primeiro algorithm

using System;
using System.Timers;

class ProcuraConstrutiva {
#region Estrutura de dados
    const int TIMER_LIMIT = 60000;
    private static DateTime timerStart;
    public static int expansoes = 0;
    public static int geracoes = 0;
    public static int avaliacoes = 0;

    public static int limiteAvaliações = 0;
    public static int limiteNivel = 10;
    public static List<int> allNodesBoardPieces = new List<int>();
    public static List<int> DebugAvailableSpaces = new List<int>();
    public static List<int> results = new List<int>();
    public static int debug = 0;
    public static System.Timers.Timer aTimer;
    public virtual int Cost {get; set;}

    public int heuristica = 0;
    public static List<ProcuraConstrutiva> visitados = new List<ProcuraConstrutiva>();

    private static bool time = false;
    
    //Em termos de definição, o BFS usa uma fila (queue) para correr o algoritmos
    public static Queue<ProcuraConstrutiva> queue = new Queue<ProcuraConstrutiva>();

    //Em termos de definição, o UCS usa uma fila prioritaria (queue) para correr o algoritmos, usa o custo para ordernar por prioridade
    public static PriorityQueue<ProcuraConstrutiva, int> priorityQueue = new PriorityQueue<ProcuraConstrutiva, int>();

    // Em termos de definição, o DFS usa uma pilha (stack) para correr o seu algoritmo de recursão. Como Stack tem uma função de Pop (retira o ultimo elemento da pilha), usamos este em vez de lista
    public static Stack<ProcuraConstrutiva> stack = new Stack<ProcuraConstrutiva>();
#endregion

#region Algorithms
    public int LarguraPrimeiro()
    {
        queue.Enqueue(this);
        while(queue.Count() > 0){
            if(time) 
            {
                DebugResultado();
                return -1;
            }
            ProcuraConstrutiva currentNode = queue.Dequeue();
            if(!currentNode.IsDuplicate(currentNode, visitados)){
                if(currentNode.SolucaoCompleta())
                {
                    currentNode.Debug();
                    Console.WriteLine("expansoes: {0} geracoes: {1}", expansoes, geracoes);
                    DebugResultado();
                    return currentNode.GetResult();
                }
                else
                {
                    List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                    sucessores = currentNode.Sucessores(sucessores);

                    if(debug > 0) currentNode.Debug();
                    visitados.Add(currentNode);
                    foreach (ProcuraConstrutiva sucessor in sucessores)
                    {
                        queue.Enqueue(sucessor);                
                    }
                    
                }
            }
        }
        DebugResultado();
        return -1;
    }

    public int ProfundidadePrimeiro(Stack<ProcuraConstrutiva> stack, List<ProcuraConstrutiva> visitados)
    {
        while (stack.Count() > 0)
        {
            if(time) 
                {
                    DebugResultado();
                    return -1;
                }
            ProcuraConstrutiva currentNode = stack.Pop();
            // Verifica se o node atual tem a solução
            if(currentNode.SolucaoCompleta())
            {
                currentNode.Debug();
                DebugResultado();
                return currentNode.GetResult();
            }
            else{
                // Verifica se o node não foi marcado como visitado
                // Se não, então percorre o algoritmo e adiciona os filhos ao stack
                // se foi visitado, então vai descartar esse node e vai ao seguinte no stack

                if(!currentNode.IsDuplicate(currentNode, visitados)){
                    List<ProcuraConstrutiva> nodes = new List<ProcuraConstrutiva>();
                    nodes = currentNode.Sucessores(nodes);
                    visitados.Add(currentNode);
                    if(debug > 0) currentNode.Debug();

                    foreach (ProcuraConstrutiva sucessor in nodes)
                    {
                        stack.Push(sucessor);
                    }
                    int result = ProfundidadePrimeiro(stack, visitados);
                    if(result > 0)
                        return result;
                }
            }
        }
        DebugResultado();
        return -1; // nao encontrou solução
    }
    
    public int CustoUniforme(PriorityQueue<ProcuraConstrutiva, int> priorityQueue, List<ProcuraConstrutiva> visitados)
    {
        priorityQueue.Enqueue(this, 0);
        while (priorityQueue.Count > 0) {
            if(time) 
            {
                DebugResultado();
                return -1;
            }
            ProcuraConstrutiva currentNode = priorityQueue.Dequeue();
            if(!currentNode.IsDuplicate(currentNode, visitados)){
                if(currentNode.SolucaoCompleta())
                {
                    currentNode.Debug();
                    Console.WriteLine("Expansoes: {0} Geracoes: {1}", expansoes, geracoes);
                    DebugResultado();
                    return currentNode.GetResult();
                }
                else
                {
                    List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                    sucessores = currentNode.Sucessores(sucessores);
                    visitados.Add(currentNode);
                    if(debug > 0) currentNode.Debug();

                    foreach (ProcuraConstrutiva sucessor in sucessores)
                    {
                        priorityQueue.Enqueue(sucessor, sucessor.Distancia());
                    }
                }
            }
        }
        DebugResultado();
        return -1;
    }

    public int AStar (PriorityQueue<ProcuraConstrutiva, int> priorityQueue, List<ProcuraConstrutiva> visitados)
    {
        visitados = new List<ProcuraConstrutiva>();
        priorityQueue.Enqueue(this, 0);
        while(priorityQueue.Count > 0)
        {
            if(time) 
            {
                DebugResultado();
                return -1;
            }

            ProcuraConstrutiva currentNode = priorityQueue.Dequeue();

                if(currentNode.SolucaoCompleta())
                {
                    currentNode.Debug();
                    Console.WriteLine("Expansoes: {0} Geracoes: {1}", expansoes, geracoes);
                    DebugResultado();
                    return currentNode.GetResult();
                }
                else
                {
                    List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                    sucessores = currentNode.Sucessores(sucessores);
                    visitados.Add(currentNode);
                    if(debug > 0) currentNode.Debug();

                    if(avaliacoes <= limiteAvaliações || limiteAvaliações == 0) 
                        foreach (ProcuraConstrutiva sucessor in sucessores)
                        {
                            priorityQueue.Enqueue(sucessor, Math.Max(currentNode.Heuristica(), sucessor.Distancia() + sucessor.Heuristica()));
                        }
                }
        }
        DebugResultado(); 
        return -1;
    }

    public int Sofrega (PriorityQueue<ProcuraConstrutiva, int> priorityQueue, List<ProcuraConstrutiva> visitados)
    {
        visitados = new List<ProcuraConstrutiva>();
        priorityQueue.Enqueue(this, 0);
        while(priorityQueue.Count > 0)
        {
            if(time) 
            {
                DebugResultado();
                return -1;
            }

            ProcuraConstrutiva currentNode = priorityQueue.Dequeue();
            if(currentNode.SolucaoCompleta())
            {
                currentNode.Debug();
                Console.WriteLine("Expansoes: {0} Geracoes: {1}", expansoes, geracoes);
                DebugResultado();
                return currentNode.GetResult();
            }
            else
            {
                List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                sucessores = currentNode.Sucessores(sucessores);
                visitados.Add(currentNode);
                if(debug > 0) currentNode.Debug();

                foreach (ProcuraConstrutiva sucessor in sucessores)
                {
                    priorityQueue.Enqueue(sucessor, Math.Max(currentNode.Heuristica(), sucessor.Heuristica()));
                }
            }

        }

        DebugResultado();
        return -1;
    }

    public int MelhorPrimeiro(int nivel)
    {
        if(time) 
            {
                DebugResultado();
                return -1;
            }
            ProcuraConstrutiva currentNode = stack.Pop();
            // Verifica se o node atual tem a solução
            if(currentNode.SolucaoCompleta())
            {
                currentNode.Debug();
                DebugResultado();
                return currentNode.GetResult();
            }
            if(nivel<0 || nivel>1){
                // Verifica se o node não foi marcado como visitado
                // Se não, então percorre o algoritmo e adiciona os filhos ao stack
                // se foi visitado, então vai descartar esse node e vai ao seguinte no stack

                List<ProcuraConstrutiva> nodes = new List<ProcuraConstrutiva>();
                nodes = currentNode.Sucessores(nodes);
                visitados.Add(currentNode);
                
                if(debug > 0) currentNode.Debug();
                
                
                foreach (var node in nodes)
                {
                    node.heuristica = node.Heuristica();
                }
                nodes.OrderBy(s => s.heuristica);
                foreach (ProcuraConstrutiva sucessor in nodes)
                {
                    stack.Push(sucessor);
                    if(avaliacoes <= limiteAvaliações || limiteAvaliações == 0){
                        int resultado = sucessor.MelhorPrimeiro(nivel - 1);
                        if(resultado >= 0){
                            return resultado;
                        }
                    }
                }
            }
        return -1; // nao encontrou solução
    }

public int BestFirst (int nivel) {
	if(SolucaoCompleta()) { // um no objectivo!
		Debug();
        DebugResultado();
        return GetResult();
	}
	if(nivel<0 || nivel>1) { // continuar a procura
		// ainda nao e o no objectivo
		List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();
		List<int> custo = new List<int>();
		List<int> id = new List<int>();
		sucessores = Sucessores(sucessores);
		// calcular as heuristicas
		
        List<int> heuristicas = new List<int>();
        foreach(var sucessor in sucessores)
            heuristicas.Add(sucessor.Heuristica());
        heuristicas.Remove(-1);
        sucessores.Remove(null);
        custo.Remove(-1);
        heuristicas.OrderByDescending(o => o);
		
		// tentar todo os sucessores, um de cada vez
        foreach (var sucessor in sucessores){
			int resultado=sucessor.BestFirst(nivel-1);
			if(resultado>=0) { // este sucessor resolveu o problema, devolver
				sucessor.Debug();
                sucessor.DebugResultado();
                return resultado;
			}
		}
        sucessores.Clear();
	}
	// percorreram-se todos os sucessores e nada (ou atingiu-se o limite), devolver -1
	return -1;
    }

#endregion

#region Utils

    public virtual int Distancia(){
        return 1;
    }

    public virtual int Heuristica(){
        avaliacoes++;
        return 0;
    }

    public void Expand(List<ProcuraConstrutiva> sucessores){
        geracoes++;
        expansoes += sucessores.Count();
    }
    
    public void AddResult(int value){
        results.Add(value);
    }

    public virtual List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores) { 
        geracoes++;
        expansoes += sucessores.Count();
        return sucessores; 
        }

    public virtual void SolucaoVazia() {}

	public virtual bool SolucaoCompleta() { return false; }    

    public virtual void Debug(){}
    public virtual void DebugResultado(){
        List<int> distintos = results.Distinct().ToList<int>();
        List<int> allNodes = allNodesBoardPieces.Distinct().ToList();
        List<int> availableSpaces = DebugAvailableSpaces.Distinct().ToList();

        allNodes.Sort();
        availableSpaces.Sort();
        
        Console.WriteLine("Tres melhores resultados: [ {0} {1} {2} ]", allNodes[allNodes.Count() - 1], allNodes[allNodes.Count() - 2], allNodes[allNodes.Count() - 3]);
        Console.Write("resultados : [");
        foreach (int result in distintos)
        {
            Console.Write(" {0} ", result);
        }
        Console.WriteLine("]");
    }

    public virtual int GetResult(){ return 2; }
    public virtual bool IsDuplicate(ProcuraConstrutiva currentNode, List<ProcuraConstrutiva> visitados){ return false; }

    public void LimpaTudo(){
        stack.Clear();
        queue.Clear();
        stack.Push(this);
        results.Clear();
        allNodesBoardPieces.Clear();
        priorityQueue.Clear();
        visitados.Clear();
        expansoes = 0;
        time = false;
        geracoes = 0;
        avaliacoes = 0;
        SolucaoVazia();
        GC.Collect();
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
    public static void DebugTimer() {
        TimeSpan currentTimer = DateTime.Now - timerStart;
        Console.WriteLine("Time Elapsed: {0}s", Math.Abs(currentTimer.TotalSeconds));
    }
#endregion

    public virtual void Teste(){ GC.Collect();}
}