using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApiTest;

[Collection("IntegrationTest")]
public class ParkingLotServiceTest: TestBase
{
    public ParkingLotServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_buy_parking_lot_when_buy_a_new_parking_lot_when_capacity_larger_equal_to_zeroAsync()
    {
        // given
        var context = GetParkingLotContext();
        ParkingLotDTO parkingLotDTO = new ParkingLotDTO()
        {
            Name = "hi",
            Capacity = 1,
            Location = "hihi",
        };

        ParkingLotService parkingLotService = new ParkingLotService(context);

        // when
        await parkingLotService.AddNewParkingLot(parkingLotDTO);

        // then
        Assert.Equal(1, context.ParkingLots.Count());
    }

    private ParkingLotContext GetParkingLotContext()
    {
        var scope = Factory.Services.CreateScope();
        var scopedService = scope.ServiceProvider;
        ParkingLotContext context = scopedService.GetRequiredService<ParkingLotContext>();
        return context;
    }
}