﻿public class RegistrationResponse
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}