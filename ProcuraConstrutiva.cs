/*
    Author: Miguel Gonçalves
    Class: Procura Construtiva
    Description: Contains search algorithms like:
        - Breadth First Search
        - Depth First Search *
        - Uniform cost *
    Notes: * has no implementation yet
*/

using System;

class ProcuraConstrutiva {
    public static int expansoes = 0;
    public static int geracoes = 0;
    public static List<int> results = new List<int>();
    public List<ProcuraConstrutiva> visitados = new List<ProcuraConstrutiva>();

    //Em termos de definição, o BFS usa uma fila (queue) para correr o algoritmos, uma lista é tecnicamente a mesma coisa e mais simples de usar
    public List<ProcuraConstrutiva> queue = new List<ProcuraConstrutiva>();

    // Em termos de definição, o DFS usa uma pilha (stack) para correr o seu algoritmo de recursão. Como Stack tem uma função de Pop (retira o ultimo elemento da pilha), usamos este em vez de lista
    public Stack<ProcuraConstrutiva> stack = new Stack<ProcuraConstrutiva>();
    public int LarguraPrimeiro()
    {
        queue.Add(this);
        for (int i = 0; i< queue.Count(); i++) {
            if(queue[i].SolucaoCompleta())
            {
                queue.Last().Debug();
                Console.WriteLine("expansoes: {0} geracoes: {1}", expansoes, geracoes);
                return results.Last();
            }
            else
            {
                List<ProcuraConstrutiva> sucessores = new List<ProcuraConstrutiva>();

                sucessores = queue[i].Sucessores(sucessores);

                queue[i].Debug();
                queue.AddRange(sucessores);
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
                    nodes = currentNode.Sucessores(nodes);
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
    public void Expand(List<ProcuraConstrutiva> sucessores){
        geracoes++;
        expansoes += sucessores.Count();
    }

    public void AddResult(int value){
        results.Add(value);
    }

    virtual public List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores) {
        return sucessores;
    }

    public virtual void SolucaoVazia() {}
	public virtual bool SolucaoCompleta() { return false; }    
    virtual public void Debug(){}

    void LimpaTudo(){
        stack.Clear();
        queue.Clear();
        stack.Push(this);
        visitados.Clear();
        expansoes = 0;
        geracoes = 0;
    }

    public void Teste(){
        Console.Clear();
        while(true){
            SolucaoVazia();
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme* [Procuras Cegas]");
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