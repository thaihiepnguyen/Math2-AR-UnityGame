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
        catch(Exception ex) {
            throw new Exception("Error: ", ex);
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
            throw new Exception("Error: ", ex);
        }
    }
}
