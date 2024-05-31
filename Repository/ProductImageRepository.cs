using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Transactions;

namespace CraftShop.API.Repository
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext db;

        public ProductImageRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task CreateImage(ProductImageCreatedDTO dto)
        {

            if (dto == null)
            {
                return;
            }
            try
            {
                int maxContent = 5 * 1024 * 1024;
                ProductImage images = new ProductImage();
                images.Id = Guid.NewGuid().ToString();
                for (int i = 0; i < dto.Images.Count; i++)
                {
                    if (dto.Images[i] == null || dto.Images[i].Length == 0)
                        continue;
                    if (dto.Images[i].Length < maxContent)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Images[i].FileName;
                        string uploadsFolder = @"F:\document\CraftShop\frontend\src\assets";
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        switch (i)
                        {
                            case 0:
                                images.ImageThumb = Path.GetFileName(uniqueFileName);
                                break;
                            case 1:
                                images.Image1 = Path.GetFileName(uniqueFileName);
                                break;
                            case 2:
                                images.Image2 = Path.GetFileName(uniqueFileName);
                                break;
                            case 3:
                                images.Image3 = Path.GetFileName(uniqueFileName);
                                break;
                            case 4:
                                images.Image4 = Path.GetFileName(uniqueFileName);
                                break;


                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await dto.Images[i].CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                images.ProductId = dto.ProductId;
                await db.ProductImage.AddAsync(images);
                await db.SaveChangesAsync();
            }
            catch { }
        }

        public async Task<ProductImage> GetProductImage(int id)
        {
            var image  = await db.ProductImage.FirstOrDefaultAsync(u => u.ProductId == id);
            return image;
        }
    }
}
