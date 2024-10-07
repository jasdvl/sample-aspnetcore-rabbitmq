using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.BookOrder;

namespace PubLib.Backoffice.WebApp.Services.Publisher
{
    public class BookOrderPublisherService
    {
        private readonly BookOrderPublisher _publisher;
        private readonly ILogger<BookOrderPublisherService> _logger;

        public BookOrderPublisherService(BookOrderPublisher publisher, ILogger<BookOrderPublisherService> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public void PublishNewProvision(BookProvisionDto message)
        {
            _logger.LogInformation($"Publishing new provision message with {nameof(BookOrderPublisher)}.");
            _publisher.PublishNewProvision(message);
        }

        //public void PublishCancelReservation(BookItemDTO message)
        //{
        //    _logger.LogInformation($"Publishing new membership message with {nameof(BookOrderPublisher)}.");
        //    _publisher.PublishCancelledMembership(message);
        //}
    }
}
