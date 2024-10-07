using Microsoft.AspNetCore.Mvc;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Server.Services.Publisher;

namespace PubLib.FrontDesk.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FrontDeskController : ControllerBase
    {
        private readonly MembershipStatusPublisherService _membershipStatusPublisherService;

        private readonly BookOrderPublisherService _bookReservationPublisherService;

        public FrontDeskController(
            MembershipStatusPublisherService membershipStatusPublisherService,
            BookOrderPublisherService reservationPublisherService)
        {
            _membershipStatusPublisherService = membershipStatusPublisherService;
            _bookReservationPublisherService = reservationPublisherService;
        }

        [HttpPost("memberships/apply")]
        public IActionResult ApplyMembership(PersonDto personDto)
        {
            MembershipApplicationDto membershipApplicationDto = new MembershipApplicationDto { Person = personDto, Timestamp = DateTime.UtcNow };
            _membershipStatusPublisherService.PublishNewMembership(membershipApplicationDto);
            return Ok(new { message = "Membership applied." });
        }

        //[HttpPost("memberships/cancel")]
        //public IActionResult CancelMembership([FromBody] string message)
        //{
        //    MembershipApplicationDto messageObj = new MembershipApplicationDto { LastName = "Jackson", FirstName = "Alice", City = "Seattle", Timestamp = DateTime.UtcNow };
        //    _membershipStatusPublisherService.PublishCancelMembership(messageObj);
        //    return Ok(new { message = "Membership cancelled." });
        //}

        [HttpPost("reservations/create")]
        public IActionResult ReserveBook(BookReservationDto bookDto)
        {
            _bookReservationPublisherService.PublishNewReservation(bookDto);
            return Ok(new { message = "New reservation created." });
        }
    }
}
