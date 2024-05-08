using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;


public class ExerciseDAO
{
    // Start is called before the first frame update

    public async Task<BaseDTO<List<ExerciseDTO>>> GetExerciseByType(ExerciseTypeDTO exerciseTypeDTO)
    {
        return await API.Post<ExerciseTypeDTO, List<ExerciseDTO>>($"{GlobalVariable.server_url}/exercises/get-exercise-by-type", exerciseTypeDTO);
    }
    public async Task<BaseDTO<List<ExerciseDTO>>> GetExerciseByTestId(int test_id)
    {
        return await API.Get<List<ExerciseDTO>>($"{GlobalVariable.server_url}/exercises/test/{test_id}");
    }
    public async Task<ExerciseDTO> GetExerciseByIdAsync(int exerciseId)
    {
        try
        {
            var a = await API.getMethod($"/exercises/{exerciseId}");
            var exerciseResponse = JsonConvert.DeserializeObject<ExerciseResponseDTO>(a);

            if (exerciseResponse != null)
            {
                return exerciseResponse.data[0];
            }
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
    public async Task<List<ExerciseDTO>> GetAllExercises()
    {
        try
        {
            var result = await API.getMethod($"/exercises/");
            var exerciseResponse = JsonConvert.DeserializeObject<BaseDTO<List<ExerciseDTO>>>(result);

            if (exerciseResponse.data != null)
            {
                return exerciseResponse.data;
            }
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
    public async Task<List<ExerciseDTO>> GetExercisesByLessonId(string lessonId)
    {
        try
        {
            var result = await API.getMethod($"/exercises/lesson/{lessonId}");
            var exerciseResponse = JsonConvert.DeserializeObject<BaseDTO<List<ExerciseDTO>>>(result);

            if (exerciseResponse.data != null)
            {
                return exerciseResponse.data;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error: ", ex);
        }
    }
    public async Task<List<ExerciseDTO>> GetExercisesByChapterId(char chaperId)
    {
        try
        {
            var result = await API.getMethod($"/exercises/chapter/{chaperId}");
            var exerciseResponse = JsonConvert.DeserializeObject<BaseDTO<List<ExerciseDTO>>>(result);

            if (exerciseResponse.data != null)
            {
                return exerciseResponse.data;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error: ", ex);

        }
    }
}
