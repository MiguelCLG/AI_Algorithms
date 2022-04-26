/*
    Author: Miguel Gonçalves
    Class: Procura Construtiva
    Description: Contains search algorithms like:
        - Breadth First Search
        - Depth First Search *
        - Uniform cost *
    Notes: * has no implementation yet
*/

//TODO: Results array not returning number of queens

using System;

class ProcuraConstrutiva {
#region Estrutura de dados
    public static int expansoes = 0;
    public static int geracoes = 0;
    public static List<int> results = new List<int>();
    private int cost = 0;
    public List<ProcuraConstrutiva> visitados = new List<ProcuraConstrutiva>();

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
        for (int i = 0; i< queue.Count(); i++) {
            if(queue.ElementAt(i).SolucaoCompleta())
            {
                queue.Last().Debug();
                Console.WriteLine("expansoes: {0} geracoes: {1}", expansoes, geracoes);
                return results.Last();
            }
            else
            {
                List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                sucessores = queue.ElementAt(i).Sucessores(sucessores, cost);

                queue.ElementAt(i).Debug();
                foreach (ProcuraConstrutiva sucessor in sucessores)
                {
                    queue.Enqueue(sucessor);                
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
                return results.Last();
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
    public void Teste(){
        Console.Clear();
        while(true){
            SolucaoVazia();
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme  [Procuras Cegas]");
            Console.WriteLine("[0] - Sair                                                                          [Sistema]");
            Console.WriteLine("---------------------------------------  Estatísticas  --------------------------------------");
            Console.WriteLine("Expansoes: {0}                Geracoes: {1}", expansoes, geracoes);
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.Write("Opcao: ");
            
            string op = Console.ReadLine();
            LimpaTudo();
            switch(op) {
                case "1": 
                    Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
                    break;
                case "2": 
                    Console.WriteLine("Profundidade Primeiro: " + ProfundidadePrimeiro(stack, visitados).ToString());
                    break;
                case "3": 
                    Console.WriteLine("Custo Uniforme: " + CustoUniforme(priorityQueue, visitados).ToString());
                    break;
                case "0": 
                    System.Environment.Exit(0);
                    break;
                default: 
                    Console.WriteLine("Opção não válida!");
                    break;
            }
        }
    }
}