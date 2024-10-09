using Central_Service.Core;
using Central_Service.Interface;
using Central_Service.DTO;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using ExtensionMethods;

namespace Central_Service.Service;

public class AppDataService : ServiceBase, IAppDataService
{
    #region Private Declarations

    private readonly IRepository<Category> _categories;
    private readonly IRepository<Product> _products;
    private readonly IRepository<Image> _images;
    private readonly IRepository<Label> _labels;
    private readonly IRepository<ApiLog> _logService;
    private readonly EFContext _dbcontext;
    private readonly IObjectFactory _factory;

    #endregion

    public AppDataService( EFContext dbcontext, ILogger<AppDataService> logger, IRepository<Category> categories, IRepository<Product> products, IRepository<Image> images, IRepository<Label> labels, IRepository<ApiLog> logService, IServiceProvider serviceProvider, IObjectFactory factory ) : base(logger, serviceProvider)
    {
        _categories = categories;
        _products = products;
        _images = images;
        _labels = labels;
        _dbcontext = dbcontext;
        _factory = factory;
        _logService = logService;
    }

    #region Private Methods

    private string BuildImageUrl( Uri baseUri, int imageSrl, Dictionary<int, string> imageDict )
    {
        return new Uri(baseUri, $"/Images/{imageSrl}{imageDict[imageSrl]}").ToString();
    }

    private async Task LogAsync( string methodName, string logType, string logMessage, string exception = "" )
    {
        await _logService.Add(new ApiLog
        {
            log_origin = $"AppDataService.{methodName}-{logType}",
            log = logMessage,
            Exception = exception,
            DateTime = DateTime.UtcNow
        });
    }

    #endregion

    #region Services

    public async Task<string> AppConfig()
    {
        var repo = GetService<IRepository<ControlMaster>>();

        string? mapkey = (await repo.Find(x => x.id == 1))[0].gmapkey;
        return mapkey ?? "";
    }

    public async Task<AppDataDTO> HomePageData( string baseUrl )
    {
        var output = new AppDataDTO();
        var baseUri = new Uri(baseUrl);

        try
        {
            var imagesTask = _images.GetAll();
            var labelsTask = _labels.GetAll();
            var categoriesTask = _categories.GetAll();
            var featuredProductsTask = _products.Find(prd => prd.IsFeatured);
            var controlsTask = GetService<IRepository<ControlMaster>>().Find(x => x.id == 1);
            await Task.WhenAll(imagesTask, labelsTask, categoriesTask, featuredProductsTask, controlsTask);
            var images = await imagesTask;
            var labels = await labelsTask;
            var categories = await categoriesTask;
            var featuredProducts = await featuredProductsTask;
            var controls = await controlsTask;
            var imageDict = images.ToDictionary(img => img.Image_Srl, img => img.Image_Type);
            var control = controls.FirstOrDefault();
            output.DefaultSearchBanner = BuildImageUrl(baseUri, control?.defaultSearchImg ?? 0, imageDict);
            output.Label = labels.OrderBy(ex => ex.Label_Id).Select(lb => lb.Labeld).ToList();
            output.CarouselUrls = images.Where(img => img.IsCarousel)
                                        .Select(img => BuildImageUrl(baseUri, img.Image_Srl, imageDict))
                                        .ToList();
            output.Categories = categories
                .OrderBy(cat => cat.CategoryName)
                .Select(cat => _factory.BuildCategoryDto(cat, BuildImageUrl(baseUri, cat.Image_Srl, imageDict)))
                .Where(dto => dto.Image_Url != null)
                .ToList();
            output.FeaturedProducts = featuredProducts
                .OrderBy(prd => prd.Product_Name)
                .Select(product => _factory.BuildProductDto(product, BuildImageUrl(baseUri, product.Image_Srl, imageDict)))
                .Where(dto => dto.Image_Url != null)
                .ToList();

            //await LogAsync(nameof(HomePageData), LogUtil.Response, output.JSONStringify());
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"Method name: {nameof(HomePageData)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify()}");
            //await LogAsync(nameof(HomePageData), LogUtil.Response, ex.JSONStringify(), ex.Message);
            output = null;
        }

        return output;
    }



    public async Task<SearchModel> SeachResult( string baseUrl, string category, string searchQuery )
    {
        var output = new SearchModel();
        try
        {
            var imagesTask = _images.GetAll();
            var selectedCategoryTask = _categories.Find(cat => cat.CategoryName == category);
            await Task.WhenAll(imagesTask, selectedCategoryTask);
            var images = await imagesTask;
            var selectedCategory = (await selectedCategoryTask).FirstOrDefault();

            var baseUri = new Uri(baseUrl);
            var imageDict = images.ToDictionary(img => img.Image_Srl, img => img.Image_Type);

            if (selectedCategory != null)
            {
                var products = (await _products.Find(prd => prd.Category_Id == selectedCategory.Category_Id && prd.Product_Name.ToLower().Contains(searchQuery.Trim().ToLower()))).OrderBy(prd => prd.Product_Name);
                output.CategoryId = selectedCategory.Category_Id;
                output.CategoryName = selectedCategory.CategoryName;
                output.CategoryImageUrl = BuildImageUrl(baseUri, selectedCategory.Image_Srl, imageDict);

                output.Result = products.Select(ex => _factory.BuildProductDto(ex, BuildImageUrl(baseUri, ex.Image_Srl, imageDict))).ToList();
            }
            else if (category.ToLower() == "all")
            {
                var products = await _products.Find(prd => prd.Product_Name.ToLower().Contains(searchQuery.Trim().ToLower()));
                output.CategoryId = 0;
                output.CategoryName = "All";
                output.Result = products.Select(ex => _factory.BuildProductDto(ex, BuildImageUrl(baseUri, ex.Image_Srl, imageDict))).ToList();
            }
            else
            {
                output = null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"Method name: {nameof(SeachResult)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify()}");
        }
        return output;
    }

    public async Task<int> SaveCart( List<ProductDto> Cart, int userId )
    {
        try
        {
            var repo = GetService<IRepository<User>>();
            if (repo == null) return -1;
            User? user = await repo.GetById(userId);
            if (user == null) return -1;
            user.Cart = Cart.JSONStringify();
            await repo.Update(user);
            return 1;
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"Method name: {nameof(SaveCart)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify()}");
        }
        return 0;
    }

    #endregion
}
