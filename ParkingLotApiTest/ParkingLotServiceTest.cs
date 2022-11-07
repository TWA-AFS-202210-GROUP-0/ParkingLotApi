using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System;
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

    [Fact]
    public async Task Should_get_one_parking_lot_detail()
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
        var parkingLot = parkingLotService.GetParkingLot(id);

        // then
        Assert.Equal(parkingLotDTO.Name, parkingLot.Name);
        Assert.Equal(parkingLotDTO.Capacity, parkingLot.Capacity);
        Assert.Equal(parkingLotDTO.Location, parkingLot.Location);
    }

    [Fact]
    public async Task Should_update_one_parking_lot_capacity()
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
        var parkingLot = await parkingLotService.UpdateParkingLotCapacity(id, 11);

        // then
        Assert.Equal(11, parkingLot.Capacity);
    }

    [Fact]
    public async Task Should_create_order_when_park_a_car_sucessfully()
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
        var carPlate = "XXX111";

        // when
        OrderDTO order = await parkingLotService.AddCarInParkingLot(id, carPlate);

        // then
        Assert.Equal(carPlate, order.PlateNumber);
        Assert.Equal(parkingLotDTO.Name, order.ParkingLotName);
        Assert.Equal("open", order.Status);
        Assert.NotNull(order.OrderNumber);
        Assert.NotNull(order.CreationTime);
    }

    [Fact]
    public async Task Should_not_create_order_when_parkinglot_is_full()
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
        await parkingLotService.AddCarInParkingLot(id, "XXX111");

        var ex = await Assert.ThrowsAsync<Exception>(() => parkingLotService.AddCarInParkingLot(id, "XXX222"));
        Assert.Equal("The parking lot is full", ex.Message);
    }

    [Fact]
    public async Task Should_close_order_when_park_a_car_sucessfully()
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
        var carPlate = "XXX111";
        OrderDTO order = await parkingLotService.AddCarInParkingLot(id, carPlate);

        // when
        var closedOrder = await parkingLotService.CLoseParkingCarOrder(order);

        // then
        Assert.Equal(carPlate, closedOrder.PlateNumber);
        Assert.Equal(parkingLotDTO.Name, closedOrder.ParkingLotName);
        Assert.Equal("closed", closedOrder.Status);
        Assert.NotNull(closedOrder.OrderNumber);
        Assert.NotNull(closedOrder.CreationTime);
        Assert.NotNull(closedOrder.ClosedTime);
    }

    private ParkingLotContext GetParkingLotContext()
    {
        var scope = Factory.Services.CreateScope();
        var scopedService = scope.ServiceProvider;
        ParkingLotContext context = scopedService.GetRequiredService<ParkingLotContext>();
        return context;
    }
}