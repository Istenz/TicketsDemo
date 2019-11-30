using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;

namespace TicketsDemo.EF.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        public List<Holiday> GetAllHolidays()
        {
            using (var ctx = new TicketsContext())
            {
                return ctx.Holidays.ToList();
            }
        }
    }
}
