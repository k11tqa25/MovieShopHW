using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        public PurchaseService(IPurchaseRepository purchaseRepository, IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
        }

        public async Task<PurchaseResponseModel> MakePurchaseAsync(int userId, int movieId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var movie = await _movieRepository.GetByIdAsync(movieId);
            var purchase = new Purchase
            {
                MovieId = movieId,
                UserId = userId,
                PurchaseDateTime = DateTime.Now,
                TotalPrice = movie.Price.GetValueOrDefault()
            };

            var newPurchase = await _purchaseRepository.AddAsync(purchase);
            return new PurchaseResponseModel
            {
                Id = newPurchase.Id,
                MovieId = newPurchase.MovieId,
                UserId = newPurchase.UserId,
                MovieTitle = movie.Title,
                UserName = user.FirstName + " " + user.LastName,
                Price = movie.Price.GetValueOrDefault(),                
                PurchaseDate = newPurchase.PurchaseDateTime
            };
        }
    }
}
