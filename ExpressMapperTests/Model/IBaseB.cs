using System;

namespace InspiredCodes.ExpressMapper.Tests.Model;

public interface IBaseB
{
    byte B1 { get; set; }
    byte B2 { get; set; }
    DateTime DateTime { get; set; }
    DateTime DateTimex { get; set; }
    AnotherEnum EnumA { get; set; }
    AnotherEnum EnumX { get; set; }
    string BaseB { get; set; }
    string BaseB_2 { get; set; }
    int[] IntArr1 { get; set; }
    int[] IntArr2 { get; set; }
    bool YesOrNo { get; set; }
}
