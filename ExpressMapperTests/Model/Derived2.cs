using System;
using System.Text.Json;

namespace InspiredCodes.ExpressMapper.Tests.Model;

public class Derived2 : IDerived2
{
    public string ThisAsIDerived2ToJson() => JsonSerializer.Serialize((IDerived2)this);

    public byte B1 { get; set; }
    public byte B2 { get; set; }
    public string Base { get; set; }
    public string BaseB { get; set; }
    public string BaseB_2 { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime DateTimex { get; set; }
    public string Derived_1 { get; set; }
    public string Derived_2 { get; set; }
    public AnotherEnum EnumA { get; set; }
    public AnotherEnum EnumX { get; set; }
    public int[] IntArr1 { get; set; }
    public int[] IntArr2 { get; set; }
    public bool YesOrNo { get; set; }
}
