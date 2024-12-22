using PassesProject.Data.StaticData.Dtos;
using PassesProject.Data.StaticData.Enums;

namespace PassesProject.Data.StaticData;

public class PassesStaticData
{
    public List<PassDto> GetPassesData()
    {
        List<PassDto> passes = new()
        {
            new PassDto
            {
                Result = PassType.Incomplete,
                Receiver = "Demaryius Thomas",
                Distance = 0.7M
            },
            new PassDto
            {
                Result = PassType.Complete,
                Receiver = "Tim Patrick",
                Distance = 0.9M
            },
            new PassDto
            {
                Result = PassType.Complete,
                Receiver = "Demaryius Thomas",
                Distance = 0.3M
            },
            new PassDto
            {
                Result = PassType.Incomplete,
                Receiver = "Tim Patrick",
                Distance = 0.9M
            },
            new PassDto
            {
                Result = PassType.Incomplete,
                Receiver = "Tim Patrick",
                Distance = 0.8M
            },
            new PassDto
            {
                Result = PassType.Complete,
                Receiver = "Demaryius Thomas",
                Distance = 0.1M
            },
            new PassDto
            {
                Result = PassType.Interception,
                Receiver = "Demaryius Thomas",
                Distance = 0.4M
            }
        };

        return passes;
    }
}