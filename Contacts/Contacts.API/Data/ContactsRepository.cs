using System;
using System.Linq;
using System.Collections.Generic;
using SampleOidc.Models;
using Microsoft.EntityFrameworkCore;

namespace SampleOidc.Data
{
    public class ContactsRepository : IDisposable
    {
        private readonly ContactsDbContext _contactsDbContext;
        public ContactsRepository(ContactsDbContext contactsDbContext)
        {
            _contactsDbContext = contactsDbContext;
        }

        public IQueryable<Contact> Query()
        {
            return _contactsDbContext.Contacts
            .Include(x => x.Addresses)
            .Include(x => x.Emails)
            .Include(x => x.Phones)
            .AsNoTracking();
        }

        public Contact Add(Contact contact)
        {
            SetDefaultForAdd(contact);

            _contactsDbContext.Contacts.Add(contact);

            _contactsDbContext.SaveChanges();

            return _contactsDbContext.Contacts.AsNoTracking().First(x => x.ContactId == contact.ContactId);
        }

        private void SetDefaultForAdd(Contact contact)
        {
            contact.ContactId = Guid.NewGuid().ToString("n");
            contact.AddedAt = DateTimeOffset.UtcNow;
            contact.UpdatedAt = DateTimeOffset.UtcNow;

            SetDefaultForAddAddresses(contact.Addresses);
            SetDefaultForAddEmails(contact.Emails);
            SetDefaultForAddPhones(contact.Phones);
        }

        private void SetDefaultForAddEmails(IEnumerable<Email> emails)
        {
            if (emails != null)
            {
                foreach (var emailItem in emails)
                {
                    emailItem.EmailId = Guid.NewGuid().ToString("n");
                    emailItem.AddedAt = DateTimeOffset.UtcNow;
                    emailItem.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
        }

        private void SetDefaultForAddAddresses(IEnumerable<Address> addresses)
        {
            if (addresses != null)
            {
                foreach (var addressItem in addresses)
                {
                    addressItem.AddressId = Guid.NewGuid().ToString("n");
                    addressItem.AddedAt = DateTimeOffset.UtcNow;
                    addressItem.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
        }

        private void SetDefaultForAddPhones(IEnumerable<Phone> phones)
        {
            if (phones != null)
            {
                foreach (var phoneItem in phones)
                {
                    phoneItem.PhoneId = Guid.NewGuid().ToString("n");
                    phoneItem.AddedAt = DateTimeOffset.UtcNow;
                    phoneItem.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
        }

        public void Dispose()
        {
            _contactsDbContext.Dispose();
        }
    }
}