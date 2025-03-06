namespace GameOfLife.Models.Options;

/// <summary>
/// Options for the simulation
/// </summary>
public class SimulationOptions {
    /// <summary>
    /// The number of previous states to keep in memory
    /// </summary>
    public int HistorySize { get; set; }
}