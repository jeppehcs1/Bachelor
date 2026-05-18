using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;

public class Euclid2DTSPFileReader : ITSPFileReader
{
    public TSPInstance Read(string filepath)
    {
        
        using var sr = new StreamReader(filepath);
        // Read header
        var header = new Dictionary<string, string>();
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Trim() == "NODE_COORD_SECTION") break;
            var parts = line.Split(':', 2);
            if (parts.Length == 2)
                header[parts[0].Trim()] = parts[1].Trim();
        }
        // Validate header
        if (header.GetValueOrDefault("TYPE") != "TSP")
            throw new Exception("Unsupported type");
        if (!header.ContainsKey("DIMENSION"))
            throw new Exception("Missing DIMENSION in header");
        if (header.GetValueOrDefault("EDGE_WEIGHT_TYPE") != "EUC_2D")
            throw new Exception("Unsupported edge weight type");
        
        int dimension = int.Parse(header["DIMENSION"]);
        // Read coordinates
        var coords = new List<(int x, int y)>();
        var perm = new List<int>();
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Trim() == "EOF") break;
            var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                perm.Add(int.Parse(parts[0]));
                coords.Add(((int)double.Parse(parts[1], CultureInfo.InvariantCulture), (int)double.Parse(parts[2], CultureInfo.InvariantCulture)));
            }
            
        }
        if (coords.Count != dimension)
            throw new Exception($"Expected {dimension} coordinates, got {coords.Count}");
        return new TSPInstance(perm, coords);
        
        
    }
    
}