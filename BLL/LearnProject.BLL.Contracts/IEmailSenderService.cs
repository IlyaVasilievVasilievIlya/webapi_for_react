﻿using LearnProject.BLL.Contracts.Models;
using LearnProject.BLL.Contracts.Models.EmailMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.BLL.Contracts
{
    public interface IEmailSenderService
    {
        Task<ServiceResponse<int>> SendEmailAsync(string to, string subject, string body);
    }
}