using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Korzh.EasyQuery.Linq;

using EqAspNetCoreDemo.Models;

namespace EqAspNetCoreDemo.Pages
{
    public class FullTextSearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int MaxButtonCount { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string Text { get; set; }

        public int FirstPageIndex => CurrentPage - ((CurrentPage - 1) % MaxButtonCount);

        public int LastPageIndex
        {
            get {
                var result = FirstPageIndex + MaxButtonCount - 1;
                if (result > TotalPages) {
                    return TotalPages;
                }
                return result;
            }
        }

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => FirstPageIndex > 1;
        public bool EnableNext => LastPageIndex < TotalPages;

        public List<Customer> Data { get; set; }

        private readonly AppDbContext _dbContext;

        public FullTextSearchModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void OnGet()
        {
            var result = (!string.IsNullOrEmpty(Text))
                         ? _dbContext.Customers.FullTextSearchQuery(Text) 
                         : _dbContext.Customers;

            Count = result.Count();

            Data = result.OrderBy(c => c.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

        }
    }
}