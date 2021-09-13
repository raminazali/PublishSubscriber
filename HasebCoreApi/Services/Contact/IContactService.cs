using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HasebCoreApi.Models.Contactus;

namespace HasebCoreApi.Services.ContactUs
{
    public interface IContactService
    {
        Task<Contactus> CreateContact(Contactus contactus);
        Task<Contactus> AnswerContact(string id, ContactusAnswer answer);
        Task Delete(string id);
    }
}
