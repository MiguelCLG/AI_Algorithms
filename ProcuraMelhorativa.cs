class ProcuraMelhorativa : ProcuraConstrutiva {
    
    public int custo = -1;
    public bool movePrimeiro = true;
    public virtual bool Parar() { return limiteAvaliações<=avaliacoes || custo==0; }

    void Copiar(ProcuraMelhorativa objecto) {
        custo=objecto.custo;
    }
    
    public override void NovaSolucao(){}
    public int EscaladaDoMonte(bool movePrimeiro)
    {
        int procuras=0;
        Avaliar(); // avaliar o proprio
        // gerar uma solucao aleatoria
        ProcuraMelhorativa solucao=(ProcuraMelhorativa)Extensions.Clone(this);
        while(!Parar()) { // iniciar num qualquer ponto e escalar o monte mais perto
            if(debug>1)
                Console.WriteLine("Escalada {0} ({1}/{2}/{3})",++procuras,geracoes,expansoes,avaliacoes);
            solucao.NovaSolucao();
            solucao.Avaliar();
            if(debug>2)
                Console.Write(" base {0}",solucao.custo);
            while(!Parar()) { // escalar o monte mais perto
                if(movePrimeiro) {
                    // avanca logo que encontre um vizinho melhor
                    //(nao olha em todas as direccoes antes de comecar a subir)
                    List<ProcuraMelhorativa> vizinhos = new List<ProcuraMelhorativa>();
                    vizinhos = solucao.Vizinhanca(vizinhos);
                    while(vizinhos.Count()>0)
                        if(solucao.custo > vizinhos.Last().Avaliar()) {
                            ProcuraMelhorativa trocar=solucao;
                            solucao=vizinhos.Last();
                            vizinhos.Remove(vizinhos.Last());
                            vizinhos.Add(trocar);
                            // verificar se esta solucao e melhor que a actual
                                solucao.Debug();
                            if(custo>solucao.custo) {
                                Copiar(solucao);
                                //DebugMelhorEncontrado();
                            }
                            break;
                        } else vizinhos.Remove(vizinhos.Last());
                    // percorridos todos os vizinhos e nao ha nenhum melhor
                    // altura para recomecar de novo
                    if(debug>2)
                        Console.Write(" topo {0}",solucao.custo);
                    break;
                } else { // analisa todos os vizinhos e avanca apenas para o melhor vizinho
                    List<ProcuraMelhorativa> vizinhos = new List<ProcuraMelhorativa>();
                    int melhorCusto = -1;
                    int melhorVizinho=-1;
                    vizinhos = solucao.Vizinhanca(vizinhos);
                    for(int i=0;i<vizinhos.Count();i++) {
                        vizinhos.ElementAt(i).Avaliar();
                        if(i==0 || melhorCusto>vizinhos.ElementAt(i).custo) {
                            melhorCusto=vizinhos.ElementAt(i).custo;
                            melhorVizinho=i;
                        }
                    }
                    // trocar caso melhore
                    if(melhorVizinho>=0 && melhorCusto < solucao.custo) {
                        ProcuraMelhorativa trocar=solucao;
                        solucao=vizinhos[melhorVizinho];
                        vizinhos[melhorVizinho]=trocar;
                        // verificar se esta solucao e melhor que a actual
                        if(custo>solucao.custo) {
                            Copiar(solucao);
                            solucao.Debug();
                           /*  DebugMelhorEncontrado(); */
                        }
                    } else {
                        // nao melhorou, recomecar
                        if(debug>2)
                            Console.Write(" topo %d",solucao.custo);
                        break;
                    }
                }
            }
        }
        return custo;
    }
    public virtual int Avaliar(){ avaliacoes++; return 0;}
    public virtual List<ProcuraMelhorativa> Vizinhanca(List<ProcuraMelhorativa> vizinhos)
    {
        expansoes++;
        geracoes+=vizinhos.Count();
        return vizinhos;
    }
}