using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class NoteBUS
{
    private readonly NoteDAO _noteDAO = new NoteDAO();

    public async Task<BaseDTO<List<NoteDTO>>> GetNotesByUserId()
    {
        return await _noteDAO.GetNotesByUserId();
    }
    public async Task<BaseDTO<AddNoteResponse>> AddNewNoteWithUserId(int lessonId)
    {
        return await _noteDAO.AddNewNoteWithUserId(lessonId);
    }
    public async Task<BaseDTO<CheckNoteExistsResponse>> CheckNoteExistsWithUserId(int lessonId)
    {
        return await _noteDAO.CheckNoteExistsWithUserId(lessonId);
    }
}
