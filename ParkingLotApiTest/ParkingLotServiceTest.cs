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

    [Fact]
    public async Task Should_delete_parkinglot_sucessfully()
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
        var id = await parkingLotService.AddNewParkingLot(parkingLotDTO);

        // when
        await parkingLotService.DeleteParkingLot(id);

        // then
        Assert.Equal(0, context.ParkingLots.Count());
    }

    [Fact]
    public async Task Should_get_parking_lot_list_by_page_with_15_in_each_page()
    {
        // given
        var context = GetParkingLotContext();
        ParkingLotService parkingLotService = new ParkingLotService(context);
        for (var i = 0; i < 18; i++)
        {
            ParkingLotDTO parkingLotDTO = new ParkingLotDTO()
            {
                Name = "hi" + i,
                Capacity = 1,
                Location = "hihi",
            };
            await parkingLotService.AddNewParkingLot(parkingLotDTO);

        }

        // when
        var parkingLotInPage = await parkingLotService.GetAllParkingLotsByPage(2);

        // then
        Assert.Equal(3, parkingLotInPage.Count());


    }

    private ParkingLotContext GetParkingLotContext()
    {
        var scope = Factory.Services.CreateScope();
        var scopedService = scope.ServiceProvider;
        ParkingLotContext context = scopedService.GetRequiredService<ParkingLotContext>();
        return context;
    }
}