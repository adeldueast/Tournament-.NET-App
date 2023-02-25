using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services_Tests
{
    public class PictureServiceTest : ServiceTestBase<PictureService>
    {
        [Category("UpsertPicture")]
        [TestCase(1)]//create
        public async Task PlanPictureUpsert_Picture_Added(int eventId)
        {
            // Arrange.
            Image sourceImg = Image.Load(@"C:\Project\H23-6PX-E01-API\API\LANPartyAPI_Services_Tests\imagesMock\1234.jpg");
            var domaine = await _context.Events.Include(e => e.PlanPicture).FirstOrDefaultAsync(e => e.Id == eventId);

            Mock<PictureService> service = new Mock<PictureService>(_context);

            service.Setup(s => s.GetImageFromForm(It.IsAny<IFormFile>())).Returns(sourceImg);
            service.Setup(s => s.SaveImageToFile(It.IsAny<Image>(), It.IsAny<string>())).Returns(true);

            //Act
            int pId = await service.Object.PlanPictureUpsert(null, domaine);


            //Assert
            Assert.That(pId, Is.EqualTo(domaine.PlanPicture.Id));
        }

        [Category("UpsertPicture")]
        [Test]//create
        public async Task GetImageFromForm_File_Null()
        {
            // Arrange.

            //Act
            NoFileGivenException exception = Assert.ThrowsAsync<NoFileGivenException>(async () => _sut.GetImageFromForm(null));

            //Assert
            Assert.Throws<NoFileGivenException>(() => throw new NoFileGivenException());
            await Task.CompletedTask;

        }

        /*[Category("UpsertPicture")]
        [Test]//create
        public async Task PlanPictureUpsert_Picture_Changed()
        {
            // Arrange.
            Image sourceImg1 = Image.Load(@"C:\Project\H23-6PX-E01-API\API\LANPartyAPI_Services_Tests\imagesMock\4723250.jpg");
            Image sourceImg2 = Image.Load(@"C:\Project\H23-6PX-E01-API\API\LANPartyAPI_Services_Tests\imagesMock\imgChanged.jpeg");

            Mock<PictureService> service = new Mock<PictureService>(_context);

            service.Setup(s=> s.GetImageFromForm(It.IsAny<IFormFile>())).Returns(sourceImg1);



            //Act
            int pId = await service.Object.PlanPictureUpsert(null, domaine);
            //Picture p = await _sut.PlanPictureUpsert(null, 2);


            //Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
            await Task.CompletedTask;
        }*/

        public override void SeedDatabase()
        {
            Event e1 = new Event
            {
                Id = 1,
                Name = "LAN01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3),
                MaxPlayerNumber = 15,
                Description = "A description.."
            };

            Event e2 = new Event
            {
                Id = 2,
                Name = "LAN02",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3),
                MaxPlayerNumber = 15,
                Description = "A description.."
            };

            _context.AddRange(e1, e2);
        }
    }
}
