using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class NoteDAO
{
    public async Task<BaseDTO<List<NoteDTO>>> GetNotesByUserId()
    {
        return await API.Get<List<NoteDTO>>($"{GlobalVariable.server_url}/notes/get-all");
    }
    public async Task<BaseDTO<bool>> AddNewNoteWithUserId(int noteId)
    {
        return await API.Post<int, bool>($"{GlobalVariable.server_url}/notes/add-new", noteId);
    }
}
