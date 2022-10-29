using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public readonly CarnetsDbContext _context;

        public SubscriptionRepository(CarnetsDbContext context)
        {
            _context = context;
        }

        public Task<Subscription> GetSubscriptionById(string subcriptionId, bool asTracking)
        {
            IQueryable<Subscription> query = _context.Subscriptions
                .Include(s => s.Gympass);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefaultAsync(s => s.SubscriptionId == subcriptionId);
        } 

        public async Task<IEnumerable<Subscription>> GetAllGympassSubscriptions(IEnumerable<string> gympassIds, bool asTracking)
        {
            if (gympassIds == null) throw new ArgumentNullException(nameof(gympassIds));

            var query = _context.Subscriptions
                .Include(s => s.Gympass)
                .Where(s => gympassIds.Contains(s.Gympass.GympassId));

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetAllCustomerSubscriptions(string stripeCustomerId, bool asTracking)
        {
            IQueryable<Subscription> query = _context.Subscriptions
                .Where(s => s.StripeCustomerId == stripeCustomerId)
                .Include(s => s.Gympass);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Result<Subscription>> CreateSubscription(Subscription subscription)
        {
            if (subscription.Gympass is null)
            {
                return new Result<Subscription>("Cannot create subscription without gympass");
            }

            subscription.SubscriptionId = Guid.NewGuid().ToString();

            await _context.Subscriptions.AddAsync(subscription);

            return new Result<Subscription>(subscription);
        }

        public async Task<Result<Subscription>> UpdateSubscription(string subscriptionId, Subscription subscription)
        {
            var subscriptionFromDb = await GetSubscriptionById(subscriptionId, true);

            if (subscriptionFromDb is null)
            {
                return new Result<Subscription>(Common.CommonConsts.NOT_FOUND);
            }

            subscriptionFromDb.LastPaymentDate = subscription.LastPaymentDate;
            subscriptionFromDb.StripePaymentmethodId = subscription.StripePaymentmethodId;
            subscriptionFromDb.StripeCustomerId = subscription.StripeCustomerId;

            return new Result<Subscription>(subscription);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
