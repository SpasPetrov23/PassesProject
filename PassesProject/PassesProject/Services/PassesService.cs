using PassesProject.Data.StaticData;
using PassesProject.Data.StaticData.Dtos;
using PassesProject.Data.StaticData.Enums;
using PassesProject.Services.Dtos;
using PassesProject.Utils;

namespace PassesProject.Services;

public class PassesService
{
    private readonly PassesStaticData _staticData;

    public PassesService(PassesStaticData staticData)
    {
        _staticData = staticData;
    }

    public MostSuccessfulReceiverDto CalculateMostCompletePassPercentage()
    {
        List<PassDto> passesData = _staticData.GetPassesData();

        List<IGrouping<string, PassDto>> passesGroupedByReceiver = passesData
            .GroupBy(k => k.Receiver).ToList();

        Dictionary<string, decimal> completePercentagePerPlayer = new();

        foreach (var passByReceiverGroup in passesGroupedByReceiver)
        {
            int totalPasses = passByReceiverGroup.Count();
            int totalSuccessfulPasses = passByReceiverGroup.Count(x => x.Result == PassType.Complete);

            decimal percentageSuccessfulPasses = Math.Round((decimal)totalSuccessfulPasses / (decimal)totalPasses * 100, 0, MidpointRounding.AwayFromZero);
            
            completePercentagePerPlayer.Add(passByReceiverGroup.Key, percentageSuccessfulPasses);
        }

        KeyValuePair<string, decimal> mostCompletePercentage = completePercentagePerPlayer.MaxBy(x => x.Value);

        MostSuccessfulReceiverDto mostSuccessfulReceiver = new()
        {
            Name = mostCompletePercentage.Key,
            SuccessfulPassesPercentage = $"{mostCompletePercentage.Value}%"
        };

        return mostSuccessfulReceiver;

    }
    
    public LongestDistancePassDto CalculateLongestPass()
    {
        List<PassDto> passesData = _staticData.GetPassesData();
        
        PassDto? longestDistancePass = passesData.MaxBy(x => x.Distance);

        if (longestDistancePass == null)
        {
            throw new AppException(ErrorMessages.EMPTY_PASSES_DATA);
        }

        LongestDistancePassDto result = new()
        {
            Name = longestDistancePass.Receiver,
            Distance = longestDistancePass.Distance
        };

        return result;
    }
}