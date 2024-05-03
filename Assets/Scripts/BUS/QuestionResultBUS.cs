
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionResultBUS
{
    private readonly QuestionResultDAO _questionResulDao = new(); 
    
    public async Task<BaseDTO<List<QuestionResultDTO>>> GetQuestionResultByTestResultId(int id)
    {
        return await _questionResulDao.GetQuestionResultByTestResultId(id);
    }
    public async Task<BaseDTO<QuestionResultDTO>> AddQuestionResult(QuestionResultDTO questionResult)
    {
        return await _questionResulDao.AddQuestionResult(questionResult);
    }
    public async Task<BaseDTO<QuestionResultDTO>> GetById(int id)
    {
        return await _questionResulDao.GetById(id);
    }
}