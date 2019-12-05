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
    public class ConsoleLoggerDecorator : ITicketService
    {
        private ILogger _logger;
        private ITicketService _innerService;
        protected Train train;
        public ConsoleLoggerDecorator(ITicketService innerService, ILogger logger)
        {
            _innerService = innerService;
            _logger = logger;
        }

        public Ticket CreateTicket(int reservationId, string fName, string lName)
        {
            Ticket ticketToReturn = _innerService.CreateTicket(reservationId, fName, lName);
            TicketConsoleLog(ticketToReturn);

            return ticketToReturn;
        }

        public void SellTicket(Ticket ticket)
        {
            _innerService.SellTicket(ticket);
        }
        public void TicketConsoleLog(Ticket ticket)
        {
            string ticketMSG = "\nNew ticket has been bought:";
            ticketMSG += "\nTicket Id: " + ticket.Id;
            ticketMSG += "\nReservation Id: " + ticket.ReservationId;
            ticketMSG += "\n" + ticket.FirstName + " " + ticket.LastName;

            _logger.Log(ticketMSG, LogSeverity.Info);
        }

    }
}
