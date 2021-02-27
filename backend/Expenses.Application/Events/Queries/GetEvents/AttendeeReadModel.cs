﻿using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;

namespace Expenses.Application.Events.Queries.GetEvents
{
    public class AttendeeReadModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}