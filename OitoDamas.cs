class OitoDamas : ProcuraConstrutiva {

	private List<int> damas = new List<int>();

    public void TesteOitoDamas(){
        Console.WriteLine("OITO DAMAS");
        Teste();
    }

    ProcuraConstrutiva Duplicar()
    {
        OitoDamas clone=new OitoDamas();
        clone.damas=damas;
        return clone;
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores){
        int novaLinha=damas.Count();

        for(int i=0;i<8;i++) {
            int j=0;
            for(;j<novaLinha;j++)
                if(i==damas[j] || i==damas[j]+(novaLinha-j) || i==damas[j]-(novaLinha-j))
                    break;
            if(j==novaLinha) {
                OitoDamas sucessor=(OitoDamas)Duplicar();
                sucessor.damas.Add(i);
                sucessores.Add(sucessor);
            }
        }

        Expand(sucessores);

        return sucessores;
    }
	public override void SolucaoVazia() { damas.Clear(); }
	public override bool SolucaoCompleta() { return damas.Count()==8; }

    public override void Debug()
    {
        for(int i=0;i<8;i++) {
            Console.WriteLine();
            for(int j=0;j<8;j++) {
                int cor=((i+j)%2 > 0 ? 32:35);  // 176:178  --- jm 03/4/2018
                if(damas.Count()>i && damas[i]==j)
                    Console.Write("{0}{0}",(char)88);  // 16:17   --- jm 03/4/2018
                else Console.Write("{0}{0}",(char)cor);
            }
        }
        Console.WriteLine();
    }
}