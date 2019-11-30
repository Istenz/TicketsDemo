using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.DefaultImplementations
{
    public class TicketsServiceLoggingDecorator : ITicketService
    {
        private ITicketRepository _tickRepo;
        private IPriceCalculationStrategy _priceStr;
        private IReservationRepository _resRepo;
        private IRunRepository _runRepository;
        private ITrainRepository _trainRepo;
        private ILogger _logger;
        protected PlaceInRun place;
        protected Ticket newTicket;
        protected Train train;
        public TicketsServiceLoggingDecorator(ITicketRepository tickRepo, IReservationRepository resRepo, ITrainRepository trainRepo,
            IPriceCalculationStrategy priceCalculationStrategy, IRunRepository runRepository, ILogger logger)
        {
            _tickRepo = tickRepo;
            _resRepo = resRepo;
            _trainRepo = trainRepo;
            _priceStr = priceCalculationStrategy;
            _runRepository = runRepository;
            _logger = logger;
        }

        public Ticket CreateTicket(int reservationId, string fName, string lName)
        {
            var res = _resRepo.Get(reservationId);

            if (res.TicketId != null)
            {
                throw new InvalidOperationException("ticket has been already issued to this reservation, unable to create another one");
            }

            place = _runRepository.GetPlaceInRun(res.PlaceInRunId);
            train = _trainRepo.GetTrainDetails(place.Run.TrainId);
            newTicket = new Ticket()
            {
                ReservationId = res.Id,
                CreatedDate = DateTime.Now,
                FirstName = fName,
                LastName = lName,
                Status = TicketStatusEnum.Active,
                PriceComponents = new List<PriceComponent>()
            };

            newTicket.PriceComponents = _priceStr.CalculatePrice(place);
            _tickRepo.Create(newTicket);
            TicketConsoleLog();
            return newTicket;
        }

        public void SellTicket(Ticket ticket)
        {
            if (ticket.Status == TicketStatusEnum.Sold)
            {
                throw new ArgumentException("ticket is already sold");
            }

            ticket.Status = TicketStatusEnum.Sold;
            _tickRepo.Update(ticket);
        }
        public void TicketConsoleLog()
        {
            var ticketMSG = $"\n Person info: {newTicket.FirstName} {newTicket.LastName}\n Train number: {train.Number}\n Route: {train.StartLocation}-{train.EndLocation}\n Date: {place.Run.Date}\n Place number: {place.Number}\n Carriage number: {place.CarriageNumber}";
            _logger.Log(ticketMSG, LogSeverity.Info);
        }

    }
}
