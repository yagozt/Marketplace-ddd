using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        public ClassifiedAdId Id { get; private set; }
        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();
        }
        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();
        }
        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
        }
        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();
        }
        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
        }
        protected override void EnsureValidState()
        {
            var valid = Id != null &&
            OwnerId != null &&
            (State switch
            {
                ClassifiedAdState.PendingReview => Title != null
                && Text != null && Price?.Amount > 0,
                ClassifiedAdState.Active =>
                Title != null
                && Text != null
                && Price?.Amount > 0
                && ApprovedBy != null,
                _ => true
            });
            if (!valid)
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = ClassifiedAdText.FromString(e.AdText);
                    break;
                case Events.ClassifiedAdTitleCreated e:
                    Title = ClassifiedAdTitle.FromString(e.Title);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
            }
        }

        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }
        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }
    }
}
