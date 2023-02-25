using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services
{
    public class PictureService : IPictureService
    {
        private readonly ApplicationDbContext _dbContext;

        public PictureService(ApplicationDbContext dbContext) => _dbContext = dbContext;

		/// <exception cref="PictureNotFoundException"></exception>
        public async Task<(byte[], string)> GetImage(int eventId, int height)
        {
            Picture picture = await _dbContext.Pictures.Where(p=> p.EventId == eventId).FirstOrDefaultAsync();

            if (picture == null)
                throw new PictureNotFoundException();


            Image image = Image.Load("C:\\inetpub\\wwwroot\\E01-API\\images\\" + picture.Path);
            image.Mutate(i =>
            {
                i.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Max,
                    Size = new Size() 
                    { 
                        Height = height
                    }
                });
            });

            MemoryStream ms = new MemoryStream();

            image.SaveAsJpeg(ms);

            byte[] bytes = ms.ToArray();

            ///image.Save("C://images/resize/" + picture.Path);
            //byte[] bytes = System.IO.File.ReadAllBytes("C://images/resize/" + picture.Path);
            //var bytes = image.SavePixelData();
            var file = (bytes, "image/jpeg");

            return file;
        }

        public async Task<int> PlanPictureUpsert(IFormFile file, Event e)
        {
            //try
            {
                Image image = GetImageFromForm(file);

                //var e = await _dbContext.Events.Include(e => e.PlanPicture).FirstOrDefaultAsync(e => e.Id == eventId);

                if (e.PlanPicture == null)
                {
                    Picture picture = new Picture();

                    picture.EventId = e.Id;
                    picture.MimeType = "image/jpeg";
                    picture.Path = Guid.NewGuid().ToString() + ".jpg";
                    SaveImageToFile(image, picture.Path);


                    _dbContext.Pictures.Add(picture);
                }
                else
                    SaveImageToFile(image, e.PlanPicture.Path);
            }

            return e.PlanPicture.Id;
        }

        public virtual bool SaveImageToFile(Image image, string path)
        {
            image.SaveAsJpeg("C:\\inetpub\\wwwroot\\E01-API\\images\\" + path);
            return true;
        }

        public virtual Image GetImageFromForm(IFormFile file)
        {
            if (file == null)
                throw new NoFileGivenException();
            return Image.Load(file.OpenReadStream());
        }
    }

    public interface IPictureService
    {
        public Task<(byte[], string)> GetImage(int eventId, int height);
        public Task<int> PlanPictureUpsert(IFormFile file, Event e);
        public bool SaveImageToFile(Image image, string path);
        public Image GetImageFromForm(IFormFile file);
    }
}
