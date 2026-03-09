namespace Bachelor.Models;

public abstract class Algorithm
{
    private int funcEvals { get; set; }
    private double runtime { get; set; }
    private int BSFF { get; set; }

    private string inputString = "";
    private string inputGraph = "";

    private string searchPointString = "";
    private string searchPointGraph = "";
    
    public abstract void Run();
}