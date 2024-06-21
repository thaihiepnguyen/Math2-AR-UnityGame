using System;

[Serializable]
public class QuestionAndAnswers
{
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;
}


[Serializable]
public class QuestionAndAnswersList
{
    public QuestionAndAnswers[] questions;
}
