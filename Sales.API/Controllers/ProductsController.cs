﻿using Sales.API.Helpers;
using Sales.Common.Models;
using Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sales.API.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Products
        public IEnumerable<Product> GetProducts()
        {
            return db.Products.OrderBy(p => p.Description).ToList();
        }
        //[HttpGet]
        //public Reply GetProducts()
        //{
        //    Reply oReply = new Reply();
        //    oReply.result = 0;
        //    using (DataContext db = new DataContext())
        //    {
        //        List<Product> lstProducts = (from d in db.Products
        //                  select new Product
        //                  {
        //                      ProductId = d.ProductId,
        //                      Description = d.Description,
        //                      Price = d.Price,
        //                      IsAvailable = d.IsAvailable,
        //                      PublishOn = d.PublishOn
        //                  }).ToList();
        //        oReply.result = 1;
        //        oReply.data = lstProducts;
        //    }
        //    return oReply;
        //}

        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            var products = await this.db.Products.Where(p => p.CategoryId == id).ToListAsync();
            return Ok(products);
        }


        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Products";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    product.ImagePath = fullPath;
                }
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            product.IsAvailable = true;
            product.PublishOn = DateTime.Now.ToUniversalTime();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Products";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    product.ImagePath = fullPath;
                }
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}