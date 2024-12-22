using PassesProject.Data.StaticData.Enums;

namespace PassesProject.Data.StaticData.Dtos;

public class PassDto
{
    public PassType Result { get; set; }
    public string Receiver { get; set; }
    public decimal Distance { get; set; }
}