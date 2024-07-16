using System;
using System.Text.Json;

namespace InspiredCodes.ExpressMapper.Tests.Model;

public class Source
{

    public string ThisToJson()=> JsonSerializer.Serialize(this);

    public byte B1 { get; set; }
    public byte B2 { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime DateTimex { get; set; }
    public AnotherEnum EnumA { get; set; }
    public int[] IntArr { get; set; }
    public string String1 { get; set; }
    public bool YesOrNo { get; set; }
}
