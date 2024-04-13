using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExerciseBUS 
{
    private readonly ExerciseDAO _exerciseDAO;

    public ExerciseBUS(ExerciseDAO exerciseDAO)
    {
        _exerciseDAO = exerciseDAO ?? throw new ArgumentNullException(nameof(exerciseDAO));
    }

    public async Task<ExerciseDTO> GetExerciseByIdAsync(int exerciseId)
    {
        return await _exerciseDAO.GetExerciseByIdAsync(exerciseId);
    }
}
