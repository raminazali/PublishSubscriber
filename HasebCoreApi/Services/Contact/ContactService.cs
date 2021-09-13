using HasebCoreApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HasebCoreApi.Models.Contactus;

namespace HasebCoreApi.Services.ContactUs
{
    public class ContactService : IContactService
    {
        private readonly IMongoRepository<Contactus> _contactUs;
        public ContactService(IMongoRepository<Contactus> contactUs)
        {
            _contactUs = contactUs;
        }

        public async Task<Contactus> AnswerContact(string id, ContactusAnswer answer)
        {
            var contact = await _contactUs.FindByIdAsync(id);

            if (contact == null)
            {
                throw new ContactNotFoundException();
            }

            answer.Id = ObjectId.GenerateNewId();
            contact.Answer.Add(answer);

            contact.IsAnswered = contact.Answer.Count > 0;

            await _contactUs.ReplaceOneAsync(contact);
            return contact;
        }

        public async Task<Contactus> CreateContact(Contactus contactus)
        {
            contactus.IsAnswered = false;
            await _contactUs.InsertOneAsync(contactus);
            return contactus;
        }

        public async Task Delete(string id)
        {
            await _contactUs.DeleteByIdAsync(id);
        }
    }

    public class ContactNotFoundException : Exception { };
}
