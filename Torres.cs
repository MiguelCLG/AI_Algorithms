/*
    Author: Miguel Gonçalves
    Class: Torres
    Description: Tries to place as many towers (of different colours, marked as A B or C) as it can as long as they arent attacking
    the same colour or 2 or more of different colours.
    Notes: ICloneable is a derivative class that allows us to make a deep copy of an object 
*/

interface IPlaceInBoard {
    char cor { get; set;}
    int posicao { get; set; }
}

enum Towers
{
    A = 'A',
    B = 'B',
    C = 'C'
}

class Torres : ProcuraConstrutiva, ICloneable {

    public List<IPlaceInBoard> Tabuleiro { get; set; }
    public List<int> AvailableSpaces { get; set; }
    public int boardSize { get; set; }
    int CustoTotal { get; set; }

    public Torres(){
        Tabuleiro = new List<IPlaceInBoard>();
        boardSize = 4;
        AvailableSpaces = new List<int>(Utils.FillCleanBoard(boardSize));
        CustoTotal = 0;
    }
    
    public object Clone()
    {
        return new Torres();
    }

    public override List<ProcuraConstrutiva> Sucessores(List<ProcuraConstrutiva> sucessores, int custo) { return sucessores; }

    public override void SolucaoVazia(){}

    public override bool SolucaoCompleta() { return false; }
    public override void Debug() {}

}