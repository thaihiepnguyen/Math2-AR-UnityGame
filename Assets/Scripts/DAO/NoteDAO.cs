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
        return await API.Get<List<NoteDTO>>($"{GlobalVariable.server_url}/notes");
    }
}
