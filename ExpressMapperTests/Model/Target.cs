using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InspiredCodes.ExpressMapper.Tests.Model;

public class Target : Source
{

    public string ThisAsSourceToJson() => JsonSerializer.Serialize((Source)this);

    public string ThisIsTargetOnly { get; set; }

}
