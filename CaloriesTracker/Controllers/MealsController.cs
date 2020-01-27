using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaloriesTracker.Data;
using CaloriesTracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CaloriesTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly DataContext _context;

        public MealsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Meals
        [HttpGet]
        public async Task<IActionResult> GetMeals()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = int.Parse(claim.Value);
            var meals = _context.Meals.Where(m => m.UserId == userId);
            return Ok(meals);
        }

        // GET: api/Meals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeal(int id)
        {
            var meal = await _context.Meals.FindAsync(id);

            if (meal == null)
            {
                return NotFound();
            }

            return Ok(meal);
        }

        // PUT: api/Meals/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeal(int id, Meal meal)
        {
            if (id != meal.Id)
            {
                return BadRequest();
            }

            _context.Entry(meal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(meal);
        }

        // POST: api/Meals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostMeal(Meal meal)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = int.Parse(claim.Value);
            meal.UserId = userId;
            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeal", new { id = meal.Id }, meal);
        }

        // DELETE: api/Meals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
            {
                return NotFound();
            }

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

            return Ok(meal);
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.Id == id);
        }
    }
}
