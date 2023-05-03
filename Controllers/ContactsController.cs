using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContactsController : Controller
    {

        public ContactsController(ContactsAPIDbContext mydbCotext) 
        {
            MydbCotext = mydbCotext;
        }

        public readonly ContactsAPIDbContext MydbCotext;

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await MydbCotext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetSingleContact([FromRoute] Guid id)
        {
            var singlecontact=await MydbCotext.Contacts.FindAsync(id);
            if(singlecontact == null)
            {
                return NotFound();
            }
             return Ok(singlecontact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContacts(AddContactRequest addContactRequest)
        {
            var contact = new Contacts()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                FullName = addContactRequest.FullName,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
            };
            await MydbCotext.Contacts.AddAsync(contact);
            await MydbCotext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact =await  MydbCotext.Contacts.FindAsync(id);
            if(contact!=null)
            {
                contact.Email= updateContactRequest.Email;  
                contact.Phone= updateContactRequest.Phone;
                contact.FullName= updateContactRequest.FullName;
                contact.Address= updateContactRequest.Address;

                await MydbCotext.SaveChangesAsync();

                return Ok(contact );
            }
            return NotFound();
            
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteSingleContact([FromRoute] Guid id)
        {
            var singlecontact = await MydbCotext.Contacts.FindAsync(id);
            if (singlecontact == null)
            {
                return NotFound();
            }
            MydbCotext.Remove(singlecontact);
            await MydbCotext.SaveChangesAsync();
            var response = new
            {






                status="200",
                message = "Contact deleted successfully",
                data = singlecontact
            };
            return Ok(response);
        
        }

    }
}
