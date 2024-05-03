
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class QuestionResultDAO
{
    public async Task<BaseDTO<List<QuestionResultDTO>>> GetQuestionResultByTestResultId(int testResultId)
    {
        return await API.Get<List<QuestionResultDTO>>($"{GlobalVariable.server_url}/question_result/testResult/{testResultId}");
    }
    public async Task<BaseDTO<QuestionResultDTO>>AddQuestionResult(QuestionResultDTO questionResult)
    {
        return await API.Post<QuestionResultDTO, QuestionResultDTO>($"{GlobalVariable.server_url}/question_result", questionResult);
    }
    public async Task<BaseDTO<QuestionResultDTO>> GetById(int id)
    {
        return await API.Get<QuestionResultDTO>($"{GlobalVariable.server_url}/question_result/{id}");
    }
}