using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.ViewComponents
{
    [ViewComponent]
    public class AddNewReplyViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string reviewId)
        {
            return View(new ReplyDto { ReviewId = reviewId });
        }
    }
}
