using AutoMapper;
using Graph.CrossCutting;
using Graph.Infrastructure.Database.Command.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Domain.Service.Mappings
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<Status, TaskStatusEnum>()
                .AfterMap((model, status) =>
                {
                    status = (TaskStatusEnum)model.Id;
                });
        }
    }
}
