using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AddNoteResponse
{
    public bool isAdded { get; set; }
}
public class AddNoteRequest
{
    public int lessonId { get; set; }
    public AddNoteRequest(int lessonId)
    {
        this.lessonId = lessonId;
    }
}

public class CheckNoteExistsResponse
{
    public bool exists { get; set; }
}
public class CheckNoteExistsRequest
{
    public int lessonId { get; set; }

    public CheckNoteExistsRequest(int lessonId)
    {
        this.lessonId = lessonId;
    }
}


public class NoteDAO
{
    public async Task<BaseDTO<List<NoteDTO>>> GetNotesByUserId()
    {
        return await API.Get<List<NoteDTO>>($"{GlobalVariable.server_url}/notes/get-all");
    }
    public async Task<BaseDTO<AddNoteResponse>> AddNewNoteWithUserId(int lessonId)
    {
        return await API.Post<AddNoteRequest, AddNoteResponse>($"{GlobalVariable.server_url}/notes/add-new", new AddNoteRequest(lessonId));
    }
    public async Task<BaseDTO<CheckNoteExistsResponse>> CheckNoteExistsWithUserId(int lessonId)
    {
        return await API.Post<CheckNoteExistsRequest, CheckNoteExistsResponse>($"{GlobalVariable.server_url}/notes/check-exists", new CheckNoteExistsRequest(lessonId));
    }
}
