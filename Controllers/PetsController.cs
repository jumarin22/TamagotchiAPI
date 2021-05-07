using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TamagotchiAPI.Models;

namespace TamagotchiAPI.Controllers
{
    // All of these routes will be at the base URL:     /api/Pets
    // That is what "api/[controller]" means below. It uses the name of the controller
    // in this case PetsController to determine the URL
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        // This is the variable you use to have access to your database
        private readonly DatabaseContext _context;

        // Constructor that receives a reference to your database context
        // and stores it in _context for you to use in your API methods
        public PetsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Pets
        //
        // Returns a list of all your Pets
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets(string input)
        {
            // Adventure mode: Only return alive pets with correct query string.
            if (input == "alive")
            {
                var alivePets = _context.Pets.AsEnumerable().Where(pet => !pet.IsDead);
                return alivePets.ToList();
            }
            else
                return await _context.Pets.OrderBy(row => row.Id).ToListAsync();
        }

        // GET: api/Pets/5
        //
        // Fetches and returns a specific pet by finding it by id. The id is specified in the
        // URL. In the sample URL above it is the `5`.  The "{id}" in the [HttpGet("{id}")] is what tells dotnet
        // to grab the id from the URL. It is then made available to us as the `id` argument to the method.
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            // Find the pet in the database using `FindAsync` to look it up by id
            var pet = await _context.Pets.FindAsync(id);

            // If we didn't find anything, we receive a `null` in return
            if (pet == null)
            {
                // Return a `404` response to the client indicating we could not find a pet with this id
                return NotFound();
            }

            // Return the pet as a JSON object.
            return pet;
        }


        // POST: api/Pets
        //
        // Creates a new pet in the database.
        //
        // The `body` of the request is parsed and then made available to us as a Pet
        // variable named pet. The controller matches the keys of the JSON object the client
        // supplies to the names of the attributes of our Pet POCO class. This represents the
        // new values for the record.
        //
        [HttpPost]
        public async Task<ActionResult<Pet>> PostPet(Pet pet)
        {
            // Birthday defaults to current DateTime. 
            pet.Birthday = DateTime.Now;
            // HungerLevel defaults to 0. 
            pet.HungerLevel = 0;
            // HappinessLevel defaults to 0. 
            pet.HappinessLevel = 0;
            // Indicate to the database context we want to add this new record
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            // Return a response that indicates the object was created (status code `201`) and some additional
            // headers with details of the newly created object.
            return CreatedAtAction("GetPet", new { id = pet.Id }, pet);
        }

        // DELETE: api/Pets/5
        //
        // Deletes an individual pet with the requested id. The id is specified in the URL
        // In the sample URL above it is the `5`. The "{id} in the [HttpDelete("{id}")] is what tells dotnet
        // to grab the id from the URL. It is then made available to us as the `id` argument to the method.
        //
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            // Find this pet by looking for the specific id
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                // There wasn't a pet with that id so return a `404` not found
                return NotFound();
            }

            // Tell the database we want to remove this record
            _context.Pets.Remove(pet);

            // Tell the database to perform the deletion
            await _context.SaveChangesAsync();

            // Return a copy of the deleted data
            return Ok(pet);
        }

        // Adding Playtimes to a Pet
        // POST /api/Pets/1/Playtimes
        [HttpPost("{id}/Playtimes")]
        public async Task<ActionResult<Playtime>> CreatePlaytimeForPet(int id, Playtime playtime)
        {
            // First, lets find the Pet (by using the ID)
            var pet = await _context.Pets.FindAsync(id);
            // If the pet doesn't exist: return a 404 Not found.
            if (pet == null)
                return NotFound();
            // Associate the playtime to the given pet.
            playtime.PetId = pet.Id;
            // Set playtime to current time. 
            playtime.When = DateTime.Now;
            // Add the playtime to the database
            _context.Playtimes.Add(playtime);
            // Add five to pet happiness level.
            pet.HappinessLevel += 5;
            // Add three to pet hunger level. 
            pet.HungerLevel += 3;


            // Adventure mode: Update LastInteractedWithDate
            pet.LastInteractedWithDate = DateTime.Now;

            await _context.SaveChangesAsync();
            // Return the new playtime to the response of the API. 
            return Ok(playtime);
        }

        [HttpPost("{id}/Feedings")]
        public async Task<ActionResult<Feeding>> CreateFeedingForPet(int id, Feeding feeding)
        {

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
                return NotFound();
            feeding.PetId = pet.Id;
            // Can't feed a full pet. 
            if (pet.HungerLevel == 0)
                return BadRequest(new { Message = $"{pet.Name} isn't hungry!" });
            // Set feeding to current time. 
            feeding.When = DateTime.Now;
            _context.Feedings.Add(feeding);
            // Add three to pet happiness level.
            pet.HappinessLevel += 3;
            // Subtract five from pet hunger level. 
            pet.HungerLevel -= 5;


            // Adventure mode: Update LastInteractedWithDate
            pet.LastInteractedWithDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(feeding);
        }

        [HttpPost("{id}/Scoldings")]
        public async Task<ActionResult<Scolding>> CreateScoldingForPet(int id, Scolding scolding)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
                return NotFound();
            scolding.PetId = pet.Id;
            scolding.When = DateTime.Now;
            _context.Scoldings.Add(scolding);
            // Subtract five from pet happiness level. 
            pet.HappinessLevel -= 5;


            // Adventure mode: Update LastInteractedWithDate
            pet.LastInteractedWithDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(scolding);
        }

        // Private helper method that looks up an existing pet by the supplied id
        private bool PetExists(int id)
        {
            return _context.Pets.Any(pet => pet.Id == id);
        }
    }
}
