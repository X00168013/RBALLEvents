using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBALLEvents.Models.RacquetballViewModels
{
    public class UserViewModel
    {
        public Gender Gender { get; set; }
        public IEnumerable<Event> Members { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<EventCoordinator> EventCoordinators { get; set; }
    }
}
