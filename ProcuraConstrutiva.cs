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

        return -1;
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
    public void Teste(){
        SolucaoVazia();
        while(true){
            Console.Clear();
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("------------------------------Algoritmos Inteligencia Artificial-----------------------------");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] - Largura Primeiro | [2] - Profundidade Primeiro | [3] - Custo Uniforme* [Procuras Cegas]");
            Console.WriteLine("[0] - Sair                                                                          [Sistema]");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.Write("Opcao: ");
            
            string op = Console.ReadLine();
            switch(op) {
                case "1": 
                    Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
                    break;
                case "2": 
                    Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
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