using AutoMapper;
using Graph.Application.Commands;
using Command = Graph.Infrastructure.Database.Command.Model;
using Query = Graph.Infrastructure.Database.Query.UserSchema;
using Graph.Infrastructure.Database.Command.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Graph.Infrastructure.ServiceBus;
using Graph.CrossCutting.Exceptions;
using Graph.CrossCutting.Extensions;
using Graph.Application.Commands.User;

namespace Graph.Domain.Service.CommandHandler
{
    public class UserCommandHandler : IRequestHandler<AddUserCommand, bool>,
                                      IRequestHandler<UpdateUserInfoCommand, bool>
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IServiceBus _Bus;
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _Mapper;

        public UserCommandHandler(IUnitOfWork unitOfWork, IServiceBus bus, IUserRepository userRepository, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _Bus = bus;

            _UserRepository = userRepository;
            _Mapper = mapper;
        }

        public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userDomain = new User(request.Name, request.Email);
            userDomain.Validate();

            await _UserRepository.Add(userDomain.ToModel<Command.User>(_Mapper));
            await _UnitOfWork.Commit();

            var publishMessage = new Message();
            publishMessage.MessageType = "AddUser";
            publishMessage.SetData(_Mapper.Map<Query.User>(userDomain));

            await _Bus.SendMessage(publishMessage);

            return true;
        }

        public async Task<bool> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var userDomain =  _UserRepository.GetById(request.Id).Result.ToDomain<User>(_Mapper);

            userDomain.SetPersonalInfo(request.Name, request.Email);

            await _UserRepository.Update(userDomain.ToModel<Command.User>(_Mapper));
            await _UnitOfWork.Commit();

            var publishMessage = new Message();
            publishMessage.MessageType = "UpdateUser";
            publishMessage.SetData(userDomain.ToQueryModel<Query.User>(_Mapper));

            await _Bus.SendMessage(publishMessage);

            return true;
        }
    }
}
