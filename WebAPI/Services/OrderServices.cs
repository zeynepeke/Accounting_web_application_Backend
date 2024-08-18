using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data; // Uygulamanızın veri bağlamı için kullanılan namespace
using WebAPI.Models; // Uygulamanızın modelleri için kullanılan namespace
using WebAPI.DTOs;   // Uygulamanızın DTO'ları için kullanılan namespace

public class OrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateOrderAsync(List<OrderDetailDto> orderDetailsDto)
    {
        // Stok kontrolü yap
        foreach (var detail in orderDetailsDto)
        {
            var product = await _context.Products.FindAsync(detail.ProductId);
            if (product == null)
            {
                return $"Ürün ID {detail.ProductId} bulunamadı.";
            }

            if (product.StockQuantity < detail.Quantity)
            {
                return $"Ürün ID {detail.ProductId} için yeterli stok bulunmuyor.";
            }
        }

        // Siparişi oluştur
        var order = new Order
        {
            OrderDate = DateTime.Now
        };

        _context.Orders.Add(order);

        foreach (var detail in orderDetailsDto)
        {
            var orderDetail = new OrderDetail
            {
                Order = order,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity
            };

            var product = await _context.Products.FindAsync(detail.ProductId);
            product.StockQuantity -= detail.Quantity;

            _context.OrderDetails.Add(orderDetail);
        }

        // Değişiklikleri kaydet
        await _context.SaveChangesAsync();

        return "Sipariş başarıyla oluşturuldu.";
    }

    public async Task<string> DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return "Sipariş bulunamadı.";
        }

        // İlgili sipariş detaylarını da silmek için (isteğe bağlı)
        var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        _context.OrderDetails.RemoveRange(orderDetails);

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return "Sipariş başarıyla silindi.";
    }
}
/*
Constructor: ApplicationDbContext nesnesini alarak _context alanına atar. Bu, veri erişimi için kullanılan bağlamdır.

CreateOrderAsync Metodu:

Stok Kontrolü: Her sipariş detayını kontrol eder ve ürünün stok miktarını doğrular.
Sipariş Oluşturma: Yeni bir sipariş oluşturur ve Orders tablosuna ekler.
Sipariş Detaylarını Eklemek: Sipariş detaylarını OrderDetails tablosuna ekler ve stok miktarını günceller.
Değişiklikleri Kaydetmek: Veritabanındaki tüm değişiklikleri kaydeder.
DeleteOrderAsync Metodu:

Siparişi Bulma: Silinmek istenen siparişi veritabanında bulur.
Sipariş Detaylarını Silme: Siparişle ilişkili tüm detayları siler.
Siparişi Silmek: Siparişi Orders tablosundan siler.
Değişiklikleri Kaydetmek: Veritabanındaki tüm değişiklikleri kaydeder.
 
 */