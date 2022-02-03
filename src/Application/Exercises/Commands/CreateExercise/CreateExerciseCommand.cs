using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Exercises.Commands.CreateExercise
{
    public class CreateExerciseCommand : IMap, IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Database { get; set; }
        public string ValidAnswer { get; set; }
        public bool IsPrivate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateExerciseCommand, Exercise>()
                .ForMember(x => x.Database, opt => opt.Ignore());
        }
    }

    public class CreateTodoListCommandHandler : IRequestHandler<CreateExerciseCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseService _databaseService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IUserContextService _userContextService;

        public CreateTodoListCommandHandler(IMapper mapper, IDatabaseService databaseService
            , IExerciseRepository exerciseRepository, IDatabaseRepository databaseRepository,
            IUserContextService userContextService)
        {
            _mapper = mapper;
            _databaseService = databaseService;
            _exerciseRepository = exerciseRepository;
            _databaseRepository = databaseRepository;
            _userContextService = userContextService;
        }

        public async Task<int> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            await _databaseService.SendQueryNoData(request.ValidAnswer, request.Database);
            var exercise = _mapper.Map<Exercise>(request);
            exercise.CreatorId = (int)_userContextService.GetUserId;
            exercise.DatabaseId = await _databaseRepository.GetDatabaseIdByName(request.Database);
            exercise.IsPrivate = request.IsPrivate;
            await _exerciseRepository.AddAsync(exercise);
            return exercise.Id;
        }
    }
    
}
