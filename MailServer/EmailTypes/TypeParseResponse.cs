using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TypeParseResponse
{
    public bool IsMatch { get; set; }
    public int TotalHits { get; set; }
    public double MatchStrength { get; set; }

    public TypeParseResponse()
    {
    }
}