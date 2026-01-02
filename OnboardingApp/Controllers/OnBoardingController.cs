using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Data;
using OnboardingApp.Models;
using OnboardingApp.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnboardingApp.Controllers
{
    public class OnBoardingController : Controller
    {
        private readonly IOnBoardingRepository _onboardRepo;
        private readonly ICandidateRepository _candidateRepo;
        private readonly AppDbContext _db;

        public OnBoardingController(IOnBoardingRepository onboardRepo, ICandidateRepository candidateRepo, AppDbContext db)
        {  
            _onboardRepo = onboardRepo;
            _candidateRepo = candidateRepo;
            _db = db;
        }

        public async Task<IActionResult> OnBoarding(int? candidateId)
        {
            if (candidateId.HasValue)
            {
                var candidate = await _candidateRepo.GetByIdAsync(candidateId.Value);
                 ViewData["Candidate"] = candidate;
            }

            var onboardingItems = await _onboardRepo.GetAllAsync();
            ViewData["OnBoardingItems"] = onboardingItems;

            return View();
        }

        [HttpGet]
        public ActionResult ConfirmAcknowledgement()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAcknowledgement(Candidate candidate)
        {

            var cand = await _candidateRepo.GetByIdAsync(candidate.Id);
            if (cand == null) return NotFound();

            cand.AcknowledgmentConfirmed = candidate.AcknowledgmentConfirmed;

            await _candidateRepo.UpdateAsync(cand);


            return View();
        }
    }
}

