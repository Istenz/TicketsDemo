using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.WeekendsCalculation
{
    public class WeekendPriceCalculationStrategy : IPriceCalculationStrategy
    {
        private IRunRepository _runRepository;
        private ITrainRepository _trainRepository;
        private IHolidayRepository _holidayRepository;
        

   
        public WeekendPriceCalculationStrategy(IRunRepository runRepository, ITrainRepository trainRepository, IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
            _runRepository = runRepository;
            _trainRepository = trainRepository;
        }

        public List<PriceComponent> CalculatePrice(PlaceInRun placeInRun)
        {
            
            var components = new List<PriceComponent>();
            var holidayList = _holidayRepository.GetAllHolidays() ;
            var run = _runRepository.GetRunDetails(placeInRun.RunId);
            var train = _trainRepository.GetTrainDetails(run.TrainId);
            var place =
                train.Carriages
                    .Select(car => car.Places.SingleOrDefault(pl =>
                        pl.Number == placeInRun.Number &&
                        car.Number == placeInRun.CarriageNumber))
                    .SingleOrDefault(x => x != null);
            place.Carriage = train.Carriages.FirstOrDefault(car=>car.Id == place.CarriageId);
            var placeComponent = new PriceComponent() { Name = "Main price" };
            placeComponent.Value = place.Carriage.DefaultPrice * place.PriceMultiplier;
            components.Add(placeComponent);

            for(int i = 1; i < holidayList.Count; i++)
            {
                if (run.Date.Day == holidayList[i].Day && run.Date.Month == holidayList[i].Month)
                {
                    var cashDeskComponent = new PriceComponent()
                    {
                        Name = "Holiday tax",
                        Value = placeComponent.Value * 1.25m
                    };
                    components.Add(cashDeskComponent);
                    break;
                }
            }
            if (placeComponent.Value > 30)
            {
                var cashDeskComponent = new PriceComponent()
                {
                    Name = "Cash desk service tax",
                    Value = placeComponent.Value * 0.2m
                };
                components.Add(cashDeskComponent);
            }

            return components;
        }
    }
}

