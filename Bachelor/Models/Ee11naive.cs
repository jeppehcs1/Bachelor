using System;
using System.Collections.Generic;

namespace test.Models;

public class Ee11naive
{
    public int Run (string bitString)
    {
        string stringtest = "00000";
        List<int> iterations = new List<int>();
        char[] c = stringtest.ToCharArray();
        char[] c2 = stringtest.ToCharArray();
        var random = new Random();
        while (Val(c2) != 5)
        {
            
            for (var i = 0; i < c.Length; i++)
            {
                if (random.Next(4) == 1)
                {
                    c[i] = (c[i].Equals('0')) ? '1' : '0';
                }
                
                
            }

            if (Val(c) >= Val(c2))
            {
                c.CopyTo(c2, 0);
            }
            iterations.Add(Val(c2));
            Console.WriteLine(c2);
        }
        Console.WriteLine(iterations.Count);
        return 0;
    }

    public int Val (char[] c)
    {
        var børge = 0;
        foreach (var bit in c)
        {
            if (bit.Equals('1'))
            {
                børge++;
            }
        }

        return børge;
    }
    
}