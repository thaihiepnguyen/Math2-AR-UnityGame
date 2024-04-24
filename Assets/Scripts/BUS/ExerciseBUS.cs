using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExerciseBUS 
{
    private readonly ExerciseDAO _exerciseDAO= new();

    public ExerciseBUS()
    {

    }

    public async Task<ExerciseDTO> GetExerciseByIdAsync(int exerciseId)
    {
        return await _exerciseDAO.GetExerciseByIdAsync(exerciseId);
    }
    public async Task<List<ExerciseDTO>> GetAllExercises()
    {
        return await _exerciseDAO.GetAllExercises();
    }

    public async Task<BaseDTO<List<ExerciseDTO>>> GetExerciseByType(ExerciseTypeDTO exerciseTypeDTO){

        return await _exerciseDAO.GetExerciseByType(exerciseTypeDTO);

    }
}
