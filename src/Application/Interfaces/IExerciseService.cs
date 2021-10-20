﻿using Application.Dto.CreateExerciseDto;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExerciseService
    {
        Task<int> CreateExerciseAsync(CreateExerciseDto model);
        Task<IEnumerable<GetExerciseVm>> GetAllExercisesAsync();
    }
}
