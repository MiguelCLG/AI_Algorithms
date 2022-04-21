using System;

class ProcuraConstrutiva {
    public int expansoes = 0;
    public int geracoes = 0;
    public List<ProcuraConstrutiva> queue = new List<ProcuraConstrutiva>();
    public List<ProcuraConstrutiva> results = new List<ProcuraConstrutiva>();
    public int LarguraPrimeiro()
    {
        queue.Add(this);
        for (int i = 0; i< queue.Count(); i++) {
            if(queue[i].SolucaoCompleta())
            {
                queue.Last().Debug();
                Console.WriteLine("expansoes: {0} geracoes: {1}", expansoes, geracoes);
                return queue.Count();
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

    public void Expand(List<ProcuraConstrutiva> sucessores){
        geracoes++;
        expansoes += sucessores.Count();
    }
    virtual public List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores) {
        return sucessores;
    }

    public virtual void SolucaoVazia() {}
	public virtual bool SolucaoCompleta() { return false; }    
    virtual public void Debug(){}
    public void Teste(){
        SolucaoVazia();
        Console.WriteLine("Largura Primeiro: " + LarguraPrimeiro().ToString());
    }
}