using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.BookOrder;

namespace PubLib.Server.Services.Publisher
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

        public void PublishNewReservation(BookReservationDto bookReservation)
        {
            if (bookReservation is null)
            {
                throw new ArgumentNullException(nameof(bookReservation));
            }

            _logger.LogInformation($"Publishing new reservation message with {nameof(BookOrderPublisher)}.");
            _publisher.PublishNewReservation(bookReservation);
        }

        //public void PublishCancelReservation(BookItemDTO message)
        //{
        //    _logger.LogInformation($"Publishing new membership message with {nameof(BookOrderPublisher)}.");
        //    _publisher.PublishCancelledMembership(message);
        //}
    }
}
