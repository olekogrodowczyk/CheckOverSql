using Application.Dto.CreateExerciseDto;
using Application.Dto.GetExerciseDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateExerciseDto model)
        {
            var result = await _exerciseService.CreateExerciseAsync(model);
            return Ok(new Result<int>(result, "Pomyślnie dodano nowe zadanie"));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _exerciseService.GetAllExercisesAsync();
            return Ok(new Result<IEnumerable<GetExerciseDto>>(result,"Pomyślnie zwrócono wszystkie zadania"));
        }
    }
}
