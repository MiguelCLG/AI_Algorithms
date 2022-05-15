/*
    Author: Miguel Gonçalves
    Class: Oito Damas
    Description: Solves the N Queens problem
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/
class OitoDamas : ProcuraConstrutiva, ICloneable {

	public List<int> damas {get ; private set; }
    public int Cost {get; set;}
    const int NUMERO_DE_DAMAS = 10;
    public OitoDamas(){
        damas = new List<int>();
        Cost = 0;
    }
    
    public OitoDamas(List<int> a){
        damas = a;
    }
    ~OitoDamas(){}
    public void TesteOitoDamas(){
        Console.WriteLine("OITO DAMAS");
        Teste();
    }

    public object Clone()
    {
        return new OitoDamas(damas);
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores){

        int novaLinha=damas.Count();
        for(int i=0;i<NUMERO_DE_DAMAS;i++) {
            int j=0;
            for(;j<novaLinha;j++)
                if(i==damas[j] || i==damas[j]+(novaLinha-j) || i==damas[j]-(novaLinha-j))
                    break;
            if(j==novaLinha) {
                OitoDamas sucessor = Extensions.Clone(this); // Object Extensions is an extension class to the System namespace that will allow us to instanciate a new object from itself
                sucessor.damas.Add(i);
                sucessor.Cost = -(NUMERO_DE_DAMAS + i);
                sucessores.Add(sucessor);
            }
        }
        AddResult(damas.Count());
        Expand(sucessores);

        return sucessores;
    }
	public override void SolucaoVazia() { damas.Clear(); }
	public override bool SolucaoCompleta() { 
        return damas.Count()==NUMERO_DE_DAMAS; 
    }

    public override void Debug()
    {
        for(int i=0;i<NUMERO_DE_DAMAS;i++) {
            Console.WriteLine();
            for(int j=0;j<NUMERO_DE_DAMAS;j++) {
                int cor=((i+j)%2 > 0 ? 32:35);  // 176:178  --- jm 03/4/2018
                if(damas.Count()>i && damas[i]==j)
                    Console.Write("{0}{0}",(char)88);  // 16:17   --- jm 03/4/2018
                else Console.Write("{0}{0}",(char)cor);
            }
        }
        Console.WriteLine();
        Console.WriteLine("Damas: " + damas.Count());
        Console.WriteLine();
    }
}