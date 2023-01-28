using System;
using SixLabors.ImageSharp;
using SkiaSharp;


namespace InternetMarket.Models.ViewModels.Ad;

public class CaptchaImage
{
    private string text; // текст капчи
    private int width; // ширина картинки
    private int height; // высота картинки
    public string PathToImage;
    private SKBitmap Image;
    public CaptchaImage(string s, int width, int height)
    {
        text = s;
        this.width = width;
        this.height = height;
        GenerateImage();
    }

    // создаем изображение
    private void GenerateImage()
    {
        SKBitmap bmp = new(150, 250);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Black);
        using (SKPaint textPaint = new SKPaint())
        using (SKTypeface tf = SKTypeface.FromFamilyName("Courier New"))
        {
            textPaint.Color = SKColors.DarkRed;
            textPaint.IsAntialias = true;
            textPaint.TextSize = 22;
            canvas.DrawText(text, 50, 50, textPaint);
        }
        PathToImage = text + Guid.NewGuid().ToString() + ".jpg";
        SKFileWStream fs = new("wwwroot/images/" + PathToImage);
        bmp.Encode(fs, SKEncodedImageFormat.Jpeg, quality: 100);
    }

    ~CaptchaImage()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Image.Dispose();
    }
}